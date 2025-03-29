using ECommerceAPI.Modules.Users.CustomModels;

namespace ECommerceAPI.Modules.Users.Repositories
{
    public interface IUserRepository
    {
        Task<PagedResultUsers> GetUsersPagedAsync(PagedRequestUsers request);
    }
}
