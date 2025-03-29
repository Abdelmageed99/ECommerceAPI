using ECommerceAPI.Modules.ShoppingCarts.Repositories;
using ECommerceAPI.Modules.ShoppingCarts.Services;

namespace ECommerceAPI.Modules.ShoppingCarts
{
    public static class ShoppingCartModule
    {

        public static void AddCartsModule(this IServiceCollection services)
        {
            services.AddScoped<ICartService, CartService>();
            services.AddScoped<ICartRepository, CartRepository>();
        }
    }
}
