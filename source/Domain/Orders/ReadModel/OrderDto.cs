using System;

namespace Domain.Orders.ReadModel
{
    public class OrderDto
    {
        public OrderDto(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
