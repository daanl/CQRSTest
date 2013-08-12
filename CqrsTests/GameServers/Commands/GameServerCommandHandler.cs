using System.Threading.Tasks;
using CqrsTests.GameServers.EventSourcing;
using CqrsTests.Infrastructure.Commands;

namespace CqrsTests.GameServers.Commands
{
    public class GameServerCommandHandler : 
        ICommandHandler<CreateGameServer>
    {
        private readonly IGameServerEventSourcedRepository _repository;

        public GameServerCommandHandler(
            IGameServerEventSourcedRepository repository
        )
        {
            _repository = repository;
        }

        public async Task Handle(CreateGameServer command)
        {
            var gameServer = new GameServer(command.GameServerId, command.UserId);

            await _repository.Save(gameServer, command.CorrelationId);
        }
    }
}