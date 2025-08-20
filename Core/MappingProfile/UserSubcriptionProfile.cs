using AutoMapper;
using Core.DTOs.UserSubscriptionDTO;
using Core.Models;

namespace Core.MappingProfile
{
    public class UserSubcriptionProfile : Profile
    {
        public UserSubcriptionProfile()
        {
            CreateMap<UserSubscription, UserSubscriptionResponseDto>();
            CreateMap<CreateUserSubscriptionDto, UserSubscription>();
        }
    }
}
