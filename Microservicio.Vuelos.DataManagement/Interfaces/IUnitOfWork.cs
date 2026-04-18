using Microservicio.Vuelos.DataAccess.Repositories;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using System;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        // 🔗 Repositories
        IPaisRepository PaisRepository { get; }
        ICiudadRepository CiudadRepository { get; }
        IAeropuertoRepository AeropuertoRepository { get; }
        IClienteRepository ClienteRepository { get; }
        IAuditoriaLogRepository AuditoriaLogRepository { get; }
        IUsuarioAppRepository UsuarioAppRepository { get; }
        IRolRepository RolRepository { get; }
        IUsuarioRolRepository UsuarioRolRepository { get; }
        ITipoMetodoPagoRepository TipoMetodoPagoRepository { get; }
        IMetodoPagoRepository MetodoPagoRepository { get; }
        IPasajeroRepository PasajeroRepository { get; }
        IReservaRepository ReservaRepository { get; }
        IFacturaRepository FacturaRepository { get; }
        IBoletoRepository BoletoRepository { get; }
        IEquipajeRepository EquipajeRepository { get; }
        IVueloRepository VueloRepository { get; }
        IEscalaRepository EscalaRepository { get; }
        IAsientoRepository AsientoRepository { get; }

        // 💾 Guardar cambios
        Task<int> SaveChangesAsync();

        // 🔄 Transacciones
        Task BeginTransactionAsync();
        Task CommitAsync();
        Task RollbackAsync();
    }
}