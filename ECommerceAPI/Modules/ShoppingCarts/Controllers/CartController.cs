using ECommerceAPI.Modules.Products.Services;
using ECommerceAPI.Modules.ShoppingCarts.DTOs;
using ECommerceAPI.Modules.ShoppingCarts.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Modules.ShoppingCarts.Controllers
{

    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {

        private readonly ICartService _cartService;
        private readonly IProductService _productService;

        public CartController(ICartService cartService, IProductService productService)
        {
            _cartService = cartService;
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult> GetCart()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                return Unauthorized();
            }

            var cart = await _cartService.GetCartAsync(UserId);

            if (cart == null)
            {
                return Ok(new { message = "No cartitem Found" });
            }

            return Ok(cart);
        }

        [HttpPost("Add")]
        public async Task<ActionResult> AddToCart(CartRequest request)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                return Unauthorized();
            }

            if (request.Quantity <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than 0" });
            }

            var product = await _productService.GetProductByIdAsync(request.ProductId);
            if (product == null)
            {
                return BadRequest(new { message = "Product Not Found" });
            }

            await _cartService.AddToCartAsync(UserId, request);


            return Ok(new { message = "Item Added" });
        }

        [HttpPost("Remove")]
        public async Task<ActionResult> RemoveFromCart(CartRequest request)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                return Unauthorized();
            }

            if (request.Quantity <= 0)
            {
                return BadRequest(new { message = "Quantity must be greater than 0" });
            }

        

            await _cartService.RemoveFromCartAsync(UserId, request);


            return Ok(new { message = $"item {request.ProductId} updated" });
        }

        [HttpPut]
        public async Task<ActionResult> UpdateCart(CartRequest request)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                return Unauthorized();
            }

            await _cartService.UpdateCartItemAsync(UserId, request);



            return Ok(new { message = $"item {request.ProductId} updated" });
        }

        [HttpDelete("{ProductId}")]
        public async Task<ActionResult> DeleteCartItem(int ProductId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                return Unauthorized();
            }

            await _cartService.RemoveCartItemAsync(UserId, ProductId);



            return Ok(new { message = "Item is deleted" });
        }

        [HttpDelete("Clear")]
        public async Task<ActionResult> ClearCart()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (UserId == null)
            {
                return Unauthorized();
            }

            await _cartService.ClearCartAsync(UserId);

            return Ok(new { message = "Cart cleard" });

        }


    }
}
