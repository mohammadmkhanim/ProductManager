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
        }
    }
}