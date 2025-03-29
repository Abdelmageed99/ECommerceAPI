using System.ComponentModel.DataAnnotations;

namespace ECommerceAPI.Modules.Users.DTOs
{
    public class RegisterModel
    {
        [Required(ErrorMessage = "First name required")]
        public string FName { get; set; }

        [Required(ErrorMessage = "Last name required")]
        public string LName { get; set; }

        [Required(ErrorMessage = "UserName required")]
        public string UserName { get; set; }
        public string FullName => FName + " " +LName;

        [EmailAddress(ErrorMessage = "Email formate is invalid")]
        [Required(ErrorMessage = "Email  required")]
        public string Email { get; set; }

        [Required(ErrorMessage = "Email required")]
        public string Password { get; set; }
    }
}
