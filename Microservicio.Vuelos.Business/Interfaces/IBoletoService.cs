using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IBoletoService
    {
        // ============================================================
        // 🔥 CREAR BOLETO
        // ============================================================
        Task<BoletoResponse> CrearAsync(CrearBoletoRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<BoletoResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR POR RESERVA
        // ============================================================
        Task<IEnumerable<BoletoResponse>> GetByReservaAsync(int idReserva);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<BoletoResponse>> FiltrarAsync(BoletoFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<BoletoResponse> ActualizarAsync(int id, ActualizarBoletoRequest request);

        // ============================================================
        // 🔥 CAMBIAR ESTADO
        // ============================================================
        Task<bool> CambiarEstadoAsync(int id, string estado);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}