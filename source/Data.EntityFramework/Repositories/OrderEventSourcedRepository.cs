using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Threading.Tasks;
using Domain.Orders;
using Domain.Orders.EventSourcing;
using Infrastructure.CQRS.Serializers;
using Infrastructure.Events;

namespace Data.EntityFramework.Repositories
{
    public class OrderEventSourcedRepository : IOrderEventSourcedRepository
    {
        private readonly CqrsContext _context;
        private readonly IEventBus _eventBus;
        private readonly IEventSerializer _eventSerializer;

        public OrderEventSourcedRepository(
            CqrsContext context,
            IEventBus eventBus,
            IEventSerializer eventSerializer
        )
        {
            _context = context;
            _eventBus = eventBus;
            _eventSerializer = eventSerializer;
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
                    Payload = await _eventSerializer.Serialize(e)
                };

                _context.OrdersEvents.Add(ev);
            }

            await _context.SaveChangesAsync();

            await _eventBus.Publish(events, correlationId);
        }

        public async Task<Order> Get(Guid gameServerId)
        {
            var constructor = typeof(Order).GetConstructor(new[] { typeof(Guid) });
            if (constructor == null)
            {
                throw new InvalidCastException("Type T must have a constructor with the following signature: .ctor(Guid)");
            }

            var events = await _context.OrdersEvents
                                 .Where(x => x.SourceId == gameServerId)
                                 .OrderBy(x => x.Version)
                                 .ToListAsync();
                                 

            var gameServer = (Order)constructor.Invoke(new object[] { gameServerId, events.Select(async x => await _eventSerializer.Deserialize<IVersionedEvent>(x.Payload)) });

            return gameServer;
        }
    }
}