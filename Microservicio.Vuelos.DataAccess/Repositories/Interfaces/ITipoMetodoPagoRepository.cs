using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface ITipoMetodoPagoRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<TipoMetodoPagoEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<TipoMetodoPagoEntity?> GetByIdAsync(int id);

        // 🔍 Buscar por nombre exacto
        Task<TipoMetodoPagoEntity?> GetByNombreExactoAsync(string nombre);

        // 🔍 Buscar por nombre parcial
        Task<IEnumerable<TipoMetodoPagoEntity>> GetByNombreAsync(string nombre);

        // ➕ Crear
        Task AddAsync(TipoMetodoPagoEntity tipoMetodo);

        // ✏️ Actualizar
        void Update(TipoMetodoPagoEntity tipoMetodo);

        // ❌ Eliminación lógica
        void Delete(TipoMetodoPagoEntity tipoMetodo);
    }
}