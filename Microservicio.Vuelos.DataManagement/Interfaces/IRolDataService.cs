using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IRolDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<RolDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<RolDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por nombre (CLAVE 💀)
        Task<RolDataModel> GetByNombreAsync(string nombreRol);

        // 🔍 Obtener roles activos
        Task<IEnumerable<RolDataModel>> GetActivosAsync();

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<RolDataModel>> GetPagedAsync(RolFiltroDataModel filtro);

        // ➕ Crear
        Task<RolDataModel> CreateAsync(RolDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(RolDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}