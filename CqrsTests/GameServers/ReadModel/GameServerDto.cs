using System;

namespace CqrsTests.GameServers.ReadModel
{
    public class GameServerDto
    {
        public GameServerDto(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
