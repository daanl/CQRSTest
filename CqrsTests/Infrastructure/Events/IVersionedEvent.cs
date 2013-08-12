namespace CqrsTests.Infrastructure.Events
{
    public interface IVersionedEvent : IEvent
    {
        int Version { get; }
    }
}