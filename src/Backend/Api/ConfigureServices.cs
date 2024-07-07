using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System.Text;

namespace Api;

public static class ConfigureServices
{
    /// <summary>
    /// Сконфигурировать аутентификацию.
    /// </summary>
    public static IServiceCollection ConfigureAuthentication(this IServiceCollection services)
    {
        services
            .AddAuthentication(
        auth =>
        {
            auth.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            auth.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        }).AddJwtBearer(jwt =>
        {
            jwt.RequireHttpsMetadata = true;
            jwt.SaveToken = true;
            jwt.TokenValidationParameters = new TokenValidationParameters
            {
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes("maflendmozarelladineshismailmishamaflendmozarelladineshismailmisha")),
                ValidateIssuer = false, //1 ->
                ValidateAudience = false //TODO менять по мере нужности, нужно запомнить и разделить но в целом только фронту эти две
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


/*AddSwaggerGen(options =>
        {
    opt.UseDateOnlyTimeOnlyStringConverters();
    options.SwaggerDoc("Версия первая апишки", new Microsoft.OpenApi.Models.OpenApiInfo
    {

        Title = "Аутфикация для code guru",
        Version = "v1",
    });
    var securityScheme = new OpenApiSecurityScheme
    {
        Name = "JWT Аутификация",
        Description = "Введите токен сюда",
        In = ParameterLocation.Header,
        Type = SecuritySchemeType.Http,
        Scheme = "bearer",
        BearerFormat = "JWT"
    };

    options.AddSecurityDefinition("Bearer", securityScheme);

    var securityRequirement = new OpenApiSecurityRequirement
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
    };

    options.AddSecurityRequirement(securityRequirement);


});*/