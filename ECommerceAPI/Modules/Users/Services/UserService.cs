using ECommerceAPI.Modules.Users.CustomModels;
using ECommerceAPI.Modules.Users.DTOs;
using ECommerceAPI.Modules.Users.Models;
using ECommerceAPI.Modules.Users.Repositories;
using ECommerceAPI.Shared.Security;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.Data;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace ECommerceAPI.Modules.Users.Services
{
    public class UserService : IUserService
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IOptions<JwtSettings> _options;
        private readonly IUserRepository _usersRepository;

        public UserService(UserManager<ApplicationUser> userManager, IOptions<JwtSettings> options, IUserRepository usersRepository)
        {
            _userManager = userManager;
            _options = options;
            _usersRepository = usersRepository;
        }

        public async Task<AuthModel> SingUp(RegisterModel model)
        {
            var authModel = new AuthModel();
            if (await _userManager.FindByEmailAsync(model.Email) is not null)
            {
                authModel.Message = "This Email already used";
                authModel.IsAuthenticated = false;
                return authModel;

            }
              

            if (await _userManager.FindByNameAsync(model.UserName) is not null)
            {
                
                authModel.Message = "This UserName already used";
                authModel.IsAuthenticated = false;
                return authModel;

            }
                

            var user = new ApplicationUser
            {
                Email = model.Email,
                UserName = model.UserName,
                FullName= model.FullName
                

            };

            var result = await _userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                authModel.Message = string.Join(",",result.Errors.Select(e => e.Description));
                authModel.IsAuthenticated = false;
                return authModel;
            }

            await _userManager.AddToRoleAsync(user, "Customer");
            authModel.Message = "User created";
            authModel.IsAuthenticated = true;
            authModel.UserName = user.UserName;

            var roles = await _userManager.GetRolesAsync(user);
            authModel.Roles = roles.ToList();

            return authModel;
        }


        public async Task<AuthModel> SingIn(LoginModel model)
        {
            var authModel = new AuthModel();
            var user = await _userManager.FindByEmailAsync(model.Email);

            if (user is null || !await _userManager.CheckPasswordAsync(user, model.Password))
                return new AuthModel { Message = "Email or Password is invalid" };

            var roles = await _userManager.GetRolesAsync(user);
            var Token = GenerateAccessToken(user, roles[0]);
            

            authModel.IsAuthenticated = true;
            authModel.ExpireOn = DateTime.UtcNow.AddHours(_options.Value.ExpireOn);

            authModel.Roles = roles?.ToList();
            authModel.Token = Token;

            var refreshToke = GenerateRefreshToken();

            if (user.RefreshTokens.Any(t => t.IsActice))
            {
                var activeRefreshToken = user.RefreshTokens.FirstOrDefault(t => t.IsActice);
                authModel.RefreshToken = activeRefreshToken?.Token;
                authModel.RefreshTokenExpireOn = activeRefreshToken.ExpireOn;
            }
            else
            {
                var refreshToken = GenerateRefreshToken();
                authModel.RefreshToken = refreshToken.Token;
                authModel.RefreshTokenExpireOn = refreshToken.ExpireOn;
                user.RefreshTokens.Add(refreshToken);
                await _userManager.UpdateAsync(user);
            }

            return authModel;


        }


        public async Task<AuthModel> StaySingingIn(string token)
        {
            var authModel = new AuthModel();

            var user = await _userManager.Users.SingleOrDefaultAsync(t => t.RefreshTokens.Any(t => t.Token == token));

            if (user == null)
            {
                authModel.Message = "Token Is Invalid";
                return authModel;
            }

            var refreshToken = user.RefreshTokens.SingleOrDefault(t => t.Token == token);

            if (!refreshToken.IsActice)
            {
                authModel.Message = "InActive Token";
                return authModel;
            }
            refreshToken.RevokedOn = DateTime.UtcNow;

            var roles = await _userManager.GetRolesAsync(user);
            var accessToken = GenerateAccessToken(user, "Customer");

            authModel.Token = accessToken;
            authModel.ExpireOn = DateTime.UtcNow.AddMinutes(_options.Value.ExpireOn);
            authModel.IsAuthenticated = true;
            authModel.Roles = roles.ToList();


            var refrehToken = GenerateRefreshToken();

            authModel.RefreshToken = refrehToken.Token;
            authModel.RefreshTokenExpireOn = refrehToken.ExpireOn;

            user.RefreshTokens.Add(refrehToken);
            await _userManager.UpdateAsync(user);


            return authModel;
        }


        private string GenerateAccessToken(ApplicationUser user, string role)
        {
            var authClaims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.Id),
                new Claim(ClaimTypes.Email, user.Email),

                new Claim(ClaimTypes.Role, role),

             


            };


            var Key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_options.Value.SecretKey));
            var signingCred = new SigningCredentials(Key, SecurityAlgorithms.HmacSha256);

            var jwttoken = new JwtSecurityToken
            (
                issuer: _options.Value.Issuer,
                audience: _options.Value.Audience,
                expires: DateTime.UtcNow.AddHours(1),
                claims: authClaims,
                signingCredentials: signingCred

            );

            var token = new JwtSecurityTokenHandler().WriteToken(jwttoken);

            return token.ToString();




        }


        private RefreshTokenModel GenerateRefreshToken()
        {
            var random = new byte[32];

            using var rng = RandomNumberGenerator.Create();

            rng.GetBytes(random);

            return new RefreshTokenModel
            {
                Token = Convert.ToBase64String(random),
                ExpireOn = DateTime.UtcNow.AddDays(10),
                CreatedOn = DateTime.UtcNow,
            };


        }

        public Task<PagedResultUsers> GetPagedUsersAsync(PagedRequestUsers request)
        {
            var result = _usersRepository.GetUsersPagedAsync(request);
            return result;
        }
    }
}
