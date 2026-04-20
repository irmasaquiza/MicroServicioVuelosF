using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using Microservicio.Vuelos.Api.Models.Settings;

namespace Microservicio.Vuelos.Api.Extensions
{
    public static class AuthenticationExtension
    {
        public static IServiceCollection AddAuthenticationExtension(
            this IServiceCollection services,
            IConfiguration configuration)
        {
            // 🔥 LEER BIEN TU CONFIG (CLAVE)
            var jwtSettings = configuration
                .GetSection("JwtSettings")
                .Get<JwtSettings>();

            // 🚨 VALIDACIÓN (esto evita tu error)
            if (jwtSettings == null)
                throw new Exception("No se pudo cargar JwtSettings");

            if (string.IsNullOrEmpty(jwtSettings.SecretKey))
                throw new Exception("JWT SecretKey está vacío");

            var key = Encoding.UTF8.GetBytes(jwtSettings.SecretKey);

            services.AddAuthentication(options =>
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

                    ValidIssuer = jwtSettings.Issuer,
                    ValidAudience = jwtSettings.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(key),

                    ClockSkew = TimeSpan.Zero
                };
            });

            return services;
        }
    }
}