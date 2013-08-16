using System.Data.Entity.ModelConfiguration;
using Domain.Orders.EventSourcing;

namespace Data.EntityFramework.Mappings
{
    public class OrderEventMap : EntityTypeConfiguration<OrderEvent>
    {
        public OrderEventMap()
        {
            HasKey(x => x.Id);

            ToTable("OrdersEvents", "public");

            Property(x => x.CorrelationId);
            Property(x => x.SourceId);
            Property(x => x.Payload);
            Property(x => x.Version);
        }
    }
}
