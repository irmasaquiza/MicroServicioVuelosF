using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface ICiudadRepository
    {
        // 🔍 Obtener todas
        Task<IEnumerable<CiudadEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<CiudadEntity> GetByIdAsync(int id);

        // 🔍 Obtener por país
        Task<IEnumerable<CiudadEntity>> GetByPaisAsync(int idPais);

        // 🔍 Buscar por nombre
        Task<IEnumerable<CiudadEntity>> GetByNombreAsync(string nombre);

        // ➕ Crear
        Task AddAsync(CiudadEntity ciudad);

        // ✏️ Actualizar
        void Update(CiudadEntity ciudad);

        // ❌ Eliminación lógica
        void Delete(CiudadEntity ciudad);
    }
}