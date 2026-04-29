using Microservicio.Vuelos.Business.DTOs.Booking.Pasajero;
using Microservicio.Vuelos.Business.DTOs.Internal.Pasajero;
using System;
using System.Collections.Generic;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Booking.Pasajero;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IPasajeroService
    {
        // ============================================================
        // 🔥 CREAR PASAJERO
        // ============================================================
        Task<PasajeroResponse> CrearAsync(CrearPasajeroRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<PasajeroResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR POR CLIENTE (opcional)
        // ============================================================
        Task<IEnumerable<PasajeroResponse>> GetByClienteAsync(int idCliente);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<PasajeroResponse>> FiltrarAsync(PasajeroFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<PasajeroResponse> ActualizarAsync(int id, ActualizarPasajeroRequest request);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);



        // booking

        Task<PasajeroBookingResponse> CrearBookingAsync(CrearPasajeroBookingRequest request);
}
}