using Application.Dtos;
using Application.Products;
using Application.Users;
using AutoMapper;
using Core.Entities;

namespace Application.Services
{
    public class MappingService : Profile
    {
        public MappingService()
        {
            CreateMap<User, Register.Command>().ReverseMap();
            CreateMap<Product, Create.Command>().ReverseMap();
            CreateMap<Product, Edit.Command>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
        }
    }
}