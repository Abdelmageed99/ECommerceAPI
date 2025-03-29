using ECommerceAPI.Modules.Orders.Repositories;
using ECommerceAPI.Modules.Orders.Services;

namespace ECommerceAPI.Modules.Orders
{
    public static class OrderModule
    {
        public static void AddOrdersModule(this IServiceCollection services)
        {
            services.AddScoped<IOrderService, OrderService>();
            services.AddScoped<IOrderRepository, OrderRepository>();
        }
    }
}
