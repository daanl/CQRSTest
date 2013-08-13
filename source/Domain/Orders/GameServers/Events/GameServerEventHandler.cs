using System.Threading.Tasks;
using CqrsTests.GameServers.ReadModel;
using CqrsTests.Infrastructure.Events;
using Raven.Client;

namespace CqrsTests.GameServers.Events
{
    public class GameServerEventHandler :
        IEventHandler<GameServerCreated>
    {
        private readonly IAsyncDocumentSession _session;

        public GameServerEventHandler(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task Handle(GameServerCreated e)
        {
            var gameServer = await _session.LoadAsync<GameServerDto>(e.SourceId);

            if (gameServer != null)
            {
                
            }
            else
            {
                gameServer = new GameServerDto(e.SourceId);

                await _session.StoreAsync(gameServer);
                await _session.SaveChangesAsync();
            }
        }
    }
}