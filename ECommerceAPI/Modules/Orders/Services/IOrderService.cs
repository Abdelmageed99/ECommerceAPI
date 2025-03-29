using ECommerceAPI.Modules.Orders.CustomModels;
using ECommerceAPI.Modules.Orders.DTOs;

namespace ECommerceAPI.Modules.Orders.Services
{
    public interface IOrderService
    {
        Task<OrdersPagedResult> GetOrdersAsync(OrdersPagedRequest request);
        Task<OrderResponse> GetOrderByIdAsync(int OrderId);
        Task<int> OrderCheckoutAsync(string UserId);
        Task<int> OrderCancelAsync(string UserId, int OrderId);

        Task<bool> UpdateOrderStatusAsync(int OrderId, string Status);
    }
}
