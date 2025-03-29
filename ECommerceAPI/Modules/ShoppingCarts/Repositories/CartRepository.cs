using ECommerceAPI.Modules.ShoppingCarts.Models;
using ECommerceAPI.Shared.Database;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Validations;

namespace ECommerceAPI.Modules.ShoppingCarts.Repositories
{
    public class CartRepository : ICartRepository
    {
        private readonly ApplicationDbContext _context;

        public CartRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<Cart> GetCartByUserIdAsync(string userId)
        {
            return await _context.Carts
                .Include(c => c.Items)
                .ThenInclude(i => i.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public async Task AddCartAsync(Cart cart)
        {
            await _context.Carts.AddAsync(cart);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateCartAsync(Cart cart)
        {
            _context.Carts.Update(cart);
            //await _context.SaveChangesAsync();
        }

        public async Task<CartItem> GetCartItemAsync(int cartId, int productId)
        {
            return await _context.CartItems
                .FirstOrDefaultAsync(i => i.CartId == cartId && i.ProductId == productId);
        }

        public async Task AddCartItemAsync(CartItem cartItem)
        {
            await _context.CartItems.AddAsync(cartItem);
            //await _context.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(int CartId, CartItem cartItem)
        {
            var Item = await _context.CartItems.FirstOrDefaultAsync(c => c.ProductId == cartItem.ProductId && c.CartId == CartId);

            if (Item != null)
            {

                Item.Quantity = cartItem.Quantity;
                //await _context.SaveChangesAsync();
            }


        }

        public async Task RemoveCartItemAsync(CartItem cartItem)
        {
            _context.CartItems.Remove(cartItem);
            //await  _context.SaveChangesAsync();
        }

        public async Task ClearCartItemsAsync(List<CartItem> cartItems)
        {
            _context.CartItems.RemoveRange(cartItems);
            //await _context.SaveChangesAsync();
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }

}
