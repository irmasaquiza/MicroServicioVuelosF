using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IPaisDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<PaisDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<PaisDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por código ISO2 (CLAVE 💀)
        Task<PaisDataModel> GetByIso2Async(string codigoIso2);

        // 🔍 Obtener por código ISO3
        Task<PaisDataModel> GetByIso3Async(string codigoIso3);

        // 🔍 Obtener por continente
        Task<IEnumerable<PaisDataModel>> GetByContinenteAsync(string continente);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<PaisDataModel>> GetPagedAsync(PaisFiltroDataModel filtro);

        // ➕ Crear
        Task<PaisDataModel> CreateAsync(PaisDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(PaisDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}