using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IBoletoDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<BoletoDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<BoletoDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por reserva
        Task<IEnumerable<BoletoDataModel>> GetByReservaAsync(int idReserva);

        // 🔍 Obtener por vuelo
        Task<IEnumerable<BoletoDataModel>> GetByVueloAsync(int idVuelo);

        // 🔍 Obtener por factura
        Task<IEnumerable<BoletoDataModel>> GetByFacturaAsync(int idFactura);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<BoletoDataModel>> GetPagedAsync(BoletoFiltroDataModel filtro);

        // ➕ Crear
        Task<BoletoDataModel> CreateAsync(BoletoDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(BoletoDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}