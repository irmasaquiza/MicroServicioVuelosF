using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IUsuarioAppService
    {
        // ============================================================
        // 🔥 CREAR USUARIO
        // ============================================================
        Task<UsuarioAppResponse> CrearAsync(CrearUsuarioAppRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<UsuarioAppResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 OBTENER POR USERNAME
        // ============================================================
        Task<UsuarioAppResponse> GetByUsernameAsync(string username);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<UsuarioAppResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<UsuarioAppResponse>> FiltrarAsync(UsuarioAppFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<UsuarioAppResponse> ActualizarAsync(int id, ActualizarUsuarioAppRequest request);

        // ============================================================
        // 🔥 CAMBIAR ESTADO (ACT / INA)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int id, string estado);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}