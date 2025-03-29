using ECommerceAPI.Modules.Orders.CustomModels;
using ECommerceAPI.Modules.Orders.Models;
using ECommerceAPI.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Orders.Repositories
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }


        public async Task<(List<Order>, int TotalRecoreds, int CurrentPage, int TotalPages)> GetOrdersAsync(OrdersPagedRequest request)
        {
            IQueryable<Order> query = _context.Orders.AsQueryable();

            if(!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(o => o.Status.Contains(request.SearchTerm));
            }

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                query = request.OrderBy.ToLower() switch
                {
                    "totalprice" => query.OrderBy(o => o.TotalPrice),
                    "totalprice_desc" => query.OrderByDescending(o => o.TotalPrice),

                    "orderdate" => query.OrderBy(o => o.OrderDate),
                    "orderdate_desc" => query.OrderByDescending(o => o.OrderDate),
                };
            }
            else
            {
                query = query.OrderBy(o => o.TotalPrice);
            }

            var totalRecoreds = query.Count();
            var currentPage = request.PageIndex;
            var totalPages = (int) (totalRecoreds / request.PageSize);

           return (await query.ToListAsync(), totalRecoreds, currentPage, totalPages);
                
        }

        public async Task<Order> GetOrderByIdAsync(int OrderId)
        {
            return await _context.Orders
                   .Include(oi => oi.Items)
                   .FirstOrDefaultAsync(o => o.Id == OrderId);
        }
        public async Task AddOrderAsync(Order order)
        {
            await _context.Orders.AddAsync(order);

        }

      
        public async Task UpdateOrderAsync(Order order)
        {
            
              _context.Orders.Update(order);

        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

    }
}
