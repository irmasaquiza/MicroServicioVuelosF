using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IUsuarioRolDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<UsuarioRolDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<UsuarioRolDataModel> GetByIdAsync(int id);

        // 🔍 Obtener roles de un usuario (CLAVE 💀)
        Task<IEnumerable<UsuarioRolDataModel>> GetByUsuarioAsync(int idUsuario);

        // 🔍 Obtener usuarios por rol
        Task<IEnumerable<UsuarioRolDataModel>> GetByRolAsync(int idRol);

        // 🔍 Obtener asignaciones activas
        Task<IEnumerable<UsuarioRolDataModel>> GetActivosAsync();

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<UsuarioRolDataModel>> GetPagedAsync(UsuarioRolFiltroDataModel filtro);

        // ➕ Crear asignación
        Task<UsuarioRolDataModel> CreateAsync(UsuarioRolDataModel model);

        // ✏️ Actualizar (estado)
        Task<bool> UpdateAsync(UsuarioRolDataModel model);

        // ❌ Eliminar asignación (lógico)
        Task<bool> DeleteAsync(int id);
    }
}