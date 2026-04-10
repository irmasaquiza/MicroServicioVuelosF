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

        // 🔍 Obtener por ID
        Task<AuditoriaLogEntity> GetByIdAsync(long id);

        // 🔍 Obtener por usuario
        Task<IEnumerable<AuditoriaLogEntity>> GetByUsuarioAsync(string usuario);

        // 🔍 Obtener por tabla
        Task<IEnumerable<AuditoriaLogEntity>> GetByTablaAsync(string tabla);

        // 🔍 Obtener por operación
        Task<IEnumerable<AuditoriaLogEntity>> GetByOperacionAsync(string operacion);

        // 🔍 Obtener por registro afectado
        Task<IEnumerable<AuditoriaLogEntity>> GetByRegistroAsync(string idRegistro);

        // 🔍 Obtener por rango de fechas
        Task<IEnumerable<AuditoriaLogEntity>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);

        // 🔍 Solo activos
        Task<IEnumerable<AuditoriaLogEntity>> GetActivosAsync();

        // ➕ Crear log
        Task AddAsync(AuditoriaLogEntity log);
    }
}