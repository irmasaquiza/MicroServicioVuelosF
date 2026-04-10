using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IFacturaRepository
    {
        // 🔍 Obtener todas
        Task<IEnumerable<FacturaEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<FacturaEntity> GetByIdAsync(int id);

        // 🔍 Obtener por número de factura
        Task<FacturaEntity> GetByNumeroAsync(string numeroFactura);

        // 🔍 Obtener facturas por cliente
        Task<IEnumerable<FacturaEntity>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener facturas por reserva
        Task<IEnumerable<FacturaEntity>> GetByReservaAsync(int idReserva);

        // 🔍 Obtener facturas por estado (ABI, APR, INA)
        Task<IEnumerable<FacturaEntity>> GetByEstadoAsync(string estado);

        // ➕ Crear
        Task AddAsync(FacturaEntity factura);

        // ✏️ Actualizar
        void Update(FacturaEntity factura);

        // ❌ Eliminación lógica
        void Delete(FacturaEntity factura);
    }
}