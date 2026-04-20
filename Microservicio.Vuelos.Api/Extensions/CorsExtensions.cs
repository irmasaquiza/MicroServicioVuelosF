namespace Microservicio.Vuelos.Api.Extensions
{
    public static class CorsExtensions
    {
        private const string CorsPolicyName = "CorsPolicy";

        public static IServiceCollection AddCorsExtension(this IServiceCollection services, IConfiguration configuration)
        {
            var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>();

            services.AddCors(options =>
            {
                options.AddPolicy(CorsPolicyName, policy =>
                {
                    policy
                        .WithOrigins(allowedOrigins ?? Array.Empty<string>())
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials();
                });
            });

            return services;
        }

        public static IApplicationBuilder UseCorsExtension(this IApplicationBuilder app)
        {
            app.UseCors(CorsPolicyName);
            return app;
        }
    }
}