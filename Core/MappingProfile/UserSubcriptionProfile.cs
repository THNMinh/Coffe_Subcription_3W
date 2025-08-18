using AutoMapper;
using Core.DTOs.UserSubscriptionDTO;
using Core.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
