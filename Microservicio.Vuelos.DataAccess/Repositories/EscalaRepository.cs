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
    public class EscalaRepository : IEscalaRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public EscalaRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todas
        public async Task<IEnumerable<EscalaEntity>> GetAllAsync()
        {
            return await _context.Escalas
                .Where(e => !e.Eliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<EscalaEntity> GetByIdAsync(int id)
        {
            return await _context.Escalas
                .FirstOrDefaultAsync(e => e.IdEscala == id && !e.Eliminado);
        }

        // 🔍 Obtener escalas por vuelo
        public async Task<IEnumerable<EscalaEntity>> GetByVueloAsync(int idVuelo)
        {
            return await _context.Escalas
                .Where(e => e.IdVuelo == idVuelo && !e.Eliminado)
                .OrderBy(e => e.Orden)
                .ToListAsync();
        }

        // 🔍 Obtener escala específica por vuelo + orden
        public async Task<EscalaEntity> GetByVueloYOrdenAsync(int idVuelo, int orden)
        {
            return await _context.Escalas
                .FirstOrDefaultAsync(e =>
                    e.IdVuelo == idVuelo &&
                    e.Orden == orden &&
                    !e.Eliminado);
        }

        // ➕ Crear
        public async Task AddAsync(EscalaEntity escala)
        {
            await _context.Escalas.AddAsync(escala);
        }

        // ✏️ Actualizar
        public void Update(EscalaEntity escala)
        {
            _context.Escalas.Update(escala);
        }

        // ❌ Eliminación lógica
        public void Delete(EscalaEntity escala)
        {
            escala.Eliminado = true;
            _context.Escalas.Update(escala);
        }
    }
}