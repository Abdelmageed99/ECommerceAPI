using ECommerceAPI.Modules.Products.Repositories;
using ECommerceAPI.Modules.ShoppingCarts.DTOs;
using ECommerceAPI.Modules.ShoppingCarts.Models;
using ECommerceAPI.Modules.ShoppingCarts.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.ShoppingCarts.Services
{
    public class CartService : ICartService
    {
        private readonly ICartRepository _cartRepository;
        private readonly IProductRepository _productRepository; // Ensure product exists

        public CartService(ICartRepository cartRepository, IProductRepository productRepository)
        {
            _cartRepository = cartRepository;
            _productRepository = productRepository;
        }

        public async Task<CartResponse> GetCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);

            if (cart == null)
                return null;


            return new CartResponse
            {
                CartId = cart.Id,
                UserId = cart.UserId,
                TotalPrice = cart.Items.Sum(item => item.Price * item.Quantity),    
                Items = cart.Items.Select(item => new CartItemResponse
                {
                    CartItemId = item.Id,
                    ProductId = item.ProductId,
                    ProductName = item.Product.Name,
                    ProductDescription = item.Product.Description,
                    ProductCategory = item.Product.Category,
                    Quantity = item.Quantity,
                    Price = item.Product.Price


                }).ToList()
            };
        }

        public async Task AddToCartAsync(string userId, CartRequest request)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null)
            {
                cart = new Cart { UserId = userId, CreatedAt = DateTime.UtcNow };
                await _cartRepository.AddCartAsync(cart);
                await _cartRepository.SaveChangesAsync();
            }

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, request.ProductId);
            if (cartItem != null)
            {
                cartItem.Quantity += request.Quantity;
                await _cartRepository.UpdateCartItemAsync(cart.Id, cartItem);
            }
            else
            {
                var product = await _productRepository.GetProductByIdAsync(request.ProductId);

                cartItem = new CartItem
                {
                    CartId = cart.Id,
                    ProductId = request.ProductId,
                    Quantity = request.Quantity,
                    Price = product.Price
                };

                await _cartRepository.AddCartItemAsync(cartItem);
            }

            cart.UpdatedAt = DateTime.UtcNow;
            await _cartRepository.UpdateCartAsync(cart);

            await _cartRepository.SaveChangesAsync();
        }

        public async Task UpdateCartItemAsync(string userId, CartRequest cartItemDTO)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, cartItemDTO.ProductId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            cartItem.Quantity = cartItemDTO.Quantity;
            await _cartRepository.UpdateCartItemAsync(cart.Id,cartItem);

            cart.UpdatedAt = DateTime.UtcNow;
            //await _cartRepository.UpdateCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveCartItemAsync(string userId, int productId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, productId);
            if (cartItem == null) throw new Exception("Item not found in cart.");

            await _cartRepository.RemoveCartItemAsync(cartItem);

            cart.UpdatedAt = DateTime.UtcNow;
            //await _cartRepository.UpdateCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task ClearCartAsync(string userId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found.");

            await _cartRepository.ClearCartItemsAsync(cart.Items);
            cart.UpdatedAt = DateTime.UtcNow;
            //await _cartRepository.UpdateCartAsync(cart);
            await _cartRepository.SaveChangesAsync();
        }

        public async Task RemoveFromCartAsync(string userId, CartRequest cartRequest)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(userId);
            if (cart == null) throw new Exception("Cart not found");

            var cartItem = await _cartRepository.GetCartItemAsync(cart.Id, cartRequest.ProductId);
            if (cartItem == null) throw new Exception("Item not found in cart");

            cartItem.Quantity -= cartRequest.Quantity;
            await _cartRepository.UpdateCartItemAsync(cart.Id, cartItem);
            cart.UpdatedAt = DateTime.UtcNow;

            await _cartRepository.SaveChangesAsync();
        }
    }

}
