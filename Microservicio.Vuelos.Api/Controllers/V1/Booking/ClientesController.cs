using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.DTOs.Booking.Cliente;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // 🔥 POST crear cliente
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearClienteBookingRequest request)
        {
            var result = await _clienteService.CrearBookingAsync(request);

            return Ok(new
            {
                data = result
            });
        }

        // 🔥 GET buscar cliente
        [HttpGet]
        public async Task<IActionResult> Buscar(
            [FromQuery] string numero_identificacion,
            [FromQuery] string correo)
        {
            var result = await _clienteService.BuscarBookingAsync(numero_identificacion, correo);

            return Ok(new
            {
                data = result
            });
        }
    }
}