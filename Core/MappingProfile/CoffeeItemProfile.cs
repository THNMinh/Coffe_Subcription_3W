using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.Request;
using Core.DTOs.UserSubscriptionDTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
