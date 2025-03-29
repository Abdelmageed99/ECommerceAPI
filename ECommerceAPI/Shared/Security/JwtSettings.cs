namespace ECommerceAPI.Shared.Security
{
    public class JwtSettings
    {
            public string Issuer { get; set; }
            public string Audience { get; set; }
            public int ExpireOn { get; set; }
            public string SecretKey { get; set; }
        

    }
}
