using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IEquipajeService
    {
        // ============================================================
        // 🔥 CREAR EQUIPAJE EN UN BOLETO
        // ============================================================
        Task<EquipajeResponse> CrearAsync(CrearEquipajeRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<EquipajeResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR EQUIPAJES POR BOLETO
        // ============================================================
        Task<IEnumerable<EquipajeResponse>> GetByBoletoAsync(int idBoleto);

        // ============================================================
        // 🔥 CAMBIAR ESTADO (TRACKING)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int idEquipaje, string estado);


        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}