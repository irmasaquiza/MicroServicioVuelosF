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
        // GET: api/v1/facturas
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<FacturaResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] FacturaFiltroRequest filtro)
        {
            try
            {
                var result = await _facturaService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<FacturaResponse>>.Ok(result));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // GET: api/v1/facturas/{id}
        // ============================================================
        [HttpGet("{id_factura:int}")]
        [ProducesResponseType(typeof(ApiResponse<FacturaResponse>), 200)]
        public async Task<IActionResult> GetById(int id_factura)
        {
            try
            {
                var result = await _facturaService.GetByIdAsync(id_factura);

                return Ok(ApiResponse<FacturaResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // POST: api/v1/facturas
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<FacturaResponse>), 201)]
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
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(ApiErrorResponse.FromBusiness(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // PATCH: api/v1/facturas/{id}/anular
        // ============================================================
        [HttpPatch("{id_factura:int}/anular")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Anular(int id_factura)
        {
            try
            {
                await _facturaService.CambiarEstadoAsync(id_factura, "INA");

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(ApiErrorResponse.FromBusiness(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }
    }
}