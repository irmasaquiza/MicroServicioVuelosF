using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class VueloRepository : IVueloRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public VueloRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<VueloEntity>> GetAllAsync()
        {
            return await _context.Vuelos
                .Where(v => !v.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<VueloEntity> GetByIdAsync(int id)
        {
            return await _context.Vuelos
                .FirstOrDefaultAsync(v => v.IdVuelo == id && !v.EsEliminado);
        }

        // 🔍 Buscar por código
        public async Task<VueloEntity> GetByCodigoAsync(string codigoVuelo)
        {
            return await _context.Vuelos
                .FirstOrDefaultAsync(v =>
                    v.CodigoVuelo == codigoVuelo &&
                    !v.EsEliminado);
        }

        // 🔍 Buscar por ruta
        public async Task<IEnumerable<VueloEntity>> GetByRutaAsync(int idOrigen, int idDestino)
        {
            return await _context.Vuelos
                .Where(v =>
                    v.IdAeropuertoOrigen == idOrigen &&
                    v.IdAeropuertoDestino == idDestino &&
                    !v.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Buscar por fecha (CORREGIDO)
        public async Task<IEnumerable<VueloEntity>> GetByFechaAsync(DateTime fecha)
        {
            return await _context.Vuelos
                .Where(v =>
                    v.FechaHoraSalida.Date == fecha.Date &&
                    !v.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Buscar por estado
        public async Task<IEnumerable<VueloEntity>> GetByEstadoAsync(string estadoVuelo)
        {
            return await _context.Vuelos
                .Where(v =>
                    v.EstadoVuelo == estadoVuelo &&
                    !v.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Disponibles
        public async Task<IEnumerable<VueloEntity>> GetDisponiblesAsync()
        {
            return await _context.Vuelos
                .Where(v =>
                    v.CapacidadDisponible > 0 &&
                    v.EstadoVuelo == "PROGRAMADO" &&
                    !v.EsEliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(VueloEntity vuelo)
        {
            await _context.Vuelos.AddAsync(vuelo);
        }

        // ✏️ Actualizar
        public void Update(VueloEntity vuelo)
        {
            _context.Vuelos.Update(vuelo);
        }

        // ❌ Eliminación lógica
        public void Delete(VueloEntity vuelo)
        {
            vuelo.EsEliminado = true;
            _context.Vuelos.Update(vuelo);
        }
    }
}
