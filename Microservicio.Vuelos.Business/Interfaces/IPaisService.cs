using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Pais;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IPaisService
    {
        // ============================================================
        // 🔥 CREAR
        // ============================================================
        Task<PaisResponse> CrearAsync(CrearPaisRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<PaisResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<PaisResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<PaisResponse>> FiltrarAsync(PaisFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<PaisResponse> ActualizarAsync(int id, ActualizarPaisRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}