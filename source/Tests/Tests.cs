using System;
using System.Threading;
using System.Threading.Tasks;
using Data.EntityFramework;
using Domain.Orders.Commands;
using Domain.Orders.EventSourcing;
using Domain.Orders.Events;
using Domain.Orders.ReadModel;
using Infrastructure.Commands;
using Infrastructure.Events;
using NUnit.Framework;
using Newtonsoft.Json;
using Raven.Client.Embedded;

namespace CqrsTests
{
    [TestFixture]
    public class Tests
    {
        [Test]
        public async Task Can_Save_OrderEvent_With_EntityFramework_And_PostgresSQL()
        {
            using (var context = new CqrsContext())
            {
                var orderEvent = new OrderEvent
                {
                    Id = Guid.NewGuid(),    
                    Payload = JsonConvert.SerializeObject(new { test = "hoi"})
                };

                context.OrdersEvents.Add(orderEvent);

                await context.SaveChangesAsync();

                var cancellationTokenSource = new CancellationTokenSource();
                var orderEventFromDatabase = await context.OrdersEvents.FindAsync(cancellationTokenSource.Token, orderEvent.Id);

                Assert.AreEqual(orderEvent.Id, orderEventFromDatabase.Id);
                Assert.AreEqual("hoi", (string)JsonConvert.DeserializeObject<dynamic>(orderEvent.Payload).test);
            }
        }

        [Test]
        public async Task Can_Do_Something()
        {
            // create document store
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data"
            };

            documentStore.Initialize();


            // open session
            using (var session = documentStore.OpenAsyncSession())
            {
                // create event bus that will handle the events (after saving from repository)
                var eventBus = new EventBus();
                var orderEventHandler = new OrderEventHandler(session);

                // register order event handler
                eventBus.Register(orderEventHandler);

                // setup event repository
                var repository = new OrderEventSourcedRepository(session, eventBus);
                var orderCommandHandler = new OrderCommandHandler(repository);

                // setup command bus
                var commandBus = new CommandBus();

                // register order command handler
                commandBus.Register(orderCommandHandler);

                // create new order command
                var command = new CreateOrder(Guid.NewGuid());

                // send the command
                await commandBus.SendAsync(command);

                // get the order dto
                var orderDto = await session.LoadAsync<OrderDto>(command.OrderId);

                // get order from events
                var order = await repository.Get(command.OrderId);

                Assert.AreEqual(command.OrderId, orderDto.Id);
                Assert.AreEqual(command.UserId, order.CreatedByUserId);
            }
        }
    }
}
