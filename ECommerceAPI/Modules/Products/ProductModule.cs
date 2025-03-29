using ECommerceAPI.Modules.Products.Repositories;
using ECommerceAPI.Modules.Products.Services;

namespace ECommerceAPI.Modules.Products
{
    public static class ProductModule
    {
        public static void AddProductsModule(this IServiceCollection services)
        {
            services.AddScoped<IProductService, ProductService>();
            services.AddScoped<IProductRepository, ProductRepository>();
        }
    }
}
