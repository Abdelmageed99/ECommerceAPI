using AutoMapper;
using ECommerceAPI.Modules.Products.CustomModels;
using ECommerceAPI.Modules.Products.DTOs;
using ECommerceAPI.Modules.Products.Models;
using ECommerceAPI.Modules.Products.Repositories;
using Microsoft.EntityFrameworkCore;

namespace ECommerceAPI.Modules.Products.Services
{
    public class ProductService : IProductService
    {
        private readonly IProductRepository _repository;
        private readonly IMapper _mapper;

        public ProductService(IProductRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<ProductsPagedResult> GetPagedProductsAsync(ProductsPagedRequest request)
        {
            var result = await _repository.GetPagedProductsAsync(request);
            return result;
        }


        public async Task<Product> GetProductByIdAsync(int Id)
        {
            
            var item = await _repository.GetProductByIdAsync(Id);
            return item;
        }

        public async Task<Product> AddProductAsync(ProductDTO productDTO)
        {
            var product = _mapper.Map<Product>(productDTO);
            await _repository.AddProductAsync(product);
            return product;
        }

        public async Task<Product> UpdateProductAsync(int Id, ProductDTO productDTO)
        {
            var updatedproduct = await _repository.UpdateProductAsync(Id,productDTO);  
            return updatedproduct;
        }
        public async Task<Product> DeleteProductAsync(ProductDTO productDTO)
        {
            var product =  _mapper.Map<Product>(productDTO);
            var deletedProduct = await _repository.DeleteProductAsync(product);

            return deletedProduct;
        }

    }
}
