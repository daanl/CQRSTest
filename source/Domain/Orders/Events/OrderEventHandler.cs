using System.Threading.Tasks;
using Domain.Orders.ReadModel;
using Infrastructure.Events;
using Raven.Client;

namespace Domain.Orders.Events
{
    public class OrderEventHandler :
        IEventHandler<OrderCreated>
    {
        private readonly IAsyncDocumentSession _session;

        public OrderEventHandler(IAsyncDocumentSession session)
        {
            _session = session;
        }

        public async Task Handle(OrderCreated e)
        {
            var gameServer = await _session.LoadAsync<OrderDto>(e.SourceId);

            if (gameServer != null)
            {
                
            }
            else
            {
                gameServer = new OrderDto(e.SourceId);

                await _session.StoreAsync(gameServer);
                await _session.SaveChangesAsync();
            }
        }
    }
}