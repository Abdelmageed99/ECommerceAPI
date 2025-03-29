using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Users.CustomModels
{
    [Owned]
    public class RefreshTokenModel
    {
        public string Token { get; set; }

        public DateTime ExpireOn { get; set; }

        public bool IsExpired => DateTime.UtcNow >= ExpireOn;

        public DateTime CreatedOn { get; set; }

        public DateTime? RevokedOn { get; set; }

        public bool IsActice => RevokedOn is null && !IsExpired;
    }
}
