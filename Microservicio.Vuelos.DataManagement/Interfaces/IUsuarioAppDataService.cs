using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IUsuarioAppDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<UsuarioAppDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<UsuarioAppDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por username (CLAVE 💀)
        Task<UsuarioAppDataModel> GetByUsernameAsync(string username);

        // 🔍 Obtener por correo
        Task<UsuarioAppDataModel> GetByCorreoAsync(string correo);

        // 🔍 Obtener por cliente (cuando aplica)
        Task<IEnumerable<UsuarioAppDataModel>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener activos
        Task<IEnumerable<UsuarioAppDataModel>> GetActivosAsync();

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<UsuarioAppDataModel>> GetPagedAsync(UsuarioAppFiltroDataModel filtro);

        // ➕ Crear
        Task<UsuarioAppDataModel> CreateAsync(UsuarioAppDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(UsuarioAppDataModel model);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);

        Task<UsuarioAppEntity?> GetByCredentialsAsync(string login);

    }
}