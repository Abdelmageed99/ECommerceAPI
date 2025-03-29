using ECommerceAPI.Modules.Payments.CustomModels;
using ECommerceAPI.Modules.Payments.DTOs;
using ECommerceAPI.Modules.Payments.Models;

namespace ECommerceAPI.Modules.Payments.Repositories
{
    public interface IPaymentRepository
    {
        Task<(List<Payment>, int totalRecords, int totalPages, int currentPage)> GetPaymentsAsync(PaymentPagedRequest request);
        Task<Payment> GetPaymentByTransactionIdAsync(string id);
        Task AddPaymentAsync(Payment payment);
        Task SaveChangesAsync();
    }
}
