using ECommerceAPI.Modules.Users.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Modules.Payments.DTOs
{
    public class PaymentResponse
    {
        public int OrderId { get; set; }
        public string UserId { get; set; }
        public string PaymentMethod { get; set; } // e.g. "Credit Card" , "PayPal", "Braintree"
        public string PaymentStatus { get; set; }// e.g. "Pending" , "Completed", "Failed"
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;

    }
}
