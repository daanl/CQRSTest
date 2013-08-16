using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Infrastructure.Events;
using Raven.Client;

namespace Domain.Orders.EventSourcing
{
    public class OrderEventSourcedRepository : IOrderEventSourcedRepository
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IEventBus _eventBus;

        public OrderEventSourcedRepository(
            IAsyncDocumentSession session,
            IEventBus eventBus
        )
        {
            _session = session;
            _eventBus = eventBus;
        }

        public async Task Save(Order order, Guid correlationId)
        {
            var events = order.Events.ToList();

            foreach (var e in events)
            {
                var ev = new OrderEvent
                {
                    SourceId = e.SourceId,
                    Version = e.Version,
                    CorrelationId = correlationId,
                    //Payload = e
                };

                await _session.StoreAsync(ev);
            }

            await _session.SaveChangesAsync();

            await _eventBus.Publish(events, correlationId);
        }

        public async Task<Order> Get(Guid gameServerId)
        {
            var constructor = typeof(Order).GetConstructor(new[] { typeof(Guid), typeof(IEnumerable<IVersionedEvent>) });
            if (constructor == null)
            {
                throw new InvalidCastException("Type T must have a constructor with the following signature: .ctor(Guid, IEnumerable<IVersionedEvent>)");
            }

            var events = await _session.Query<OrderEvent>()
                                 .Where(x => x.SourceId == gameServerId)
                                 .OrderBy(x => x.Version)
                                 .ToListAsync();
                                 

            var gameServer = (Order)constructor.Invoke(new object[] { gameServerId, events.Select(x => x.Payload) });

            return gameServer;
        }
    }
}