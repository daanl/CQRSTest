using System;
using CqrsTests.Infrastructure.Commands;

namespace CqrsTests.GameServers.Commands
{
    public class CreateGameServer : ICommand
    {
        public CreateGameServer(Guid userId)
        {
            UserId = userId;
            CorrelationId = Guid.NewGuid();
            GameServerId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; private set; }
        public Guid GameServerId { get; private set; }
        public Guid UserId { get; private set; }
    }
}