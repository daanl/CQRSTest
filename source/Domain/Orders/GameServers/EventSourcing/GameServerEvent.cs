using System;
using CqrsTests.Infrastructure.Events;

namespace CqrsTests.GameServers.EventSourcing
{
    public class GameServerEvent : IVersionedEvent
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public int Version { get; set; }
        public Guid CorrelationId { get; set; }
        public IVersionedEvent Payload { get; set; }
    }
}