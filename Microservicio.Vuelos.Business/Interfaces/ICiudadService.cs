using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Ciudad;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface ICiudadService
    {
        // ============================================================
        // 🔥 CREAR
        // ============================================================
        Task<CiudadResponse> CrearAsync(CrearCiudadRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<CiudadResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR TODAS
        // ============================================================
        Task<IEnumerable<CiudadResponse>> GetAllAsync();

        // ============================================================
        // 🔥 LISTAR POR PAÍS
        // ============================================================
        Task<IEnumerable<CiudadResponse>> GetByPaisAsync(int idPais);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<CiudadResponse>> FiltrarAsync(CiudadFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<CiudadResponse> ActualizarAsync(int id, ActualizarCiudadRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}