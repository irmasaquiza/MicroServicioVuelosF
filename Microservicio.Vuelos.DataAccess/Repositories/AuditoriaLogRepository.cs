using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class AuditoriaLogRepository : IAuditoriaLogRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public AuditoriaLogRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<AuditoriaLogEntity>> GetAllAsync()
        {
            return await _context.AuditoriaLogs.ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<AuditoriaLogEntity> GetByIdAsync(long id)
        {
            return await _context.AuditoriaLogs
                .FirstOrDefaultAsync(a => a.IdAuditoria == id);
        }

        // 🔍 Obtener por usuario
        public async Task<IEnumerable<AuditoriaLogEntity>> GetByUsuarioAsync(string usuario)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.UsuarioEjecutor == usuario)
                .ToListAsync();
        }

        // 🔍 Obtener por tabla
        public async Task<IEnumerable<AuditoriaLogEntity>> GetByTablaAsync(string tabla)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.TablaAfectada == tabla)
                .ToListAsync();
        }

        // 🔍 Obtener por operación
        public async Task<IEnumerable<AuditoriaLogEntity>> GetByOperacionAsync(string operacion)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.Operacion == operacion)
                .ToListAsync();
        }

        // 🔍 Obtener por registro afectado
        public async Task<IEnumerable<AuditoriaLogEntity>> GetByRegistroAsync(string idRegistro)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.IdRegistroAfectado == idRegistro)
                .ToListAsync();
        }

        // 🔍 Obtener por rango de fechas
        public async Task<IEnumerable<AuditoriaLogEntity>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin)
        {
            return await _context.AuditoriaLogs
                .Where(a => a.FechaEventoUtc >= fechaInicio && a.FechaEventoUtc <= fechaFin)
                .ToListAsync();
        }

        // 🔍 Obtener solo activos
        public async Task<IEnumerable<AuditoriaLogEntity>> GetActivosAsync()
        {
            return await _context.AuditoriaLogs
                .Where(a => a.Activo)
                .ToListAsync();
        }

        // ➕ Crear log
        public async Task AddAsync(AuditoriaLogEntity log)
        {
            await _context.AuditoriaLogs.AddAsync(log);
        }
    }
}