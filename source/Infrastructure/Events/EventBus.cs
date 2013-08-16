using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public class EventBus : IEventBus
    {
        private IDictionary<Type, IEventHandler> _handlers = new Dictionary<Type, IEventHandler>();

        public void Register(IEventHandler eventHandler)
        {
            var genericHandler = typeof(IEventHandler<>);
            var supportedEventTypes = eventHandler.GetType()
                                                .GetInterfaces()
                                                .Where(iface => iface.IsGenericType && iface.GetGenericTypeDefinition() == genericHandler)
                                                .Select(iface => iface.GetGenericArguments()[0])
                                                .ToList();

            if (_handlers.Keys.Any(supportedEventTypes.Contains))
            {
                throw new ArgumentException("The event handled by the received handler already has a registered handler.");
            }

            // Register this handler for each of the handled types.
            foreach (var eventType in supportedEventTypes)
            {
                _handlers.Add(eventType, eventHandler);
            }
        }

        public async Task Publish(IEnumerable<IVersionedEvent> events, Guid correlationId)
        {
            foreach (var e in events)
            {
                await Publish(e, correlationId);
            }
        }

        public async Task Publish(IVersionedEvent e, Guid correlationId)
        {
            IEventHandler eventHandler;
            
            if (_handlers.TryGetValue(e.GetType(), out eventHandler))
            {
                await ((dynamic)eventHandler).Handle((dynamic)e);
            }
        }
    }
}