using ECommerceAPI.Modules.Orders.Validations;
using ECommerceAPI.Modules.Users.Models;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Modules.Orders.Models
{
   
    public class Order
    {
        public int Id { get; set; }

        [ForeignKey(nameof(User))]
        public string UserId { get; set; }
        public DateTime OrderDate { get; set; }
        public string Status { get; set; } = "Pending";  // Pending - Completed - Shipped - Cancelled 
        public decimal TotalPrice { get; set; }

        
        public ApplicationUser User { get; set; }
        public List<OrderItem> Items { get; set;} = new List<OrderItem>();
        

    }
}
