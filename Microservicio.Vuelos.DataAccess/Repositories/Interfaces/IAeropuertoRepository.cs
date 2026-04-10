using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IAeropuertoRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<AeropuertoEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<AeropuertoEntity> GetByIdAsync(int id);

        // 🔍 Buscar por código (ej: UIO, GYE)
        Task<AeropuertoEntity> GetByCodigoAsync(string codigo);

        // ➕ Crear
        Task AddAsync(AeropuertoEntity aeropuerto);

        // ✏️ Actualizar
        void Update(AeropuertoEntity aeropuerto);

        // ❌ Eliminación lógica
        void Delete(AeropuertoEntity aeropuerto);
    }
}
