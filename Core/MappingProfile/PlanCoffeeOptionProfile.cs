using AutoMapper;
using Core.DTOs.PlanCoffeeOptionDTO;
using Core.Models;

namespace Core.MappingProfile
{
    public class PlanCoffeeOptionProfile : Profile
    {
        public PlanCoffeeOptionProfile()
        {
            CreateMap<PlanCoffeeOption, PlanCoffeeOptionResponseDto>();
            CreateMap<CreatePlanCoffeeOptionDto, PlanCoffeeOption>();
        }
    }

}

