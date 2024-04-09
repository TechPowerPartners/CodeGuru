using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Guard.Api;

public static class ConfigureServices
{
    /// <summary>
    /// Сконфигурировать аутентификацию.
    /// </summary>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(opt =>
            {
                opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = "Swagger",
                    ValidAudience = "Sms",
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("ismailismailismailismailismailismailismail"))
                };
            });

        return services;
    }

    /// <summary>
    /// Сконфигурировать Swagger.
    /// </summary>
    public static IServiceCollection ConfigureSwagger(this IServiceCollection services)
    {
        services.AddSwaggerGen(opt =>
        {
            opt.UseDateOnlyTimeOnlyStringConverters();

            opt.SwaggerDoc("v1", new OpenApiInfo { Title = "GuardApi", Version = "v0.1" });

            opt.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                In = ParameterLocation.Header,
                Description = "token",
                Name = "Authorization",
                Type = SecuritySchemeType.Http,
                BearerFormat = "JWT",
                Scheme = "bearer"
            });

            opt.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type=ReferenceType.SecurityScheme,
                            Id="Bearer"
                        }
                    },
                    []
                }
            });
        });

        return services;
    }
}
