using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IAsientoService
    {
        // ============================================================
        // 🔥 CREAR ASIENTO EN UN VUELO
        // POST /vuelos/{id}/asientos
        // ============================================================
        Task<AsientoResponse> CrearAsync(int idVuelo, CrearAsientoRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<AsientoResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR POR VUELO
        // ============================================================
        Task<IEnumerable<AsientoResponse>> GetByVueloAsync(int idVuelo);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<AsientoResponse>> FiltrarAsync(AsientoFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<AsientoResponse> ActualizarAsync(int id, ActualizarAsientoRequest request);

        // ============================================================
        // 🔥 CAMBIAR DISPONIBILIDAD
        // (usado en reservas)
        // ============================================================
        Task<bool> CambiarDisponibilidadAsync(int id, bool disponible);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}