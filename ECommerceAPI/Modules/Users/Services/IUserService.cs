using ECommerceAPI.Modules.Users.CustomModels;
using ECommerceAPI.Modules.Users.DTOs;

namespace ECommerceAPI.Modules.Users.Services
{
    public interface IUserService
    {
        public Task<AuthModel> SingUp(RegisterModel model);
        public Task<AuthModel> SingIn(LoginModel model);
        public Task<AuthModel> StaySingingIn(string refreshToken);

        Task<PagedResultUsers> GetPagedUsersAsync(PagedRequestUsers request);





    }
}
