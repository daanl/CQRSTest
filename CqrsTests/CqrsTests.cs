using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CqrsTests.GameServers;
using CqrsTests.GameServers.Commands;
using CqrsTests.GameServers.EventSourcing;
using CqrsTests.GameServers.Events;
using CqrsTests.GameServers.ReadModel;
using CqrsTests.Infrastructure.Commands;
using CqrsTests.Infrastructure.Events;
using NUnit.Framework;
using Raven.Client.Embedded;

namespace CqrsTests
{
    [TestFixture]
    public class CqrsTests
    {
        [Test]
        public async Task Can_Do_Something()
        {
            var documentStore = new EmbeddableDocumentStore
            {
                DataDirectory = "Data"
            };

            documentStore.Initialize();

            using (var session = documentStore.OpenAsyncSession())
            {
                var eventBus = new EventBus();
                var gameServerEventHandler = new GameServerEventHandler(session);
                eventBus.Register(gameServerEventHandler);

                var repository = new GameServerEventSourcedRepository(session, eventBus);
                var gameServerCommandHandler = new GameServerCommandHandler(repository);

                var commandBus = new CommandBus();
                commandBus.Register(gameServerCommandHandler);

                var command = new CreateGameServer(Guid.NewGuid());

                await commandBus.SendAsync(command);

                await Task.Delay(TimeSpan.FromSeconds(1));

                var gameServerDto = await session.LoadAsync<GameServerDto>(command.GameServerId);

                var gameServer = await repository.Get(command.GameServerId);
            }
        }
    }
}
