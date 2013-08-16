using System.Collections.Generic;
using System.Threading.Tasks;
using Domain.Orders.ReadModel;
using Microsoft.AspNet.SignalR;
using Raven.Client;

namespace Web.Hubs
{
    public class OrderHub : Hub
    {
        private readonly IAsyncDocumentSession _session;

        public OrderHub(
            IAsyncDocumentSession session    
        )
        {
            _session = session;
        }

        public async Task<IList<OrderDto>> Orders()
        {
            return await _session.Query<OrderDto>()
                                .ToListAsync();
        }
    }
}