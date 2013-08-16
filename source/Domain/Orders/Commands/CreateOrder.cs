using System;
using Infrastructure.Commands;

namespace Domain.Orders.Commands
{
    public class CreateOrder : ICommand
    {
        public CreateOrder(Guid userId)
        {
            UserId = userId;
            CorrelationId = Guid.NewGuid();
            OrderId = Guid.NewGuid();
        }

        public Guid CorrelationId { get; private set; }
        public Guid OrderId { get; private set; }
        public Guid UserId { get; private set; }
    }
}