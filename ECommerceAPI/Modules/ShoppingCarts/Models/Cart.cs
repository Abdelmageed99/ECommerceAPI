using ECommerceAPI.Modules.Users.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace ECommerceAPI.Modules.ShoppingCarts.Models
{
    public class Cart
    {
        public int Id { get; set; }
        public DateTime CreatedAt { get; set; } = DateTime.Now;
        public DateTime UpdatedAt { get; set; } = DateTime.Now;


        [ForeignKey(nameof(ApplicationUser))]
        public string UserId { get; set; }
        public ApplicationUser ApplicationUser { get; set; }

        public List<CartItem> Items { get; set; }
    }
}
