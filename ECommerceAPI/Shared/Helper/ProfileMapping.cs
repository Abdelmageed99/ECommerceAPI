using AutoMapper;
using ECommerceAPI.Modules.Products.DTOs;
using ECommerceAPI.Modules.Products.Models;

namespace ECommerceAPI.Shared.Helper
{
    public class ProfileMapping : Profile
    {
        public ProfileMapping()
        {
            CreateMap<ProductDTO, Product>();
        }
    }
}
