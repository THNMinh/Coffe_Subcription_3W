using AutoMapper;
using Core.DTOs.SubscriptionPlanDTO;
using Core.Models;

namespace Core.MappingProfile
{
    public class SubscriptionPlanProfile : Profile
    {
        public SubscriptionPlanProfile()
        {
            CreateMap<SubscriptionPlan, SubscriptionPlanReponseDto>();
            CreateMap<CreateSubscriptionPlanDto, SubscriptionPlan>();
        }
    }
}
