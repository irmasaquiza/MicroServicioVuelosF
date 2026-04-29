using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.DTOs.Booking.Vuelo;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/vuelos")]
    public class VuelosController : ControllerBase
    {
        private readonly IVueloService _vueloService;

        public VuelosController(IVueloService vueloService)
        {
            _vueloService = vueloService;
        }

        // ============================================================
        // 🔥 GET /api/v1/booking/vuelos
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> Buscar([FromQuery] VueloBookingFiltroRequest request)
        {
            var result = await _vueloService.BuscarBookingAsync(request);

            return Ok(new
            {
                data = result
            });
        }
    }
}