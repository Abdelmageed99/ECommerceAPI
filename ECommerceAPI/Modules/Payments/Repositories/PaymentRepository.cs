using Braintree;
using ECommerceAPI.Modules.Payments.CustomModels;
using ECommerceAPI.Modules.Payments.DTOs;
using ECommerceAPI.Modules.Payments.Models;
using ECommerceAPI.Shared.Database;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Payments.Repositories
{
    public class PaymentRepository : IPaymentRepository
    {
        private readonly ApplicationDbContext _context;

        public PaymentRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<(List<Payment>, int totalRecords, int totalPages, int currentPage)> GetPaymentsAsync(PaymentPagedRequest request)
        {
            IQueryable<Payment> query = _context.Payments.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(x =>
                       x.UserId.Contains(request.SearchTerm)
                    || x.PaymentMethod.Contains(request.SearchTerm)
                    || x.PaymentStatus.Contains(request.SearchTerm));

            }

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                query = request.OrderBy.ToLower() switch
                {
                    "userid" => query.OrderBy(p => p.UserId),
                    "amount" => query.OrderBy(p => p.Amount),
                    "amount_desc" => query.OrderByDescending(p => p.Amount)
                };

            }
            else
            {
                query = query.OrderBy(p => p.UserId);
            }

            var payments = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();
            var currentPage = request.PageIndex;
            var totalRecordes = query.Count();
            var totalPages = (int)(totalRecordes / request.PageSize);

            
           
            
            return (payments, totalPages, totalPages, currentPage);
        }
        public async Task<Payment> GetPaymentByTransactionIdAsync(string id)
        {
            return await _context.Payments.FirstOrDefaultAsync( p => p.TransactionId == id);
        }

        public async Task AddPaymentAsync(Payment payment)
        {
            await _context.Payments.AddAsync(payment);
        }
       
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }


    }
}
