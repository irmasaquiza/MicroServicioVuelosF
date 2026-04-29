using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.DTOs.Booking.Reserva;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/reservas")]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearReservaBookingRequest request)
        {
            var result = await _reservaService.CrearBookingAsync(request);

            return Ok(new
            {
                data = result
            });
        }

        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstado(int id, [FromBody] ActualizarEstadoReservaBookingRequest request)
        {
            var result = await _reservaService.ActualizarEstadoBookingAsync(id, request);

            return Ok(new
            {
                success = result
            });
        }
    }

}