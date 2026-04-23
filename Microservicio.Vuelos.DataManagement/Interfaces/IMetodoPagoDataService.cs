/*using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IMetodoPagoDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<MetodoPagoDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<MetodoPagoDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por cliente (CLAVE 💀)
        Task<IEnumerable<MetodoPagoDataModel>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener por tipo de método (tarjeta, transferencia, etc.)
        Task<IEnumerable<MetodoPagoDataModel>> GetByTipoMetodoAsync(int idTipoMetodo);

        // 🔍 Obtener métodos principales
        Task<IEnumerable<MetodoPagoDataModel>> GetPrincipalesAsync(int idCliente);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<MetodoPagoDataModel>> GetPagedAsync(MetodoPagoFiltroDataModel filtro);

        // ➕ Crear
        Task<MetodoPagoDataModel> CreateAsync(MetodoPagoDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(MetodoPagoDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}*/