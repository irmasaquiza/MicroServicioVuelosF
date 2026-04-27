using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IAsientoRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<AsientoEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<AsientoEntity> GetByIdAsync(int id);

        // 🔍 Obtener asientos por vuelo
        Task<IEnumerable<AsientoEntity>> GetByVueloAsync(int idVuelo);

        // 🔍 Buscar por número de asiento (ej: 12A)
        Task<AsientoEntity> GetByNumeroAsync(int idVuelo, string numeroAsiento);

        // ➕ Crear
        Task AddAsync(AsientoEntity asiento);

        // ✏️ Actualizar
        void Update(AsientoEntity asiento);

        // ❌ Eliminación lógica
        void Delete(AsientoEntity asiento);

        Task AddRangeAsync(IEnumerable<AsientoEntity> entities);
    }
}