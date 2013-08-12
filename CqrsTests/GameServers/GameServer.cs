using System;
using System.Collections.Generic;
using CqrsTests.GameServers.Events;
using CqrsTests.Infrastructure.Events;

namespace CqrsTests.GameServers
{
    public class GameServer : EventSourced
    {
        protected GameServer(Guid id) : base(id)
        {
            Handles<GameServerCreated>(OnGameServerCreated);
        }

        public GameServer(Guid id, IEnumerable<IVersionedEvent> pastEvents)
            : this(id)
        {
            LoadFrom(pastEvents);
        }

        public GameServer(Guid id, Guid userId) : this(id)
        {
            Update(new GameServerCreated
            {
                UserId = userId
            });
        }

        public Guid CreatedByUserId { get; private set; }

        protected void OnGameServerCreated(GameServerCreated e)
        {
            CreatedByUserId = e.UserId;
        }
    }
}