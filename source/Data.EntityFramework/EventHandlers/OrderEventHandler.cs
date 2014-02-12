using System.Data.Entity;
using System.Threading.Tasks;
using Domain.Orders.Events;
using Domain.Orders.ReadModel;
using Infrastructure.Events;

namespace Data.EntityFramework.EventHandlers
{
    public class OrderEventHandler :
        IEventHandler<OrderCreated>, IOrderEventHandler
    {
        private readonly CqrsContext _context;

        public OrderEventHandler(CqrsContext context)
        {
            _context = context;
        }

        public async Task Handle(OrderCreated e)
        {
            var gameServer = await _context.OrdersDtos.FindAsync(e.SourceId);

            if (gameServer != null)
            {
                
            }
            else
            {
                gameServer = new OrderDto
                {
                    OrderId = e.SourceId
                };

                _context.OrdersDtos.Add(gameServer);
                await _context.SaveChangesAsync();
            }
        }
    }
}