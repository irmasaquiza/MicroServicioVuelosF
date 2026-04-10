using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IAuditoriaLogRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<AuditoriaLogEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<AuditoriaLogEntity> GetByIdAsync(int id);

        // 🔍 Obtener por usuario
        Task<IEnumerable<AuditoriaLogEntity>> GetByUsuarioAsync(string usuario);

        // 🔍 Obtener por entidad (ej: "Vuelo", "Factura")
        Task<IEnumerable<AuditoriaLogEntity>> GetByEntidadAsync(string entidad);

        // 🔍 Obtener por rango de fechas
        Task<IEnumerable<AuditoriaLogEntity>> GetByFechaAsync(
            DateTime fechaInicio,
            DateTime fechaFin);

        // ➕ Registrar log (NO update normalmente)
        Task AddAsync(AuditoriaLogEntity log);
    }
}