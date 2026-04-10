using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IClienteRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<ClienteEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<ClienteEntity> GetByIdAsync(int id);

        // 🔍 Buscar por documento (cédula, pasaporte, etc.)
        Task<ClienteEntity> GetByDocumentoAsync(string numeroDocumento);

        // 🔍 Buscar por email
        Task<ClienteEntity> GetByEmailAsync(string email);

        // 🔍 Buscar por nombre (filtro)
        Task<IEnumerable<ClienteEntity>> GetByNombreAsync(string nombre);

        // ➕ Crear
        Task AddAsync(ClienteEntity cliente);

        // ✏️ Actualizar
        void Update(ClienteEntity cliente);

        // ❌ Eliminación lógica
        void Delete(ClienteEntity cliente);
    }
}