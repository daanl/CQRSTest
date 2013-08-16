using System.Threading.Tasks;
using Domain.Orders.EventSourcing;
using Infrastructure.Commands;

namespace Domain.Orders.Commands
{
    public class OrderCommandHandler : 
        ICommandHandler<CreateOrder>
    {
        private readonly IOrderEventSourcedRepository _repository;

        public OrderCommandHandler(
            IOrderEventSourcedRepository repository
        )
        {
            _repository = repository;
        }

        public async Task Handle(CreateOrder command)
        {
            var gameServer = new Order(command.OrderId, command.UserId);

            await _repository.Save(gameServer, command.CorrelationId);
        }
    }
}