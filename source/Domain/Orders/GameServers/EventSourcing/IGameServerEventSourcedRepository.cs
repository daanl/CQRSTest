using System;
using System.Threading.Tasks;

namespace CqrsTests.GameServers.EventSourcing
{
    public interface IGameServerEventSourcedRepository
    {
        Task Save(GameServer gameServer, Guid correlationId);
        Task<GameServer> Get(Guid gameServerId);
    }
}