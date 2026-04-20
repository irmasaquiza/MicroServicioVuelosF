using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes/{id_cliente:int}/metodos-pago")]
    [Authorize]
    public class MetodosPagoController : ControllerBase
    {
        private readonly IMetodoPagoService _metodoPagoService;

        public MetodosPagoController(IMetodoPagoService metodoPagoService)
        {
            _metodoPagoService = metodoPagoService;
        }

        // ============================================================
        // GET: api/v1/clientes/{id_cliente}/metodos-pago
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<MetodoPagoResponse>>), 200)]
        public async Task<IActionResult> GetByCliente(int id_cliente)
        {
            try
            {
                var result = await _metodoPagoService.GetByClienteAsync(id_cliente);

                return Ok(ApiResponse<IEnumerable<MetodoPagoResponse>>.Ok(result));
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
        // POST: api/v1/clientes/{id_cliente}/metodos-pago
        // ============================================================
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<MetodoPagoResponse>), 201)]
        public async Task<IActionResult> Crear(
            int id_cliente,
            [FromBody] CrearMetodoPagoRequest request)
        {
            try
            {
                request.IdCliente = id_cliente;

                var result = await _metodoPagoService.CrearAsync(request);

                return CreatedAtAction(nameof(GetByCliente),
                    new { id_cliente },
                    ApiResponse<MetodoPagoResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiErrorResponse.FromValidation(ex));
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

        // ============================================================
        // DELETE: api/v1/clientes/{id_cliente}/metodos-pago/{id_metodo}
        // ============================================================
        [HttpDelete("{id_metodo:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Eliminar(int id_cliente, int id_metodo)
        {
            try
            {
                await _metodoPagoService.EliminarAsync(id_metodo);

                return NoContent();
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
    }
}