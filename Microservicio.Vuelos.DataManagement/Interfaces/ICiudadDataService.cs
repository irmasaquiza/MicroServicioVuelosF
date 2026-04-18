using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface ICiudadDataService
    {
        // 🔍 Obtener todas
        Task<IEnumerable<CiudadDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<CiudadDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por país
        Task<IEnumerable<CiudadDataModel>> GetByPaisAsync(int idPais);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<CiudadDataModel>> GetPagedAsync(CiudadFiltroDataModel filtro);

        // ➕ Crear
        Task<CiudadDataModel> CreateAsync(CiudadDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(CiudadDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}