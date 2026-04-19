using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Cliente;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IClienteService
    {
        // ============================================================
        // 🔥 CREAR
        // ============================================================
        Task<ClienteResponse> CrearAsync(CrearClienteRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<ClienteResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 OBTENER POR IDENTIFICACIÓN
        // (CEDULA / PASAPORTE / RUC)
        // ============================================================
        Task<ClienteResponse> GetByIdentificacionAsync(string numeroIdentificacion);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<ClienteResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<ClienteResponse>> FiltrarAsync(ClienteFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<ClienteResponse> ActualizarAsync(int id, ActualizarClienteRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}