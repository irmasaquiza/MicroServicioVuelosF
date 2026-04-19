using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IAeropuertoService
    {
        // ============================================================
        // 🔥 CREAR
        // ============================================================
        Task<AeropuertoResponse> CrearAsync(CrearAeropuertoRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<AeropuertoResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        Task<IEnumerable<AeropuertoResponse>> GetAllAsync();

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<AeropuertoResponse>> FiltrarAsync(AeropuertoFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<AeropuertoResponse> ActualizarAsync(int id, ActualizarAeropuertoRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}