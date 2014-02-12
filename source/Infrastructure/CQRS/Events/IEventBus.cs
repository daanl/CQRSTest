using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Infrastructure.Events
{
    public interface IEventBus
    {
        Task Publish(IEnumerable<IVersionedEvent> events, Guid correlationId);
    }
}