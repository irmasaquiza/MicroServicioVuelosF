using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Escala;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IEscalaService
    {
        // ============================================================
        // 🔥 CREAR ESCALA EN UN VUELO
        // ============================================================
        Task<EscalaResponse> CrearAsync(int idVuelo, CrearEscalaRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<EscalaResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR ESCALAS POR VUELO
        // ============================================================
        Task<IEnumerable<EscalaResponse>> GetByVueloAsync(int idVuelo);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<EscalaResponse> ActualizarAsync(int id, ActualizarEscalaRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}