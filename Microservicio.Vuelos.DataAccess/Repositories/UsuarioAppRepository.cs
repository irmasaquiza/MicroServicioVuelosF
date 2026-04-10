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
    public class UsuarioAppRepository : IUsuarioAppRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public UsuarioAppRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<UsuarioAppEntity>> GetAllAsync()
        {
            return await _context.UsuariosApp
                .Where(u => !u.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<UsuarioAppEntity?> GetByIdAsync(int id)
        {
            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u =>
                    u.IdUsuario == id &&
                    !u.EsEliminado);
        }

        // 🔍 Buscar por username
        public async Task<UsuarioAppEntity?> GetByUsernameAsync(string username)
        {
            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u =>
                    u.Username == username &&
                    !u.EsEliminado);
        }

        // 🔍 Buscar por correo
        public async Task<UsuarioAppEntity?> GetByCorreoAsync(string correo)
        {
            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u =>
                    u.Correo == correo &&
                    !u.EsEliminado);
        }

        // 🔍 Validar login
        public async Task<UsuarioAppEntity?> GetByCredencialesAsync(string usernameOrEmail, string passwordHash)
        {
            var input = usernameOrEmail.ToLower();

            return await _context.UsuariosApp
                .FirstOrDefaultAsync(u =>
                    (u.Username.ToLower() == input || u.Correo.ToLower() == input) &&
                    u.PasswordHash == passwordHash &&
                    !u.EsEliminado);
        }

        // ➕ Crear
        public async Task AddAsync(UsuarioAppEntity usuario)
        {
            await _context.UsuariosApp.AddAsync(usuario);
        }

        // ✏️ Actualizar
        public void Update(UsuarioAppEntity usuario)
        {
            _context.UsuariosApp.Update(usuario);
        }

        // ❌ Eliminación lógica
        public void Delete(UsuarioAppEntity usuario)
        {
            usuario.EsEliminado = true;
            _context.UsuariosApp.Update(usuario);
        }
    }
}