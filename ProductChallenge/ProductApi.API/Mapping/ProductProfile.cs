using AutoMapper;
using ProductApi.Domain.Entities;
using ProductApi.Application.DTOs;

namespace ProductApi.API.Mapping
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            CreateMap<Product, ProductResponse>().ReverseMap();
            CreateMap<ProductRequest, Product>().ReverseMap();
        }
    }
}
