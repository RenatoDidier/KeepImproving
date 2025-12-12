using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace KeepImproving.API.Extensions;

public static class AuthJwtExtension
{
    public static IServiceCollection AddJwtAuth(this IServiceCollection services, IConfiguration configuration)
    {
        IConfigurationSection? configurationJwt = configuration.GetSection("Jwt");

        string secret = configurationJwt["Key"]!;
        byte[] key = Encoding.ASCII.GetBytes(secret);

        services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.MapInboundClaims = false;

                    options.TokenValidationParameters = new TokenValidationParameters 
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidAudience = configurationJwt["Audience"],
                        ValidIssuer = configurationJwt["Issuer"]
                    };
                });

        return services;
    }
}