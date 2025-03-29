using ECommerceAPI.Modules.Products.CustomModels;
using ECommerceAPI.Modules.Products.DTOs;
using ECommerceAPI.Modules.Products.Models;

namespace ECommerceAPI.Modules.Products.Services
{
    public interface IProductService
    {
        Task<ProductsPagedResult> GetPagedProductsAsync(ProductsPagedRequest request);

        Task<Product> GetProductByIdAsync(int Id);

        Task<Product> AddProductAsync(ProductDTO productDTO);

        Task<Product> UpdateProductAsync(int Id, ProductDTO productDTO);

        Task<Product> DeleteProductAsync(ProductDTO productDTO);




    }
}
