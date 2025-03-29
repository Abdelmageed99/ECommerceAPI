using ECommerceAPI.Modules.ShoppingCarts.DTOs;
using ECommerceAPI.Modules.ShoppingCarts.Models;

namespace ECommerceAPI.Modules.ShoppingCarts.Repositories
{
    public interface ICartRepository
    {
        Task<Cart> GetCartByUserIdAsync(string userId);
        Task AddCartAsync(Cart cart);
        Task UpdateCartAsync(Cart cart);
        Task<CartItem> GetCartItemAsync(int cartId, int productId);
        Task AddCartItemAsync(CartItem cartItem);
        Task UpdateCartItemAsync(int CartId, CartItem cartItem);
        Task RemoveCartItemAsync(CartItem cartItem);
        Task ClearCartItemsAsync(List<CartItem> cartItems);
       
        Task SaveChangesAsync();
    }

}
