using Microsoft.OpenApi.Models;

namespace Microservicio.Vuelos.Api.Extensions
{
    public static class SwaggerExtensions
    {
        public static IServiceCollection AddSwaggerExtension(this IServiceCollection services)
        {
            services.AddEndpointsApiExplorer();

            services.AddSwaggerGen(options =>
            {
                // ============================================================
                // 🔥 INFO GENERAL
                // ============================================================
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Microservicio Vuelos API",
                    Version = "v1",
                    Description = "API REST para gestión de vuelos, reservas y boletos"
                });

                // ============================================================
                // 🔐 CONFIGURACIÓN JWT
                // ============================================================
                options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description = "Ingrese el token JWT así: Bearer {tu_token}"
                });

                options.AddSecurityRequirement(new OpenApiSecurityRequirement
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

            return services;
        }

        public static IApplicationBuilder UseSwaggerExtension(this IApplicationBuilder app)
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "Microservicio Vuelos API v1");

                options.RoutePrefix = string.Empty; // 👉 Swagger en raíz: http://localhost:xxxx/
            });

            return app;
        }
    }
}