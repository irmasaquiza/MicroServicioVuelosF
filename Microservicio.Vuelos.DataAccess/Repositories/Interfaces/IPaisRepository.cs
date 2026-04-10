using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IPaisRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<PaisEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<PaisEntity> GetByIdAsync(int id);

        // 🔍 Buscar por código ISO2 (EC, US, etc.)
        Task<PaisEntity> GetByCodigoIso2Async(string codigoIso2);

        // 🔍 Buscar por código ISO3 (ECU, USA, etc.)
        Task<PaisEntity> GetByCodigoIso3Async(string codigoIso3);

        // 🔍 Buscar por nombre
        Task<IEnumerable<PaisEntity>> GetByNombreAsync(string nombre);

        // ➕ Crear
        Task AddAsync(PaisEntity pais);

        // ✏️ Actualizar
        void Update(PaisEntity pais);

        // ❌ Eliminación lógica
        void Delete(PaisEntity pais);
    }
}