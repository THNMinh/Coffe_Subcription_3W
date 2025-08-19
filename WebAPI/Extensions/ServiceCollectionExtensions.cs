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
            services.AddScoped<IJwtService, JwtService>();

            //-----------------User-----------------
            services.AddScoped<IUserService, UserService>();
            services.AddScoped<IUserRepository, UserRepository>();
            services.AddScoped<IEmailService, EmailService>();

            //-----------------UserSubcription-----------------
            services.AddScoped<IUserSubcriptionRepository, UserSubcriptionRepository>();
            services.AddScoped<IUserSubcriptionService, UserSubcriptionService>();

            //-----------------SubscriptionPlan-----------------
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();

            //-----------------CoffeeItem-----------------
            services.AddScoped<ICoffeeItemRepository, CoffeeItemRepository>();
            services.AddScoped<ICoffeeItemService, CoffeeItemService>();

            //-----------------GoogleAuth-----------------
            services.AddScoped<IGoogleAuthService, GoogleAuthService>();

            //-----------------DailyCupTracking-----------------
            services.AddScoped<IDailyCupTrackingRepository, DailyCupTrackingRepository>();
            services.AddScoped<IDailyCupTrackingService, DailyCupTrackingService>();
            services.AddHostedService<DailyTrackingBackgroundService>();

            //-----------------CoffeeRedemption-----------------
            services.AddScoped<ICoffeeRedemptionRepository, CoffeeRedemptionRepository>();
            services.AddScoped<ICoffeeRedemptionService, CoffeeRedemptionService>();
          
            //-----------------SubscriptionPlan-----------------
            services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
            services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();

            //-----------------SubscriptionTimeWindow-----------------
            services.AddScoped<ISubscriptionTimeWindowRepository, SubscriptionTimeWindowRepository>();
            services.AddScoped<ISubscriptionTimeWindowService, SubscriptionTimeWindowService>();


            //-----------------PaymentTransaction-----------------
            services.AddScoped<IPaymentTransactionRepository, PaymentTransactionRepository>();
            services.AddScoped<IPaymentTransactionService, PaymentTransactionService>();

            //-----------------PlanCoffeOption-----------------
            services.AddScoped<IPlanCoffeeOptionRepository, PlanCoffeeOptionRepository>();
            services.AddScoped<IPlanCoffeeOptionService, PlanCoffeeOptionService>();


            //-----------------Cloudinary-----------------
            services.AddScoped<ICloudinaryService, CloudinaryService>();


        }
    }
}
