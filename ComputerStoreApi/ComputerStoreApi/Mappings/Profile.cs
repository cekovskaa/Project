using ComputerStoreApi.DTOs;
using ComputerStoreApi.Models;

namespace ComputerStoreApi.Mappings
{
    public class Profile : AutoMapper.Profile
    {
        public Profile()
        {
            CreateMap<Category, CategoryDTO>().ReverseMap();
            CreateMap<Product, ProductDTO>()
                .ForMember(dest => dest.CategoryId, opt => opt.MapFrom(src => src.CategoryId))
                .ReverseMap();
        }
    }
}




