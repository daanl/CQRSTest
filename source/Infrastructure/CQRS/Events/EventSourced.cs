using System;
using System.Collections.Generic;
using Infrastructure.Events;

namespace Infrastructure.CQRS.Events
{
    public abstract class EventSourced : IEventSourced
    {
        private List<IVersionedEvent> _pendingEvents = new List<IVersionedEvent>();
        private Dictionary<Type, Action<IVersionedEvent>> _handlers = new Dictionary<Type, Action<IVersionedEvent>>();

        protected EventSourced(Guid id)
        {
            Id = id;

            SetupHandlers();
        }

        public abstract void SetupHandlers();

        public Guid Id { get; private set; }
        
        public int Version { get; protected set; }

        public IEnumerable<IVersionedEvent> Events
        {
            get { return _pendingEvents; }
        }

        protected void Handles<TEvent>(Action<TEvent> handler)
           where TEvent : IEvent
        {
            _handlers.Add(typeof(TEvent), e => handler((TEvent)e));
        }

        protected void LoadFrom(IEnumerable<IVersionedEvent> pastEvents)
        {
            foreach (var e in pastEvents)
            {
                _handlers[e.GetType()].Invoke(e);
                Version = e.Version;
            }
        }

        protected void Update(VersionedEvent e)
        {
            e.SourceId = Id;
            e.Version = Version + 1;
            _handlers[e.GetType()].Invoke(e);
            Version = e.Version;
            _pendingEvents.Add(e);
        }
    }
}