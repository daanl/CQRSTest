using System;
using CqrsTests.Infrastructure.Events;

namespace CqrsTests.GameServers.Events
{
    public class GameServerCreated : VersionedEvent
    {
        public Guid UserId { get; set; }
    }
}