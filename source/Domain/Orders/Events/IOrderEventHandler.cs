using System.Threading.Tasks;

namespace Domain.Orders.Events
{
    public interface IOrderEventHandler
    {
        Task Handle(OrderCreated e);
    }
}