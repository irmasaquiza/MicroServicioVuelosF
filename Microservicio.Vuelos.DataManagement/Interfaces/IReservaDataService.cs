using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IReservaDataService
    {
        // 🔍 Obtener todas
        Task<IEnumerable<ReservaDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<ReservaDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por código de reserva (CLAVE 💀)
        Task<ReservaDataModel> GetByCodigoAsync(string codigoReserva);

        // 🔍 Obtener por cliente
        Task<IEnumerable<ReservaDataModel>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener por pasajero
        Task<IEnumerable<ReservaDataModel>> GetByPasajeroAsync(int idPasajero);

        // 🔍 Obtener por vuelo
        Task<IEnumerable<ReservaDataModel>> GetByVueloAsync(int idVuelo);

        // 🔍 Obtener por estado
        Task<IEnumerable<ReservaDataModel>> GetByEstadoAsync(string estadoReserva);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<ReservaDataModel>> GetPagedAsync(ReservaFiltroDataModel filtro);

        // ➕ Crear
        Task<ReservaDataModel> CreateAsync(ReservaDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(ReservaDataModel model);

        // ❌ Cancelar reserva (más correcto que eliminar 💀)
        Task<bool> CancelAsync(int id, string motivo);

        // ❌ Eliminación lógica (opcional)
        Task<bool> DeleteAsync(int id);
    }
}