using Microsoft.EntityFrameworkCore;

namespace DrillHub.DataAccess
{
    public class DrillHubContext : DbContext
    {
        public DbSet<DrillHub.Model.Products.Product> Products { get; set; }
        public DbSet<DrillHub.Model.Orders.Order> Orders { get; set; }
        public DbSet<DrillHub.Model.Products.ProductOrder> ProductOrders { get; set; }
        public DbSet<DrillHub.Model.Categories.Category> Categories { get; set; }
        public DbSet<DrillHub.Model.Subcategories.Subcategory> Subcategories { get; set; }

        public DrillHubContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
