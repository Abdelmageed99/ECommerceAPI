namespace ECommerceAPI.Modules.Users.CustomModels
{
    public class AuthModel
    {
        public string Message { get; set; }

        public bool IsAuthenticated { get; set; }
       
        
        public string UserName { get; set; }
        public List<string> Roles { get; set; }
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }


        public string RefreshToken { get; set; }
        public DateTime RefreshTokenExpireOn { get; set; }
    }
}
