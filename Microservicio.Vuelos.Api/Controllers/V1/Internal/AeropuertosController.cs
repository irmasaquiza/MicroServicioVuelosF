using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/aeropuertos")]
    [Authorize]
    public class AeropuertosController : ControllerBase
    {
        private readonly IAeropuertoService _aeropuertoService;

        public AeropuertosController(IAeropuertoService aeropuertoService)
        {
            _aeropuertoService = aeropuertoService;
        }

        // ============================================================
        // GET: api/v1/aeropuertos
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AeropuertoResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] AeropuertoFiltroRequest filtro)
        {
            try
            {
                var result = await _aeropuertoService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<AeropuertoResponse>>.Ok(result));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // GET: api/v1/aeropuertos/{id}
        // ============================================================
        [HttpGet("{id_aeropuerto:int}")]
        [ProducesResponseType(typeof(ApiResponse<AeropuertoResponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public async Task<IActionResult> GetById(int id_aeropuerto)
        {
            try
            {
                var result = await _aeropuertoService.GetByIdAsync(id_aeropuerto);

                return Ok(ApiResponse<AeropuertoResponse>.Ok(result));
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
        // POST: api/v1/aeropuertos
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<AeropuertoResponse>), 201)]
        public async Task<IActionResult> Crear([FromBody] CrearAeropuertoRequest request)
        {
            try
            {
                var result = await _aeropuertoService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_aeropuerto = result.IdAeropuerto },
                    ApiResponse<AeropuertoResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (BusinessException ex)
            {
                return Conflict(ApiErrorResponse.FromBusiness(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // PUT: api/v1/aeropuertos/{id}
        // ============================================================
        [HttpPut("{id_aeropuerto:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<AeropuertoResponse>), 200)]
        public async Task<IActionResult> Actualizar(
            int id_aeropuerto,
            [FromBody] ActualizarAeropuertoRequest request)
        {
            try
            {
                var result = await _aeropuertoService.ActualizarAsync(id_aeropuerto, request);

                return Ok(ApiResponse<AeropuertoResponse>.Ok(result));
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
        // DELETE: api/v1/aeropuertos/{id}
        // ============================================================
        [HttpDelete("{id_aeropuerto:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Eliminar(int id_aeropuerto)
        {
            try
            {
                await _aeropuertoService.EliminarAsync(id_aeropuerto);

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