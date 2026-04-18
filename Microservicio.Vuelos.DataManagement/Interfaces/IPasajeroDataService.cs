using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IPasajeroDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<PasajeroDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<PasajeroDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por cliente (cuando aplica)
        Task<IEnumerable<PasajeroDataModel>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener por número de documento (CLAVE 💀)
        Task<PasajeroDataModel> GetByDocumentoAsync(string numeroDocumento);

        // 🔍 Obtener por nacionalidad
        Task<IEnumerable<PasajeroDataModel>> GetByNacionalidadAsync(string nacionalidad);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<PasajeroDataModel>> GetPagedAsync(PasajeroFiltroDataModel filtro);

        // ➕ Crear
        Task<PasajeroDataModel> CreateAsync(PasajeroDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(PasajeroDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}