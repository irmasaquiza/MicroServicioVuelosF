using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IFacturaDataService
    {
        // 🔍 Obtener todas
        Task<IEnumerable<FacturaDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<FacturaDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por cliente
        Task<IEnumerable<FacturaDataModel>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener por reserva
        Task<IEnumerable<FacturaDataModel>> GetByReservaAsync(int idReserva);

        // 🔍 Obtener por método de pago
        Task<IEnumerable<FacturaDataModel>> GetByMetodoPagoAsync(int idMetodo);

        // 🔍 Obtener por número de factura (CLAVE 💀)
        Task<FacturaDataModel> GetByNumeroAsync(string numeroFactura);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<FacturaDataModel>> GetPagedAsync(FacturaFiltroDataModel filtro);

        // ➕ Crear
        Task<FacturaDataModel> CreateAsync(FacturaDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(FacturaDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}