using System;
using Infrastructure.Events;

namespace Domain.Orders.EventSourcing
{
    public class OrderEvent : IVersionedEvent
    {
        public Guid Id { get; set; }
        public Guid SourceId { get; set; }
        public int Version { get; set; }
        public Guid CorrelationId { get; set; }
        public string Payload { get; set; }
    }
}