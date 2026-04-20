using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Services;

using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Services;

using Microservicio.Vuelos.DataManagement.UoW;
using Microservicio.Vuelos.DataAccess.Context;

using Microsoft.EntityFrameworkCore;

namespace Microservicio.Vuelos.Api.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddProjectServices(this IServiceCollection services, IConfiguration configuration)
        {
            // ============================================================
            // 🔥 DB CONTEXT
            // ============================================================
            services.AddDbContext<SistemaVuelosDbContext>(options =>
                options.UseSqlServer(configuration.GetConnectionString("DefaultConnection")));

            // ============================================================
            // 🔥 UNIT OF WORK (ESTO TE FALTABA 💀)
            // ============================================================
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // ============================================================
            // 🔥 BUSINESS SERVICES (CAPA 3)
            // ============================================================
            services.AddScoped<IAuthService, AuthService>();
            services.AddScoped<IAeropuertoService, AeropuertoService>();
            services.AddScoped<IAsientoService, AsientoService>();
            services.AddScoped<IAuditoriaLogService, AuditoriaLogService>();
            services.AddScoped<IBoletoService, BoletoService>();
            services.AddScoped<ICiudadService, CiudadService>();
            services.AddScoped<IClienteService, ClienteService>();
            services.AddScoped<IEquipajeService, EquipajeService>();
            services.AddScoped<IEscalaService, EscalaService>();
            services.AddScoped<IFacturaService, FacturaService>();
            services.AddScoped<IMetodoPagoService, MetodoPagoService>();
            services.AddScoped<IPaisService, PaisService>();
            services.AddScoped<IPasajeroService, PasajeroService>();
            services.AddScoped<IReservaService, ReservaService>();
            services.AddScoped<IRolService, RolService>();
            services.AddScoped<ITipoMetodoPagoService, TipoMetodoPagoService>();
            services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            services.AddScoped<IUsuarioRolService, UsuarioRolService>();
            services.AddScoped<IVueloService, VueloService>();

            // ============================================================
            // 🔥 DATA SERVICES (CAPA 2)
            // ============================================================
            services.AddScoped<IAeropuertoDataService, AeropuertoDataService>();
            services.AddScoped<IAsientoDataService, AsientoDataService>();
            services.AddScoped<IAuditoriaLogDataService, AuditoriaLogDataService>();
            services.AddScoped<IBoletoDataService, BoletoDataService>();
            services.AddScoped<ICiudadDataService, CiudadDataService>();
            services.AddScoped<IClienteDataService, ClienteDataService>();
            services.AddScoped<IEquipajeDataService, EquipajeDataService>();
            services.AddScoped<IEscalaDataService, EscalaDataService>();
            services.AddScoped<IFacturaDataService, FacturaDataService>();
            services.AddScoped<IMetodoPagoDataService, MetodoPagoDataService>();
            services.AddScoped<IPaisDataService, PaisDataService>();
            services.AddScoped<IPasajeroDataService, PasajeroDataService>();
            services.AddScoped<IReservaDataService, ReservaDataService>();
            services.AddScoped<IRolDataService, RolDataService>();
            services.AddScoped<ITipoMetodoPagoDataService, TipoMetodoPagoDataService>();
            services.AddScoped<IUsuarioAppDataService, UsuarioAppDataService>();
            services.AddScoped<IUsuarioRolDataService, UsuarioRolDataService>();
            services.AddScoped<IVueloDataService, VueloDataService>();

            return services;
        }
    }
}