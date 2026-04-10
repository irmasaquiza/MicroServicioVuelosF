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
    public class AeropuertoRepository : IAeropuertoRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public AeropuertoRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos (sin eliminados)
        public async Task<IEnumerable<AeropuertoEntity>> GetAllAsync()
        {
            return await _context.Aeropuertos
                .Where(a => !a.Eliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<AeropuertoEntity> GetByIdAsync(int id)
        {
            return await _context.Aeropuertos
                .FirstOrDefaultAsync(a => a.IdAeropuerto == id && !a.Eliminado);
        }

        // 🔍 Buscar por código (IATA o ICAO)
        public async Task<AeropuertoEntity> GetByCodigoAsync(string codigo)
        {
            return await _context.Aeropuertos
                .FirstOrDefaultAsync(a =>
                    (a.CodigoIata == codigo || a.CodigoIcao == codigo)
                    && !a.Eliminado);
        }

        // ➕ Crear
        public async Task AddAsync(AeropuertoEntity aeropuerto)
        {
            await _context.Aeropuertos.AddAsync(aeropuerto);
        }

        // ✏️ Actualizar
        public void Update(AeropuertoEntity aeropuerto)
        {
            _context.Aeropuertos.Update(aeropuerto);
        }

        // ❌ Eliminación lógica (SÍ existe en tu entity: Eliminado)
        public void Delete(AeropuertoEntity aeropuerto)
        {
            aeropuerto.Eliminado = true;
            _context.Aeropuertos.Update(aeropuerto);
        }
    }
}