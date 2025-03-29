using Braintree;
using ECommerceAPI.Modules.Orders.Repositories;
using ECommerceAPI.Modules.Payments.CustomModels;
using ECommerceAPI.Modules.Payments.DTOs;
using ECommerceAPI.Modules.Payments.Models;
using ECommerceAPI.Modules.Payments.Repositories;
using Microsoft.Extensions.Options;


namespace ECommerceAPI.Modules.Payments.Services
{
    public class PaymentService : IPaymentService
    {
        private readonly IBraintreeGateway _braintreeGateway;
        private readonly IPaymentRepository _paymentRepository;

        public PaymentService(IBraintreeGateway braintreeGateway, IPaymentRepository paymentRepository)
        {
            _braintreeGateway = braintreeGateway;
            _paymentRepository = paymentRepository;
        }

        public async Task<string> ProcessPaymentAsync(decimal amount, string orderId, string userId)
        {
            var request = new TransactionRequest
            {
                Amount = amount,
                OrderId = orderId,
                PaymentMethodNonce = "fake-valid-nonce", // هنستبدلها بالنونس اللي بيجي من الـ Frontend
                Options = new TransactionOptionsRequest
                {
                    SubmitForSettlement = true
                }
            };

            var result = await _braintreeGateway.Transaction.SaleAsync(request);

            if (result.IsSuccess())
            {
                var transactionId = result.Target.Id;

                var payment = new Payment
                {
                    OrderId = int.Parse(orderId),
                    UserId = userId,
                    Amount = amount,
                    PaymentMethod = "Braintree",
                    PaymentStatus = "Completed",
                    TransactionId = transactionId
                };

                await _paymentRepository.AddPaymentAsync(payment);
                await _paymentRepository.SaveChangesAsync();

                return transactionId;
            }
            else
            {
                throw new Exception("Payment failed: " + result.Message);
            }
        }

        public async Task<bool> VerifyPaymentAsync(string transactionId)
        {
            var Transaction = await _paymentRepository.GetPaymentByTransactionIdAsync(transactionId);
            if (Transaction == null)
                throw new Exception("Transaction Not Found");
            return true;


            //var transaction = await _braintreeGateway.Transaction.FindAsync(transactionId);
            //return transaction.Status == TransactionStatus.SETTLED || transaction.Status == TransactionStatus.SUBMITTED_FOR_SETTLEMENT; 
        }

        public async Task<PaymentPagedResult> GetPaymentsAsync (PaymentPagedRequest request)
        {
            var (payments, totalRecordes, totalPages, currentPage) = await _paymentRepository.GetPaymentsAsync(request);


            var paymentRespones = new List<PaymentResponse>();

            foreach (var payment in payments)
            {
                var paymentRespone = new PaymentResponse();

                paymentRespone.PaymentStatus = payment.PaymentStatus;
                paymentRespone.PaymentMethod = payment.PaymentMethod;
                paymentRespone.OrderId = payment.OrderId;
                paymentRespone.Amount = payment.Amount;
                paymentRespone.UserId = payment.UserId;
                paymentRespone.TransactionId = payment.TransactionId;

                paymentRespones.Add(paymentRespone);
            }

            var paymentPagedResult = new PaymentPagedResult()
            {
                Entities = paymentRespones,
                TotalPages = totalPages,
                TotalRecords = totalRecordes,
                CurrentPage = currentPage
            };

            return paymentPagedResult;
        }
    }
}
