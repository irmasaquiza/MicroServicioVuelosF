using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Reserva;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IReservaService
    {
        // ============================================================
        // 🔥 CREAR RESERVA (FLOW COMPLETO)
        // ============================================================
        Task<ReservaResponse> CrearAsync(CrearReservaRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<ReservaResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 OBTENER DETALLE COMPLETO
        // (incluye boletos + facturas)
        // ============================================================
        Task<ReservaDetalleResponse> GetDetalleAsync(int id);

        // ============================================================
        // 🔥 LISTAR POR CLIENTE
        // ============================================================
        Task<IEnumerable<ReservaResponse>> GetByClienteAsync(int idCliente);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<ReservaResponse>> FiltrarAsync(ReservaFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<ReservaResponse> ActualizarAsync(int id, ActualizarReservaRequest request);

        // ============================================================
        // 🔥 CAMBIAR ESTADO
        // (PEN / CON / CAN / EXP / FIN / EMI)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoReservaRequest request);

        // ============================================================
        // 🔥 CANCELAR RESERVA (FLOW CRÍTICO)
        // ============================================================
        Task<bool> CancelarAsync(int id, string motivo);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}