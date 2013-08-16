using System;
using Infrastructure.Events;

namespace Domain.Orders.Events
{
    public class OrderCreated : VersionedEvent
    {
        public Guid UserId { get; set; }
    }
}