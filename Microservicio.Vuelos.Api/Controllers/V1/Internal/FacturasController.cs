using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Factura;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/facturas")]
    [Authorize]
    public class FacturasController : ControllerBase
    {
        private readonly IFacturaService _facturaService;

        public FacturasController(IFacturaService facturaService)
        {
            _facturaService = facturaService;
        }

        // ============================================================
        // GET ALL
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetAll([FromQuery] FacturaFiltroRequest filtro)
        {
            try
            {
                var result = await _facturaService.FiltrarAsync(filtro);
                return Ok(ApiResponse<IEnumerable<FacturaResponse>>.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        [HttpGet("{id_factura:int}")]
        public async Task<IActionResult> GetById(int id_factura)
        {
            try
            {
                var result = await _facturaService.GetByIdAsync(id_factura);
                return Ok(ApiResponse<FacturaResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // CREATE
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearFacturaRequest request)
        {
            try
            {
                var result = await _facturaService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_factura = result.IdFactura },
                    ApiResponse<FacturaResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // ANULAR
        // ============================================================
        [HttpPatch("{id_factura:int}/anular")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Anular(int id_factura)
        {
            try
            {
                await _facturaService.CambiarEstadoAsync(id_factura, "INA");

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}