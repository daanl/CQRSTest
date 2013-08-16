using System;
using Domain.Orders.Events;
using Infrastructure.Events;

namespace Domain.Orders
{
    public class Order : EventSourced
    {
        protected Order(Guid id) : base(id)
        {
            Handles<OrderCreated>(OnOrderCreated);
        }

        public Order(Guid id, Guid userId) : this(id)
        {
            Update(new OrderCreated
            {
                UserId = userId
            });
        }

        public Guid CreatedByUserId { get; private set; }

        protected void OnOrderCreated(OrderCreated e)
        {
            CreatedByUserId = e.UserId;
        }
    }
}