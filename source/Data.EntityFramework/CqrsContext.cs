using System.Data.Entity;
using Data.EntityFramework.Mappings;
using Domain.Orders.EventSourcing;

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
        }

        public DbSet<OrderEvent> OrdersEvents { get; set; }
    }
}
