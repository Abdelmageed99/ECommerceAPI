using ECommerceAPI.Modules.Orders.Models;
using ECommerceAPI.Modules.Payments.Validations;
using ECommerceAPI.Modules.Users.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Modules.Payments.Models
{
   
    public class Payment
    {
        public int Id { get; set; }

        [ForeignKey(nameof(Order))]
        public int OrderId { get; set; }
        public Order Order { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public ApplicationUser User { get; set; }

        

        public string PaymentMethod { get; set; } // e.g. "Credit Card" , "PayPal", "Braintree"
        public string PaymentStatus { get; set; }// e.g. "Pending" , "Completed", "Failed"
        public decimal Amount { get; set; }
        public string TransactionId { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;


        

       

    }
}
