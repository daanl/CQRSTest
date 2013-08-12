using System;

namespace CqrsTests.Infrastructure.Events
{
    public interface IEvent
    {
        Guid SourceId { get; }
    }
}