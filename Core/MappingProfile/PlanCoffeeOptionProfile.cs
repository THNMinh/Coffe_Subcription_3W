using AutoMapper;
using Core.DTOs.CoffeeItemDTO;
using Core.DTOs.PlanCoffeeOptionDTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

