using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IUsuarioRolRepository
    {
        Task<UsuarioRolEntity> GetByIdAsync(int id); // 👈 AGREGA ESTO

        // 🔍 Obtener todos
        Task<IEnumerable<UsuarioRolEntity>> GetAllAsync();

        // 🔍 Obtener roles por usuario
        Task<IEnumerable<UsuarioRolEntity>> GetByUsuarioAsync(int idUsuario);

        // 🔍 Obtener usuarios por rol
        Task<IEnumerable<UsuarioRolEntity>> GetByRolAsync(int idRol);

        // 🔍 Validar si un usuario tiene un rol
        Task<UsuarioRolEntity> GetByUsuarioAndRolAsync(int idUsuario, int idRol);

        // ➕ Asignar rol a usuario
        Task AddAsync(UsuarioRolEntity usuarioRol);

        void Update(UsuarioRolEntity entity); // 🔥 AGREGA ESTO


        // ❌ Quitar rol (eliminación directa, no lógica normalmente)
        void Delete(UsuarioRolEntity usuarioRol);
    }
}