//using ECommerceAPI.Modules.Products.CustomModels;
//using ECommerceAPI.Modules.Products.Models;
using ECommerceAPI.Modules.Users.CustomModels;
using ECommerceAPI.Modules.Users.DTOs;
using ECommerceAPI.Modules.Users.Models;
using ECommerceAPI.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Users.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PagedResultUsers> GetUsersPagedAsync(PagedRequestUsers request)
        {
            IQueryable<UserDTO> query = _context.Users
                .Select(u => new UserDTO 
                                { FullName = u.FullName, UserName = u.UserName, Email = u.Email, PhoneNumber = u.PhoneNumber }).AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(u => u.UserName.Contains(request.SearchTerm)
                            || u.FullName.Contains(request.SearchTerm)
                            || u.Email.Contains(request.SearchTerm));

            }

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                query = request.OrderBy.ToLower() switch
                {
                    "username" => query.OrderBy(u => u.UserName),
                    "username_desc" => query.OrderByDescending(u => u.UserName),

                    "fullname" => query.OrderBy(u => u.FullName),
                    "fullname_desc" => query.OrderByDescending(u => u.FullName),

                    _ => query.OrderBy(u => u.FullName)

                };
            }
            else
            {
                query = query.OrderBy(p => p.FullName);
            }

            List<UserDTO> Users = await query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToListAsync();

            var totalRecords = query.Count();

            var pageNumber = request.PageIndex;

            var totalPages = (int)(totalRecords / request.PageSize);

            return new PagedResultUsers
            {
                Entities = Users,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
            };


        }
    }
}
