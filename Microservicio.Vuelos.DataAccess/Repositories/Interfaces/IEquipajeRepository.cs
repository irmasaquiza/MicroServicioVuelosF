using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IEquipajeRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<EquipajeEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<EquipajeEntity> GetByIdAsync(int id);

        // 🔍 Obtener equipaje por boleto
        Task<IEnumerable<EquipajeEntity>> GetByBoletoAsync(int idBoleto);

        // 🔍 Obtener equipaje por pasajero
        Task<IEnumerable<EquipajeEntity>> GetByPasajeroAsync(int idPasajero);

        // ➕ Crear
        Task AddAsync(EquipajeEntity equipaje);

        // ✏️ Actualizar
        void Update(EquipajeEntity equipaje);

        // ❌ Eliminación lógica
        void Delete(EquipajeEntity equipaje);
    }
}
