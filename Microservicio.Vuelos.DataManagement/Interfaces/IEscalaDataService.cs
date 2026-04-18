using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IEscalaDataService
    {
        // 🔍 Obtener todas
        Task<IEnumerable<EscalaDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<EscalaDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por vuelo (CLAVE 💀)
        Task<IEnumerable<EscalaDataModel>> GetByVueloAsync(int idVuelo);

        // 🔍 Obtener por aeropuerto
        Task<IEnumerable<EscalaDataModel>> GetByAeropuertoAsync(int idAeropuerto);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<EscalaDataModel>> GetPagedAsync(EscalaFiltroDataModel filtro);

        // ➕ Crear
        Task<EscalaDataModel> CreateAsync(EscalaDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(EscalaDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}