using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IVueloRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<VueloEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<VueloEntity> GetByIdAsync(int id);

        // 🔍 Buscar por código de vuelo (AV1234)
        Task<VueloEntity> GetByCodigoAsync(string codigoVuelo);

        // 🔍 Buscar vuelos por ruta
        Task<IEnumerable<VueloEntity>> GetByRutaAsync(int idOrigen, int idDestino);

        // 🔍 Buscar vuelos por fecha
        Task<IEnumerable<VueloEntity>> GetByFechaAsync(DateTime fecha);

        // 🔍 Buscar vuelos por estado
        Task<IEnumerable<VueloEntity>> GetByEstadoAsync(string estadoVuelo);

        // 🔍 Buscar vuelos disponibles (con capacidad)
        Task<IEnumerable<VueloEntity>> GetDisponiblesAsync();

        // ➕ Crear
        Task AddAsync(VueloEntity vuelo);

        // ✏️ Actualizar
        void Update(VueloEntity vuelo);

        // ❌ Eliminación lógica
        void Delete(VueloEntity vuelo);
    }
}