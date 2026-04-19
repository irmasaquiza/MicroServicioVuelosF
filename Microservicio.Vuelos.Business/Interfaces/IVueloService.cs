using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Vuelo;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IVueloService
    {
        // ============================================================
        // 🔥 CREAR VUELO
        // ============================================================
        Task<VueloResponse> CrearAsync(CrearVueloRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<VueloResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 OBTENER DETALLE (incluye escalas y asientos)
        // ============================================================
        Task<VueloDetalleResponse> GetDetalleAsync(int id);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<VueloResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<VueloResponse>> FiltrarAsync(VueloFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<VueloResponse> ActualizarAsync(int id, ActualizarVueloRequest request);

        // ============================================================
        // 🔥 CAMBIAR ESTADO (PROGRAMADO / EN_VUELO / ATERRIZADO / CANCELADO)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoVueloRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR DISPONIBILIDAD (usado por reservas)
        // ============================================================
        Task<bool> ActualizarDisponibilidadAsync(int id, int cantidad);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}