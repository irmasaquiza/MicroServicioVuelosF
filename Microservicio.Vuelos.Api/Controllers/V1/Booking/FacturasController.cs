using Microsoft.AspNetCore.Mvc;
using Microservicio.Vuelos.Business.DTOs.Booking.Factura;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Booking
{
    [ApiController]
    [Route("api/v1/booking/facturas")]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturasController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        // 🔥 obtener factura por reserva
        [HttpGet("por-reserva/{idReserva}")]
        public async Task<IActionResult> GetByReserva(int idReserva)
        {
            var result = await _facturaService.GetByReservaBookingAsync(idReserva);

            return Ok(new
            {
                data = result
            });
        }

        // 🔥 pagar factura
        [HttpPost("{idFactura}/pagar")]
        public async Task<IActionResult> Pagar(int idFactura)
        {
            var result = await _facturaService.PagarBookingAsync(idFactura);

            return Ok(new
            {
                success = result
            });
        }
    }
}