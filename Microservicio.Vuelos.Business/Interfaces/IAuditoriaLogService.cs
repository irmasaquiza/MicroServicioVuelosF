using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IAuditoriaLogService
    {
        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<AuditoriaLogResponse> GetByIdAsync(long id);

        // ============================================================
        // 🔥 LISTAR
        // ============================================================
        Task<IEnumerable<AuditoriaLogResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<AuditoriaLogResponse>> FiltrarAsync(AuditoriaLogFiltroRequest request);
    }
}