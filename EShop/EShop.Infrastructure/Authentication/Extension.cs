using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json;

namespace EShop.Infrastructure.Authentication
{
    public static class Extension
    {
        public static IServiceCollection AddJwt(this IServiceCollection collection, IConfiguration configuration)
        {
            //var options = new JwtOptions();
            //configuration.GetSection("jwt").Bind(options);
            //collection.Configure<JwtOptions>(x => x = options);
            collection.AddSingleton<IAuthenticationHandler, AuthenticationHandler>();
            //collection.AddAuthentication()
            //    .AddJwtBearer(cfg => {
            //        cfg.RequireHttpsMetadata = false;
            //        cfg.SaveToken = true;
            //        cfg.TokenValidationParameters = new Microsoft.IdentityModel.Tokens.TokenValidationParameters
            //        {
            //            ValidateAudience = false,
            //            ValidIssuer = options.Issuer,
            //            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(options.SecretKey))
            //        };
            //    });

            //return collection;

            collection.AddAuthentication()
            .AddJwtBearer(options =>
            {
                options.RequireHttpsMetadata = false;
                options.SaveToken = true;
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuerSigningKey = true,
                    IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8
                        .GetBytes(configuration.GetSection("jwt:SecretKey").Value)),
                    ValidateIssuer = false,
                    ValidateAudience = false
                };

            // ---------------------------------------------

            // ---------------------------------------------
            // https://stackoverflow.com/questions/71405661/invalid-jwt-bearer-token-on-net-6-0-web-api
            options.Events = new JwtBearerEvents()
            {
                OnChallenge = context =>
                {
                    context.HandleResponse();
                    context.Response.StatusCode = StatusCodes.Status401Unauthorized;
                    context.Response.ContentType = "application/json";

                    // Ensure we always have an error and error description.
                    if (string.IsNullOrEmpty(context.Error))
                        context.Error = "invalid_token";
                    if (string.IsNullOrEmpty(context.ErrorDescription))
                        context.ErrorDescription = "This request requires a valid JWT access token to be provided";

                    // Add some extra context for expired tokens.
                    if (context.AuthenticateFailure != null && context.AuthenticateFailure.GetType() == typeof(SecurityTokenExpiredException))
                    {
                        var authenticationException = context.AuthenticateFailure as SecurityTokenExpiredException;
                        context.Response.Headers.Add("x-token-expired", authenticationException.Expires.ToString("o"));
                        context.ErrorDescription = $"The token expired on {authenticationException.Expires.ToString("o")}";
                    }

                    return context.Response.WriteAsync(JsonSerializer.Serialize(new
                    {
                        error = context.Error,
                        error_description = context.ErrorDescription
                    }));
                }
            };
        });

            return collection;
        }
    }
}
