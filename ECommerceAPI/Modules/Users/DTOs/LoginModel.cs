using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Modules.Users.DTOs
{
    public class LoginModel
    {
        [EmailAddress(ErrorMessage = "Invalid Email Formate")]
        [Required(ErrorMessage = "Password Is Required")]
        public string Email { get; set; }


        [Required(ErrorMessage = "Password Is Required")]
     
        public string Password { get; set; }
        
    }
}
