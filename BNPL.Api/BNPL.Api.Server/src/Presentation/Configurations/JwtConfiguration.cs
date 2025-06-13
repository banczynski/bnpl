using Amazon.CognitoIdentityProvider;
using BNPL.Api.Server.src.Application.Abstractions.Identity;
using BNPL.Api.Server.src.Infrastructure.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;

namespace BNPL.Api.Server.src.Presentation.Configurations
{
    public static class JwtConfiguration
    {
        public static IServiceCollection AddJwtAuthentication(this IServiceCollection services, IConfiguration configuration)
        {
            var authority = configuration["AWS:Cognito:Authority"];

            if (string.IsNullOrWhiteSpace(authority))
                throw new InvalidOperationException("Cognito configuration missing. Check 'AWS:Cognito:Authority' and 'AWS:Cognito:Audience'.");

            services.AddAuthentication(options =>
            {
                options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
                options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
            })
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = bool.Parse(configuration["AWS:Cognito:RequireHttpsMetadata"] ?? "true");
                options.Authority = authority;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = false,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true
                };
            });

            services.AddDefaultAWSOptions(configuration.GetAWSOptions());
            services.AddAWSService<IAmazonCognitoIdentityProvider>();

            services.AddScoped<ICognitoAuthService, CognitoAuthService>();
            services.AddScoped<ICognitoUserService, CognitoUserService>();

            services.AddAuthorization();

            return services;
        }
    }
}
