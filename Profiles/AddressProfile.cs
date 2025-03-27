using AutoMapper;
using CodeFirst.Dtos;
using CodeFirst.Models;

namespace CodeFirst.Profiles
{
    public class AddressProfile : Profile
    {
        public AddressProfile() 
        {
            // Mapping from Address entity to AddressDto
            CreateMap<AddressDto, Address>().ReverseMap();


            // Mapping from AddressToCreateDto to Address entity
            CreateMap<AddressToCreateDto, Address>();


        }
    }
}
