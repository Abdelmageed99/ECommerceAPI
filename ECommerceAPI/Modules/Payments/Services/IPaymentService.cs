using ECommerceAPI.Modules.Payments.CustomModels;
using ECommerceAPI.Modules.Payments.DTOs;
using ECommerceAPI.Modules.Payments.Models;

namespace ECommerceAPI.Modules.Payments.Services
{
    public interface IPaymentService
    {
        Task<string> ProcessPaymentAsync(decimal amount, string orderId, string userId);
        Task<bool> VerifyPaymentAsync(string transactionId);

        Task<PaymentPagedResult> GetPaymentsAsync(PaymentPagedRequest request);


    }
}
