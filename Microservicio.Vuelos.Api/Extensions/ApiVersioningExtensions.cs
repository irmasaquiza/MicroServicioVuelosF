using Asp.Versioning;
using Asp.Versioning.ApiExplorer;

namespace Microservicio.Vuelos.Api.Extensions
{
    public static class ApiVersioningExtensions
    {
        public static IServiceCollection AddApiVersioningExtension(this IServiceCollection services)
        {
            // ============================================================
            // 🔥 CONFIGURAR VERSIONAMIENTO
            // ============================================================
            services.AddApiVersioning(options =>
            {
                options.DefaultApiVersion = new ApiVersion(1, 0);

                options.AssumeDefaultVersionWhenUnspecified = true;

                options.ReportApiVersions = true;

                // 🔥 Leer versión desde la URL
                options.ApiVersionReader = new UrlSegmentApiVersionReader();
            });

            // ============================================================
            // 🔥 EXPLORER (para Swagger)
            // ============================================================
            services.AddApiVersioning().AddApiExplorer(options =>
            {
                options.GroupNameFormat = "'v'VVV";

                options.SubstituteApiVersionInUrl = true;
            });

            return services;
        }
    }
}