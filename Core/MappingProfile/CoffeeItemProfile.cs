using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.Request;
using Core.Models;

namespace Core.MappingProfile
{
    public class CoffeeItemProfile : Profile
    {
        public CoffeeItemProfile()
        {
            CreateMap<CoffeeItem, CoffeeItemResponseDto>();
            CreateMap<CreateCoffeeItemDto, CoffeeItem>();
            CreateMap<CoffeeItemRequestDto, CoffeeItem>()
           .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());
        }
    }
}
