using System;
using System.Collections.Generic;

namespace Infrastructure.Events
{
    public interface IEventSourced
    {
        Guid Id { get; }

        int Version { get; }

        IEnumerable<IVersionedEvent> Events { get; }
    }
}