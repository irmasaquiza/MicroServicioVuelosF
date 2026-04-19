using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IUsuarioRolService
    {
        // ============================================================
        // 🔥 ASIGNAR ROL A USUARIO
        // POST /usuarios/{id}/roles
        // ============================================================
        Task<UsuarioRolResponse> CrearAsync(int idUsuario, CrearUsuarioRolRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<UsuarioRolResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR ROLES DE UN USUARIO
        // ============================================================
        Task<IEnumerable<UsuarioRolResponse>> GetByUsuarioAsync(int idUsuario);

        // ============================================================
        // 🔥 LISTAR USUARIOS POR ROL
        // ============================================================
        Task<IEnumerable<UsuarioRolResponse>> GetByRolAsync(int idRol);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<UsuarioRolResponse>> FiltrarAsync(UsuarioRolFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR (estado / activo)
        // ============================================================
        Task<UsuarioRolResponse> ActualizarAsync(int id, ActualizarUsuarioRolRequest request);

        // ============================================================
        // 🔥 DESASIGNAR (ELIMINAR LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}