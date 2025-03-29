using ECommerceAPI.Modules.Products.CustomModels;
using ECommerceAPI.Modules.Products.DTOs;
using ECommerceAPI.Modules.Products.Models;
using ECommerceAPI.Modules.Products.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace ECommerceAPI.Modules.Products.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {

        private readonly IProductService _productService;

        public ProductsController(IProductService productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<ActionResult<ProductsPagedResult>> GetPagedProductsAsync([FromQuery] ProductsPagedRequest pagedRequest)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.GetPagedProductsAsync(pagedRequest);
            if (result == null)
            {
                return NotFound();
            }

            return Ok(result);

        }

        [HttpGet("GetById/{Id}")]
        public async Task<ActionResult> GetProductByIdAsyn(int Id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.GetProductByIdAsync(Id);
            return Ok(result);

        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> AddProductAsync(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.AddProductAsync(productDTO);
            return Ok(result);

        }
        [HttpPut("{Id}")]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> UpdateProductAsync(int Id, ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.UpdateProductAsync(Id, productDTO);
            return Ok(result);

        }

        [HttpDelete]
        [Authorize(Roles = "Admin")]
        public async Task<ActionResult> DeleteProductAsync(ProductDTO productDTO)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var result = await _productService.DeleteProductAsync(productDTO);
            return Ok(result);

        }




    }
}
