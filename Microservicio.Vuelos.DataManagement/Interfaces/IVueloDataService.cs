using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IVueloDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<VueloDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<VueloDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por código de vuelo (CLAVE 💀)
        Task<VueloDataModel> GetByCodigoAsync(string codigoVuelo);

        // 🔍 Obtener por aeropuerto origen
        Task<IEnumerable<VueloDataModel>> GetByOrigenAsync(int idAeropuertoOrigen);

        // 🔍 Obtener por aeropuerto destino
        Task<IEnumerable<VueloDataModel>> GetByDestinoAsync(int idAeropuertoDestino);

        // 🔍 Obtener por rango de fechas
        Task<IEnumerable<VueloDataModel>> GetByFechaAsync(
            System.DateTime fechaInicio,
            System.DateTime fechaFin);

        // 🔍 Obtener por estado (PROGRAMADO, CANCELADO, etc.)
        Task<IEnumerable<VueloDataModel>> GetByEstadoAsync(string estadoVuelo);

        // 🔍 Obtener vuelos disponibles (CLAVE PARA BOOKING 💀)
        Task<IEnumerable<VueloDataModel>> GetDisponiblesAsync();

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<VueloDataModel>> GetPagedAsync(VueloFiltroDataModel filtro);

        // ➕ Crear
        Task<VueloDataModel> CreateAsync(VueloDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(VueloDataModel model);

        // ❌ Cancelar vuelo (MEJOR QUE BORRAR 💀)
        Task<bool> CancelAsync(int id, string motivo);

        // ❌ Eliminación lógica (opcional)
        Task<bool> DeleteAsync(int id);
    }
}