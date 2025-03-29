using ECommerceAPI.Modules.Orders.CustomModels;
using ECommerceAPI.Modules.Orders.Models;

namespace ECommerceAPI.Modules.Orders.Repositories
{
    public interface IOrderRepository
    {
        Task<(List<Order>, int TotalRecoreds, int CurrentPage, int TotalPages)> GetOrdersAsync(OrdersPagedRequest request);
        Task<Order> GetOrderByIdAsync(int OrderId);
        Task AddOrderAsync(Order order);
        Task UpdateOrderAsync(Order order);
        Task SaveChangesAsync();
    }
}
