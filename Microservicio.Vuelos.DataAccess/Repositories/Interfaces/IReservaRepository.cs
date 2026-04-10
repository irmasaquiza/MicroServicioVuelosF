using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IReservaRepository
    {
        // 🔍 Obtener todas
        Task<IEnumerable<ReservaEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<ReservaEntity> GetByIdAsync(int id);

        // 🔍 Obtener por GUID (uso interno / tracking)
        Task<ReservaEntity> GetByGuidAsync(Guid reservaGuid);

        // 🔍 Obtener por código de reserva (PNR)
        Task<ReservaEntity> GetByCodigoAsync(string codigoReserva);

        // 🔍 Obtener reservas por cliente
        Task<IEnumerable<ReservaEntity>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener reservas por pasajero
        Task<IEnumerable<ReservaEntity>> GetByPasajeroAsync(int idPasajero);

        // 🔍 Obtener reservas por estado
        Task<IEnumerable<ReservaEntity>> GetByEstadoAsync(string estadoReserva);

        // ➕ Crear
        Task AddAsync(ReservaEntity reserva);

        // ✏️ Actualizar
        void Update(ReservaEntity reserva);

        // ❌ Eliminación lógica
        void Delete(ReservaEntity reserva);
    }
}