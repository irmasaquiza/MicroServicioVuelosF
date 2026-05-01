using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Escala;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vuelos/{id_vuelo:int}/escalas")]
    [Authorize]
    public class EscalasController : ControllerBase
    {
        private readonly IEscalaService _escalaService;

        public EscalasController(IEscalaService escalaService)
        {
            _escalaService = escalaService;
        }

        // ============================================================
        // GET: api/v1/vuelos/{id_vuelo}/escalas
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EscalaResponse>>), 200)]
        public async Task<IActionResult> GetByVuelo(int id_vuelo)
        {
            try
            {
                var result = await _escalaService.GetByVueloAsync(id_vuelo);

                return Ok(ApiResponse<IEnumerable<EscalaResponse>>.Ok(result));
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
        // GET: api/v1/vuelos/{id_vuelo}/escalas/{id_escala}
        // ============================================================
        [HttpGet("{id_escala:int}")]
        [ProducesResponseType(typeof(ApiResponse<EscalaResponse>), 200)]
        public async Task<IActionResult> GetById(int id_vuelo, int id_escala)
        {
            try
            {
                var result = await _escalaService.GetByIdAsync(id_escala);

                return Ok(ApiResponse<EscalaResponse>.Ok(result));
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
        // POST: api/v1/vuelos/{id_vuelo}/escalas
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<EscalaResponse>), 201)]
        public async Task<IActionResult> Crear(int id_vuelo, [FromBody] CrearEscalaRequest request)
        {
            try
            {
                var result = await _escalaService.CrearAsync(id_vuelo, request);

                return CreatedAtAction(nameof(GetById),
                    new { id_vuelo, id_escala = result.IdEscala },
                    ApiResponse<EscalaResponse>.Ok(result));
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
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // DELETE: api/v1/vuelos/{id_vuelo}/escalas/{id_escala}
        // ============================================================
        [HttpDelete("{id_escala:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Eliminar(int id_vuelo, int id_escala)
        {
            try
            {
                await _escalaService.EliminarAsync(id_escala);

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