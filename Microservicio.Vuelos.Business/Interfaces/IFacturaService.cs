using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Factura;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IFacturaService
    {
        // ============================================================
        // 🔥 CREAR FACTURA
        // ============================================================
        Task<FacturaResponse> CrearAsync(CrearFacturaRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<FacturaResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR POR RESERVA
        // ============================================================
        Task<IEnumerable<FacturaResponse>> GetByReservaAsync(int idReserva);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<FacturaResponse>> FiltrarAsync(FacturaFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<FacturaResponse> ActualizarAsync(int id, ActualizarFacturaRequest request);

        Task AprobarAsync(int idFactura, int idUsuario);

        // ============================================================
        // 🔥 CAMBIAR ESTADO (ABI / APR / INA)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int id, string estado);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);

    }
}