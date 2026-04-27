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
    public class AsientoRepository : IAsientoRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public AsientoRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<AsientoEntity>> GetAllAsync()
        {
            return await _context.Asientos
                .Where(a => !a.Eliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<AsientoEntity> GetByIdAsync(int id)
        {
            return await _context.Asientos
                .FirstOrDefaultAsync(a => a.IdAsiento == id && !a.Eliminado);
        }

        // 🔍 Obtener por vuelo
        public async Task<IEnumerable<AsientoEntity>> GetByVueloAsync(int idVuelo)
        {
            return await _context.Asientos
                .Where(a => a.IdVuelo == idVuelo && !a.Eliminado)
                .ToListAsync();
        }

        // 🔍 Buscar por número de asiento dentro de un vuelo
        public async Task<AsientoEntity> GetByNumeroAsync(int idVuelo, string numeroAsiento)
        {
            return await _context.Asientos
                .FirstOrDefaultAsync(a =>
                    a.IdVuelo == idVuelo &&
                    a.NumeroAsiento == numeroAsiento &&
                    !a.Eliminado);
        }

        // ➕ Crear
        public async Task AddAsync(AsientoEntity asiento)
        {
            await _context.Asientos.AddAsync(asiento);
        }

        // ✏️ Actualizar
        public void Update(AsientoEntity asiento)
        {
            _context.Asientos.Update(asiento);
        }

        // ❌ Eliminación lógica
        public void Delete(AsientoEntity asiento)
        {
            asiento.Eliminado = true;
            _context.Asientos.Update(asiento);
        }


        public async Task AddRangeAsync(IEnumerable<AsientoEntity> entities)
        {
            _context.Asientos.AddRange(entities);
            await _context.SaveChangesAsync();
        }
    }
}