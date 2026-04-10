using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IPasajeroRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<PasajeroEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<PasajeroEntity> GetByIdAsync(int id);

        // 🔍 Obtener pasajeros por cliente (cuando aplica)
        Task<IEnumerable<PasajeroEntity>> GetByClienteAsync(int idCliente);

        // 🔍 Buscar por documento
        Task<PasajeroEntity> GetByDocumentoAsync(string numeroDocumento);

        // 🔍 Buscar por nombre
        Task<IEnumerable<PasajeroEntity>> GetByNombreAsync(string nombres, string apellidos);

        // ➕ Crear
        Task AddAsync(PasajeroEntity pasajero);

        // ✏️ Actualizar
        void Update(PasajeroEntity pasajero);

        // ❌ Eliminación lógica
        void Delete(PasajeroEntity pasajero);
    }
}