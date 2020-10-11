using Microsoft.EntityFrameworkCore;

namespace DrillHub.DataAccess
{
    public class DrillHubContext : DbContext
    {
        public DbSet<Model.Products.Product> Products { get; set; }
        public DbSet<Model.Orders.Order> Orders { get; set; }
        public DbSet<Model.Products.ProductOrder> ProductOrders { get; set; }
        public DbSet<Model.Categories.Category> Categories { get; set; }
        public DbSet<Model.SubCategories.SubCategory> SubCategories { get; set; }

        public DrillHubContext(DbContextOptions options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }
    }
}
