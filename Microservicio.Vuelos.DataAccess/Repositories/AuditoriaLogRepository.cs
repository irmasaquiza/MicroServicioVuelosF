using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IAuditoriaLogRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<AuditoriaLogEntity>> GetAllAsync();

        // 🔍 Obtener por ID (CORREGIDO: long)
        Task<AuditoriaLogEntity> GetByIdAsync(long id);

        // 🔍 Obtener por usuario (CORREGIDO: UsuarioEjecutor)
        Task<IEnumerable<AuditoriaLogEntity>> GetByUsuarioAsync(string usuarioEjecutor);

        // 🔍 Obtener por tabla afectada (CORREGIDO)
        Task<IEnumerable<AuditoriaLogEntity>> GetByTablaAsync(string tablaAfectada);

        // 🔍 Obtener por operación (INSERT, UPDATE, DELETE)
        Task<IEnumerable<AuditoriaLogEntity>> GetByOperacionAsync(string operacion);

        // 🔍 Obtener por rango de fechas
        Task<IEnumerable<AuditoriaLogEntity>> GetByFechaAsync(
            DateTime fechaInicio,
            DateTime fechaFin);

        // ➕ Registrar log
        Task AddAsync(AuditoriaLogEntity log);
    }
}