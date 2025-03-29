using ECommerceAPI.Modules.Orders.CustomModels;
using ECommerceAPI.Modules.Orders.DTOs;
using ECommerceAPI.Modules.Orders.Models;
using ECommerceAPI.Modules.Orders.Repositories;
using ECommerceAPI.Modules.ShoppingCarts.Repositories;

namespace ECommerceAPI.Modules.Orders.Services
{
    public class OrderService : IOrderService
    {
        private readonly IOrderRepository _orderRepository;
        private readonly ICartRepository _cartRepository;

        public OrderService(IOrderRepository orderRepository, ICartRepository cartRepository)
        {
            _orderRepository = orderRepository;
            _cartRepository = cartRepository;
        }


        public async Task<OrdersPagedResult> GetOrdersAsync(OrdersPagedRequest request)
        {
            var (orders, totalRecords, currentPage, totalPages)  = await _orderRepository.GetOrdersAsync(request);
            if (orders == null)
            {
                new Exception("Not Orders Found");
            }

            List<OrderResponse> orderResponses = new List<OrderResponse>();

            foreach (var order in orders)
            {
                OrderResponse orderResponse = new OrderResponse
                {
                    OrderId = order.Id,
                    OrderDate = order.OrderDate,
                    Status = order.Status,
                    TotalPrice = order.TotalPrice,
                    Items = order.Items.Select(oir => new OrderItemResponse
                    {

                        ProductId = oir.ProductId,
                        Price = oir.Price,
                        Quantity = oir.Quantity,

                    }).ToList()

                };

                orderResponses.Add(orderResponse);
            }

            var result = new OrdersPagedResult
            {
                Entities = orderResponses,
                TotalRecords = totalRecords,
                TotalPages = totalPages,
                CurrentPage = currentPage,

            };
            

            return result;
        }

        public async Task<OrderResponse> GetOrderByIdAsync(int OrderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(OrderId);

            if(order == null)
            {
               throw new KeyNotFoundException("Order Not Found");
            }

            var orderResponse = new OrderResponse
            {
                OrderId = order.Id,
                OrderDate = order.OrderDate,
                Status = order.Status,
                TotalPrice = order.TotalPrice,
                Items = order.Items.Select(oir => new OrderItemResponse
                {

                    ProductId = oir.ProductId,
                    Price = oir.Price,
                    Quantity = oir.Quantity,

                }).ToList()

            };

            return orderResponse;   
        }


        public async Task<int> OrderCheckoutAsync(string UserId)
        {
            var cart = await _cartRepository.GetCartByUserIdAsync(UserId);
            if (cart.Items == null || !cart.Items.Any())
            {
                throw new Exception("cart is empty");
            }


            var order = new Order
            {
                UserId = UserId,
                Status = "Pending",
                OrderDate = DateTime.Now,
                TotalPrice = cart.Items.Sum(x => x.Price * x.Quantity),
                Items = cart.Items.Select(oi => new OrderItem
                {
                    ProductId = oi.ProductId,
                    Price = oi.Price,
                    Quantity = oi.Quantity,

                }).ToList()

            };

            await _orderRepository.AddOrderAsync(order);
            await _orderRepository.SaveChangesAsync();

            await _cartRepository.ClearCartItemsAsync(cart.Items);
            await _cartRepository.SaveChangesAsync();

            return order.Id;
                
        }

        public async Task<int> OrderCancelAsync(string UserId, int OrderId)
        {
            var order = await _orderRepository.GetOrderByIdAsync(OrderId);

            if (order.Status != "Pending")
            {
                throw new Exception("Order Can't Cancel");
            }

            order.Status = "Cancelled";
            await _orderRepository.UpdateOrderAsync(order);
            await _orderRepository.SaveChangesAsync();

            return order.Id;

        }

        public async Task<bool> UpdateOrderStatusAsync(int OrderId, string Status)
        {
            var order = await  _orderRepository.GetOrderByIdAsync(OrderId);
            if (order == null)
            {
                throw new KeyNotFoundException ("Order not found");
            }

            var validStatuses = new[] { "Pending", "Completed", "Shipped", "Cancelled" };
            if (!validStatuses.Contains(Status))
            {
                throw new ArgumentException("Invalid status value");
            }

            order.Status = Status;

            await _orderRepository.UpdateOrderAsync(order);
            await _orderRepository.SaveChangesAsync();

            return true;


        }
    }
    }

