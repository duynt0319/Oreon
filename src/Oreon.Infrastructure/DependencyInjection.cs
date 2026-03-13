using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using Oreon.Application.Abstractions.Services;
using Oreon.Application.Common.Settings;
using Oreon.Domain.Abstractions.Services;
using Oreon.Infrastructure.Identity;
using Oreon.Infrastructure.Identity.Services;
using Oreon.Infrastructure.Persistence;
using Oreon.Infrastructure.Services;
using System.Text;

namespace Oreon.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        // Database Context
        services.AddDbContext<DataContext>(opt =>
            opt.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

        // ASP.NET Core Identity Configuration
        services.AddIdentityCore<AppUser>(opt =>
        {
            opt.Password.RequireDigit = true;
            opt.Password.RequireLowercase = true;
            opt.Password.RequireUppercase = true;
            opt.Password.RequireNonAlphanumeric = false;
            opt.Password.RequiredLength = 6;
            opt.User.RequireUniqueEmail = true;
        })
        .AddRoles<AppRole>()
        .AddRoleManager<RoleManager<AppRole>>()
        .AddSignInManager<SignInManager<AppUser>>()
        .AddEntityFrameworkStores<DataContext>();

        // JWT Authentication Configuration
        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(opt =>
            {
                opt.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["TokenKey"])),
                    ValidateIssuer = false,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ClockSkew = TimeSpan.Zero
                };

                // Support authentication via query string for SignalR
                opt.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        var accessToken = context.Request.Query["access_token"];
                        var path = context.HttpContext.Request.Path;
                        if (!string.IsNullOrEmpty(accessToken) && path.StartsWithSegments("/hubs"))
                            context.Token = accessToken;
                        return Task.CompletedTask;
                    }
                };
            });

        // HttpContextAccessor for CurrentUserService
        services.AddHttpContextAccessor();

        // Application Settings
        services.Configure<CloudinarySettings>(configuration.GetSection("CloudinarySettings"));

        // Domain Services (abstraction in Domain, implementation in Infrastructure)
        services.AddScoped<ICurrentUserService, CurrentUserService>();

        // Infrastructure Services
        //services.AddScoped<IUnitOfWork, UnitOfWork>();
        //services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IPhotoService, PhotoService>();
        //services.AddScoped<IIdentityService, IdentityService>();

        return services;
    }
}
