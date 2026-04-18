using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IAeropuertoDataService
    {
        // 🔍 Obtener todos (sin paginar)
        Task<IEnumerable<AeropuertoDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<AeropuertoDataModel> GetByIdAsync(int id);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<AeropuertoDataModel>> GetPagedAsync(AeropuertoFiltroDataModel filtro);

        // ➕ Crear
        Task<AeropuertoDataModel> CreateAsync(AeropuertoDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(AeropuertoDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}