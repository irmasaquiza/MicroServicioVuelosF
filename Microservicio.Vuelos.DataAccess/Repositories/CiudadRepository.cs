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
    public class CiudadRepository : ICiudadRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public CiudadRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todas
        public async Task<IEnumerable<CiudadEntity>> GetAllAsync()
        {
            return await _context.Ciudades
                .Where(c => !c.Eliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<CiudadEntity> GetByIdAsync(int id)
        {
            return await _context.Ciudades
                .FirstOrDefaultAsync(c => c.IdCiudad == id && !c.Eliminado);
        }

        // 🔍 Obtener por país
        public async Task<IEnumerable<CiudadEntity>> GetByPaisAsync(int idPais)
        {
            return await _context.Ciudades
                .Where(c => c.IdPais == idPais && !c.Eliminado)
                .ToListAsync();
        }

        // 🔍 Buscar por nombre
        public async Task<IEnumerable<CiudadEntity>> GetByNombreAsync(string nombre)
        {
            return await _context.Ciudades
                .Where(c => c.Nombre.Contains(nombre) && !c.Eliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(CiudadEntity ciudad)
        {
            await _context.Ciudades.AddAsync(ciudad);
        }

        // ✏️ Actualizar
        public void Update(CiudadEntity ciudad)
        {
            _context.Ciudades.Update(ciudad);
        }

        // ❌ Eliminación lógica
        public void Delete(CiudadEntity ciudad)
        {
            ciudad.Eliminado = true;
            _context.Ciudades.Update(ciudad);
        }
    }
}