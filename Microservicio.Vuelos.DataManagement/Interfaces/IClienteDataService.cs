using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IClienteDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<ClienteDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<ClienteDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por identificación (muy importante 💀)
        Task<ClienteDataModel> GetByIdentificacionAsync(string numeroIdentificacion);

        // 🔍 Obtener por correo
        Task<ClienteDataModel> GetByCorreoAsync(string correo);

        // 🔍 Obtener por país
        Task<IEnumerable<ClienteDataModel>> GetByPaisAsync(int idPais);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<ClienteDataModel>> GetPagedAsync(ClienteFiltroDataModel filtro);

        // ➕ Crear
        Task<ClienteDataModel> CreateAsync(ClienteDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(ClienteDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}