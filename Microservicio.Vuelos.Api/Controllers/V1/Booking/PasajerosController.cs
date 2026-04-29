using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.DTOs.Booking.Pasajero;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/pasajeros")]
    public class PasajerosController : ControllerBase
    {
        private readonly IPasajeroService _pasajeroService;

        public PasajerosController(IPasajeroService pasajeroService)
        {
            _pasajeroService = pasajeroService;
        }

        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPasajeroBookingRequest request)
        {
            var result = await _pasajeroService.CrearBookingAsync(request);

            return Ok(new
            {
                data = result
            });
        }
    }
}