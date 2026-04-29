using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/vuelos")]
    public class AsientosController : ControllerBase
    {
        private readonly IAsientoService _asientoService;

        public AsientosController(IAsientoService asientoService)
        {
            _asientoService = asientoService;
        }

        // 🔥 GET asientos por vuelo
        [HttpGet("{idVuelo}/asientos")]
        public async Task<IActionResult> GetAsientos(int idVuelo, [FromQuery] bool? disponible, [FromQuery] string clase)
        {
            var result = await _asientoService.GetAsientosBookingAsync(idVuelo, disponible, clase);

            return Ok(new
            {
                data = result
            });
        }
    }
}