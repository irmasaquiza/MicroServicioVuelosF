using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface ITipoMetodoPagoDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<TipoMetodoPagoDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<TipoMetodoPagoDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por nombre (CLAVE 💀)
        Task<TipoMetodoPagoDataModel> GetByNombreAsync(string nombreTipo);

        // 🔍 Obtener activos
        Task<IEnumerable<TipoMetodoPagoDataModel>> GetActivosAsync();

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<TipoMetodoPagoDataModel>> GetPagedAsync(TipoMetodoPagoFiltroDataModel filtro);

        // ➕ Crear
        Task<TipoMetodoPagoDataModel> CreateAsync(TipoMetodoPagoDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(TipoMetodoPagoDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}