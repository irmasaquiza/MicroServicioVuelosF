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
    public class RolRepository : IRolRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public RolRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<RolEntity>> GetAllAsync()
        {
            return await _context.Roles
                .Where(r => !r.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<RolEntity> GetByIdAsync(int id)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r => r.IdRol == id && !r.EsEliminado);
        }

        // 🔍 Buscar por nombre
        public async Task<RolEntity> GetByNombreAsync(string nombreRol)
        {
            return await _context.Roles
                .FirstOrDefaultAsync(r =>
                    r.NombreRol == nombreRol &&
                    !r.EsEliminado);
        }

        // ➕ Crear
        public async Task AddAsync(RolEntity rol)
        {
            await _context.Roles.AddAsync(rol);
        }

        // ✏️ Actualizar
        public void Update(RolEntity rol)
        {
            _context.Roles.Update(rol);
        }

        // ❌ Eliminación lógica
        public void Delete(RolEntity rol)
        {
            rol.EsEliminado = true;
            _context.Roles.Update(rol);
        }
    }
}