using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IAsientoDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<AsientoDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<AsientoDataModel> GetByIdAsync(int id);

        // 🔍 Obtener asientos por vuelo
        Task<IEnumerable<AsientoDataModel>> GetByVueloAsync(int idVuelo);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<AsientoDataModel>> GetPagedAsync(AsientoFiltroDataModel filtro);

        // ➕ Crear
        Task<AsientoDataModel> CreateAsync(AsientoDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(AsientoDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}