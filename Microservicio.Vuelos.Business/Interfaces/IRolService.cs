using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Rol;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IRolService
    {
        // ============================================================
        // 🔥 CREAR ROL
        // ============================================================
        Task<RolResponse> CrearAsync(CrearRolRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<RolResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<RolResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<RolResponse>> FiltrarAsync(RolFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<RolResponse> ActualizarAsync(int id, ActualizarRolRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}