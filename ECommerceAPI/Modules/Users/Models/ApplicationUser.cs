using ECommerceAPI.Modules.Orders.Models;
//using ECommerceAPI.Modules.ShoppingCarts.Models;
using ECommerceAPI.Modules.Users.CustomModels;
using ECommerceAPI.Modules.Users.Validations;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Users.Models
{
    
    public class ApplicationUser :IdentityUser
    {
        public string FullName { get; set; }

        public List<RefreshTokenModel> RefreshTokens { get; set; }

        public List<Order> Orders { get; set; }
    }
}
