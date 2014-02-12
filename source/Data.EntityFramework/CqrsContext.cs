using System.Data.Entity;
using Data.EntityFramework.Mappings;
using Domain.Orders.EventSourcing;
using Domain.Orders.ReadModel;

namespace Data.EntityFramework
{
    public class CqrsContext : DbContext
    {
        public CqrsContext() : base("CqrsContext")
        {
            
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new OrderEventMap());
            modelBuilder.Configurations.Add(new OrderDtoMap());
        }

        public DbSet<OrderEvent> OrdersEvents { get; set; }
        public DbSet<OrderDto> OrdersDtos { get; set; }
    }
}
