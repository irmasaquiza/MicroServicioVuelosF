using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.DTOs.Booking.Boleto;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/boletos")]
    public class BoletosController : ControllerBase
    {
        private readonly IBoletoService _boletoService;

        public BoletosController(IBoletoService boletoService)
        {
            _boletoService = boletoService;
        }

        // 🔥 emitir boleto
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearBoletoBookingRequest request)
        {
            var result = await _boletoService.CrearBookingAsync(request);

            return Ok(new
            {
                data = result
            });
        }

        // 🔥 consultar boletos por reserva
        [HttpGet("por-reserva/{idReserva}")]
        public async Task<IActionResult> GetByReserva(int idReserva)
        {
            var result = await _boletoService.GetByReservaBookingAsync(idReserva);

            return Ok(new
            {
                data = result
            });
        }
    }
}