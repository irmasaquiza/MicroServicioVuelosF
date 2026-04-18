using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IEscalaRepository
    {
        // 🔍 Obtener todas
        Task<IEnumerable<EscalaEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<EscalaEntity> GetByIdAsync(int id);

        // 🔍 Obtener escalas por vuelo
        Task<IEnumerable<EscalaEntity>> GetByVueloAsync(int idVuelo);

        // 🔍 Obtener escala por orden dentro de un vuelo
        Task<EscalaEntity> GetByVueloYOrdenAsync(int idVuelo, int orden);

        // ➕ Crear
        Task AddAsync(EscalaEntity escala);

        // ✏️ Actualizar
        void Update(EscalaEntity escala);

        // ❌ Eliminación lógica
        void Delete(EscalaEntity escala);
    }
}