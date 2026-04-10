using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IRolRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<RolEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<RolEntity> GetByIdAsync(int id);

        // 🔍 Buscar por nombre (ADMIN, CLIENTE, AGENTE)
        Task<RolEntity> GetByNombreAsync(string nombreRol);

        // ➕ Crear
        Task AddAsync(RolEntity rol);

        // ✏️ Actualizar
        void Update(RolEntity rol);

        // ❌ Eliminación lógica
        void Delete(RolEntity rol);
    }
}
