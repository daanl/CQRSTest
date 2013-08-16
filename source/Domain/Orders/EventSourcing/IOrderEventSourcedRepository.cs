using System;
using System.Threading.Tasks;

namespace Domain.Orders.EventSourcing
{
    public interface IOrderEventSourcedRepository
    {
        Task Save(Order order, Guid correlationId);
        Task<Order> Get(Guid gameServerId);
    }
}