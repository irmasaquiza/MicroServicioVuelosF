using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioAppRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<UsuarioAppEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<UsuarioAppEntity?> GetByIdAsync(int id);

        // 🔍 Buscar por username
        Task<UsuarioAppEntity?> GetByUsernameAsync(string username);

        // 🔍 Buscar por correo
        Task<UsuarioAppEntity?> GetByCorreoAsync(string correo);

        // 🔍 Validar login
        Task<UsuarioAppEntity?> GetByCredencialesAsync(string usernameOrEmail, string passwordHash);

        // ➕ Crear
        Task AddAsync(UsuarioAppEntity usuario);

        // ✏️ Actualizar
        void Update(UsuarioAppEntity usuario);

        // ❌ Eliminación lógica
        void Delete(UsuarioAppEntity usuario);
    }
}