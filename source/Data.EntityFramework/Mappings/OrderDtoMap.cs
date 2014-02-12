using System.Data.Entity.ModelConfiguration;
using Domain.Orders.ReadModel;

namespace Data.EntityFramework.Mappings
{
    public class OrderDtoMap : EntityTypeConfiguration<OrderDto>
    {
        public OrderDtoMap()
        {
            HasKey(x => x.OrderId);

            ToTable("OrdersDtos", "public");

            Property(x => x.OrderId);
        }
    }
}
