using ECommerceAPI.Modules.Orders.CustomModels;
using ECommerceAPI.Modules.Orders.DTOs;
using ECommerceAPI.Modules.Orders.Models;
using ECommerceAPI.Modules.Orders.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace ECommerceAPI.Modules.Orders.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<ActionResult<OrdersPagedResult>> GetOrders([FromQuery] OrdersPagedRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var orders = await _orderService.GetOrdersAsync(request);

            return Ok(orders);
        }

        [HttpGet("{OrderId}")]
        public async Task<ActionResult<OrderResponse>> GetOrderById(int OrderId)
        {
            var Order = await _orderService.GetOrderByIdAsync(OrderId);

            return Ok(Order);
        }


        [HttpGet("Checkout")]
        public async Task<ActionResult> Checkout()
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                return Unauthorized();
            }

            var OrderId = await _orderService.OrderCheckoutAsync(UserId);

            if (OrderId == null)
            {
                return BadRequest("Order not Created");
            }

            return Ok(new { Massege = "Order Placed Successfully", OrderId = OrderId });
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{orderId}")]
        public async Task<IActionResult> UpdateOrderStatusAsync(int orderId, UpdateOrderStatusRequest request)
        {
           
                var result = await _orderService.UpdateOrderStatusAsync(orderId, request.Status);

                return Ok(new { massege = " Order Updated successfully" });

        }

        [HttpDelete("Cancel/{OrderId}")]
        public async Task<ActionResult> CancelOrder(int OrderId)
        {
            var UserId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (UserId == null)
            {
                return Unauthorized();
            }

            var Id = await _orderService.OrderCancelAsync(UserId, OrderId);

            return Ok(new { Massege = "Order Canceled Successfully", OrderId = Id });
        }




    }
}
