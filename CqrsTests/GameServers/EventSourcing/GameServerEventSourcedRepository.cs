using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CqrsTests.Infrastructure.Events;
using Raven.Client;

namespace CqrsTests.GameServers.EventSourcing
{
    public class GameServerEventSourcedRepository : IGameServerEventSourcedRepository
    {
        private readonly IAsyncDocumentSession _session;
        private readonly IEventBus _eventBus;

        public GameServerEventSourcedRepository(
            IAsyncDocumentSession session,
            IEventBus eventBus
        )
        {
            _session = session;
            _eventBus = eventBus;
        }

        public async Task Save(GameServer gameServer, Guid correlationId)
        {
            var events = gameServer.Events.ToList();

            foreach (var e in events)
            {
                var ev = new GameServerEvent
                {
                    SourceId = e.SourceId,
                    Version = e.Version,
                    CorrelationId = correlationId,
                    Payload = e
                };

                await _session.StoreAsync(ev);
            }

            await _session.SaveChangesAsync();

            await _eventBus.Publish(events, correlationId);
        }

        public async Task<GameServer> Get(Guid gameServerId)
        {
            var constructor = typeof(GameServer).GetConstructor(new[] { typeof(Guid), typeof(IEnumerable<IVersionedEvent>) });
            if (constructor == null)
            {
                throw new InvalidCastException("Type T must have a constructor with the following signature: .ctor(Guid, IEnumerable<IVersionedEvent>)");
            }

            var events = await _session.Query<GameServerEvent>()
                                 .Where(x => x.SourceId == gameServerId)
                                 .OrderBy(x => x.Version)
                                 .ToListAsync();
                                 

            var gameServer = (GameServer)constructor.Invoke(new object[] { gameServerId, events.Select(x => x.Payload) });

            return gameServer;
        }
    }
}