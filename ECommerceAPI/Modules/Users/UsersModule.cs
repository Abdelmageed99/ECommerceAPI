using ECommerceAPI.Modules.Users.Models;
using ECommerceAPI.Modules.Users.Repositories;
using ECommerceAPI.Modules.Users.Services;
using ECommerceAPI.Shared.Database;
using ECommerceAPI.Shared.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerceAPI.Modules.Users
{
    public static class UsersModule
    {
        public static void AddUsersModule(this IServiceCollection services)
        {
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddAutoMapper(typeof(Program));

        

            
        }
    }
}
