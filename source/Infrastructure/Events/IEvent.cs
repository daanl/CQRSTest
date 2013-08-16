using System;

namespace Infrastructure.Events
{
    public interface IEvent
    {
        Guid SourceId { get; }
    }
}