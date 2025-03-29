using ECommerceAPI.Modules.ShoppingCarts.DTOs;
using ECommerceAPI.Modules.ShoppingCarts.Models;

namespace ECommerceAPI.Modules.ShoppingCarts.Services
{
    public interface ICartService
    {
        Task<CartResponse> GetCartAsync(string userId);
        Task AddToCartAsync(string userId, CartRequest cartRequest);
        Task UpdateCartItemAsync(string userId, CartRequest cartRequest);
        Task RemoveFromCartAsync(string userId, CartRequest cartRequest);
        Task RemoveCartItemAsync(string userId, int productId);
        Task ClearCartAsync(string userId);
    }

}
