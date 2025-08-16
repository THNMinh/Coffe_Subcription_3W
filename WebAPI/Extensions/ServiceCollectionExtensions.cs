using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Core.Interfaces.Repositories;
using Core.Interfaces.Repository;
using Core.Interfaces.Services;
using Service.Services;

using Microsoft.Extensions.DependencyInjection;
using Repository.Repositories;

namespace Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static void Register(this IServiceCollection services)
        {
            services.AddHttpContextAccessor();
            services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
            services.AddScoped<IJwtService, JwtService>();
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailService, EmailService>();


            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
            services.AddScoped<ICoffeeItemRepository, CoffeeItemRepository>();
            //-----------------DailyCupTracking-----------------
            services.AddScoped<IDailyCupTrackingRepository, DailyCupTrackingRepository>();
            services.AddScoped<IDailyCupTrackingService, DailyCupTrackingService>();
        }
    }
}
