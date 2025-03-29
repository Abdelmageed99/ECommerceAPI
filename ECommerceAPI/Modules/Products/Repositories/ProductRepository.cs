using ECommerceAPI.Modules.Products.CustomModels;
using ECommerceAPI.Modules.Products.DTOs;
using ECommerceAPI.Modules.Products.Models;
using ECommerceAPI.Shared.Database;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Products.Repositories
{
    public class ProductRepository : IProductRepository
    {
        private readonly ApplicationDbContext _context;

        public ProductRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<ProductsPagedResult> GetPagedProductsAsync(ProductsPagedRequest request)
        {
            IQueryable<Product> query = _context.Products.AsQueryable();

            if (!string.IsNullOrWhiteSpace(request.SearchTerm))
            {
                query = query.Where(p => p.Name.Contains(request.SearchTerm)
                            || p.Category.Contains(request.SearchTerm)
                            || p.Description.Contains(request.SearchTerm));

            }

            if (!string.IsNullOrWhiteSpace(request.OrderBy))
            {
                query = request.OrderBy.ToLower() switch
                {
                    "name" => query.OrderBy(p => p.Name),
                    "name_desc" => query.OrderByDescending(p => p.Name),

                    "price" => query.OrderBy(p => p.Price),
                    "price_desc" => query.OrderByDescending(p => p.Price),

                    _ => query.OrderBy(p => p.Id)

                };
            }
            else
            {
                query = query.OrderBy(p => p.Id);
            }

            var items = query.Skip((request.PageIndex - 1) * request.PageSize).Take(request.PageSize).ToList();

            var totalRecords = query.Count();

            var pageNumber = request.PageIndex;

            var totalPages = (int)(totalRecords / request.PageSize);

            return new ProductsPagedResult
            {
                Entities = items,
                TotalPages = totalPages,
                TotalRecords = totalRecords,
                CurrentPage = pageNumber,
            };
        }


        public async Task<Product> GetProductByIdAsync(int Id)
        {
            var item = await  _context.Products.FindAsync(Id);
            return item;
        }

        public async Task<Product> AddProductAsync(Product product)
        {
            await _context.Products.AddAsync(product);
            await _context.SaveChangesAsync();
            return product;
        }

        public async Task<Product> UpdateProductAsync(int Id, ProductDTO productDTO)
        {
            var product = await _context.Products.FindAsync(Id);

            product.Name = productDTO.Name?? product.Name;
            product.Price = productDTO.Price ?? product.Price;
            product.Description = productDTO.Name ?? product.Description;
            product.Category = productDTO.Category ?? product.Category;

            _context.Update(product);
            _context.SaveChanges();

            return product;

        }
        public  async Task<Product> DeleteProductAsync(Product product)
        {
             _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return product;
        }




    }
    }
