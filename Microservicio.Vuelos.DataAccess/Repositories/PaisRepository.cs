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
    public class PaisRepository : IPaisRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public PaisRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<PaisEntity>> GetAllAsync()
        {
            return await _context.Paises
                .Where(p => !p.Eliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<PaisEntity> GetByIdAsync(int id)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(p => p.IdPais == id && !p.Eliminado);
        }

        // 🔍 Buscar por ISO2
        public async Task<PaisEntity> GetByCodigoIso2Async(string codigoIso2)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(p =>
                    p.CodigoIso2 == codigoIso2 &&
                    !p.Eliminado);
        }

        // 🔍 Buscar por ISO3
        public async Task<PaisEntity> GetByCodigoIso3Async(string codigoIso3)
        {
            return await _context.Paises
                .FirstOrDefaultAsync(p =>
                    p.CodigoIso3 == codigoIso3 &&
                    !p.Eliminado);
        }

        // 🔍 Buscar por nombre
        public async Task<IEnumerable<PaisEntity>> GetByNombreAsync(string nombre)
        {
            return await _context.Paises
                .Where(p =>
                    p.Nombre.Contains(nombre) &&
                    !p.Eliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(PaisEntity pais)
        {
            await _context.Paises.AddAsync(pais);
        }

        // ✏️ Actualizar
        public void Update(PaisEntity pais)
        {
            _context.Paises.Update(pais);
        }

        // ❌ Eliminación lógica
        public void Delete(PaisEntity pais)
        {
            pais.Eliminado = true;
            _context.Paises.Update(pais);
        }
    }
}