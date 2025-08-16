using Core.Interfaces.Repositories;
using Core.Interfaces.Services;
using  Core.Models;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using Repository.Repositories;
using Service.Services;
using System;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using VNPAY;

var builder = WebApplication.CreateBuilder(args);

#region Jwt configuration 

var jwtIssuer = builder.Configuration.GetSection("Jwt:Issuer").Get<string>();
var jwtAudience = builder.Configuration.GetSection("Jwt:Audience").Get<string>();
var jwtKey = builder.Configuration.GetSection("Jwt:Key").Get<string>();

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
     .AddJwtBearer(options =>
     {
         options.TokenValidationParameters = new TokenValidationParameters
         {
             ValidateIssuer = true,
             ValidateAudience = true,
             ValidateLifetime = true,
             ValidateIssuerSigningKey = true,
             ValidIssuer = jwtIssuer,
             ValidAudience = jwtAudience,
             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey)),
             RoleClaimType = ClaimTypes.Role
         };

         options.Events = new JwtBearerEvents
         {
             // Customize the 401 response
             OnChallenge = context =>
             {
                 // Skip the default response
                 context.HandleResponse();

                 // Customize the response
                 context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                 context.Response.ContentType = "application/json";

                 var result = JsonSerializer.Serialize(new
                 {
                     success = false,
                     message = "Access Denied. Token is missing or invalid."
                 });

                 return context.Response.WriteAsync(result);
             },

             // Customize the 403 response
             OnForbidden = context =>
             {
                 // Customize the response
                 context.Response.StatusCode = StatusCodes.Status403Forbidden;
                 context.Response.ContentType = "application/json";

                 var result = JsonSerializer.Serialize(new
                 {
                     success = false,
                     message = "Access Denied. You do not have permission to access this resource."
                 });

                 return context.Response.WriteAsync(result);
             }
         };
     });
#endregion
// Add services to the container.
builder.Services.AddScoped<IJwtService, JwtService>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IEmailService, EmailService>();


builder.Services.AddScoped<ISubscriptionPlanRepository, SubscriptionPlanRepository>();
builder.Services.AddScoped<ISubscriptionPlanService, SubscriptionPlanService>();
builder.Services.AddScoped<ICoffeeItemRepository, CoffeeItemRepository>();
 
// Add VNPAY service to the container.
builder.Services.AddSingleton<IVnpay, Vnpay>();
builder.Services.AddHttpContextAccessor();


builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

//1.Configure conn db
builder.Services.AddDbContext<CoffeSubContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection"))
);
#region Swagger Configuration

builder.Services.AddSwaggerGen(c =>
{
    c.SwaggerDoc("v1", new OpenApiInfo { Title = "Your API", Version = "v1" });

    // Add JWT Authentication to Swagger
    c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
    {
        Name = "Authorization",
        Type = SecuritySchemeType.Http,
        Scheme = "Bearer",
        BearerFormat = "JWT",
        In = ParameterLocation.Header,
        Description = "Enter your JWT token here. Example: Bearer {your token}"
    });

    c.AddSecurityRequirement(new OpenApiSecurityRequirement
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "Bearer"
                }
            },
            new string[] {}
        }
    });
});
#endregion

var app = builder.Build();

#region Middleware configuration
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Coffee Subscription API v1");
        c.RoutePrefix = string.Empty; // Swagger at root
    });
}
#endregion

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
