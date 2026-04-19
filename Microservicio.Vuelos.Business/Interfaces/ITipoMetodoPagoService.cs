using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.TipoMetodoPago;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface ITipoMetodoPagoService
    {
        // ============================================================
        // 🔥 CREAR TIPO
        // ============================================================
        Task<TipoMetodoPagoResponse> CrearAsync(CrearTipoMetodoPagoRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<TipoMetodoPagoResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<TipoMetodoPagoResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<TipoMetodoPagoResponse>> FiltrarAsync(TipoMetodoPagoFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<TipoMetodoPagoResponse> ActualizarAsync(int id, ActualizarTipoMetodoPagoRequest request);

        // ============================================================
        // 🔥 CAMBIAR ESTADO (ACTIVO / INACTIVO)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int id, string estado);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}
