using ECommerceAPI.Modules.Products.CustomModels;
using ECommerceAPI.Modules.Products.DTOs;
using ECommerceAPI.Modules.Products.Models;

namespace ECommerceAPI.Modules.Products.Repositories
{
    public interface IProductRepository
    {
        Task<ProductsPagedResult> GetPagedProductsAsync(ProductsPagedRequest request);

        Task<Product> GetProductByIdAsync(int Id);

        Task<Product> AddProductAsync(Product product);

        Task<Product> UpdateProductAsync(int Id, ProductDTO productDTO);

        Task<Product> DeleteProductAsync(Product product);
    }
}

