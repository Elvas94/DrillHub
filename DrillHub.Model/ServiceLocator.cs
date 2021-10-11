using Microsoft.Extensions.DependencyInjection;

namespace DrillHub.Model
{
    public static class ServiceLocator
    {
        public static void RegisterServices(IServiceCollection services)
        {
            services.AddTransient<Products.ProductService>();
            services.AddTransient<Orders.OrderService>();
            services.AddTransient<ExcelService>();
            services.AddTransient<Categories.CategoryService>();
            services.AddTransient<SubCategories.SubCategoryService>();
        }
    }
}
