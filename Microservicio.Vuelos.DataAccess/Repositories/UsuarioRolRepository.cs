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
    public class UsuarioRolRepository : IUsuarioRolRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public UsuarioRolRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        public async Task<UsuarioRolEntity> GetByIdAsync(int id)
        {
            return await _context.UsuariosRoles
                .FirstOrDefaultAsync(ur => ur.IdUsuarioRol == id && !ur.EsEliminado);
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<UsuarioRolEntity>> GetAllAsync()
        {
            return await _context.UsuariosRoles
                .Where(ur => !ur.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener roles por usuario
        public async Task<IEnumerable<UsuarioRolEntity>> GetByUsuarioAsync(int idUsuario)
        {
            return await _context.UsuariosRoles
                .Where(ur => ur.IdUsuario == idUsuario && !ur.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener usuarios por rol
        public async Task<IEnumerable<UsuarioRolEntity>> GetByRolAsync(int idRol)
        {
            return await _context.UsuariosRoles
                .Where(ur => ur.IdRol == idRol && !ur.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Validar si un usuario tiene un rol
        public async Task<UsuarioRolEntity> GetByUsuarioAndRolAsync(int idUsuario, int idRol)
        {
            return await _context.UsuariosRoles
                .FirstOrDefaultAsync(ur =>
                    ur.IdUsuario == idUsuario &&
                    ur.IdRol == idRol &&
                    !ur.EsEliminado);
        }

        // ➕ Asignar rol
        public async Task AddAsync(UsuarioRolEntity usuarioRol)
        {
            await _context.UsuariosRoles.AddAsync(usuarioRol);
        }
        public void Update(UsuarioRolEntity entity)
        {
            _context.UsuariosRoles.Update(entity);
        }

        // ❌ Quitar rol (soft delete según TU entity)
        public void Delete(UsuarioRolEntity usuarioRol)
        {
            usuarioRol.EsEliminado = true;
            _context.UsuariosRoles.Update(usuarioRol);
        }
    }
}