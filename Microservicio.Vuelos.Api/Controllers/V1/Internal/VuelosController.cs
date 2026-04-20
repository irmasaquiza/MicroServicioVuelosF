using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Vuelo;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vuelos")]
    [Authorize]
    public class VuelosController : ControllerBase
    {
        private readonly IVueloService _vueloService;

        public VuelosController(IVueloService vueloService)
        {
            _vueloService = vueloService;
        }

        // ============================================================
        // GET: api/v1/vuelos
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<VueloResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] VueloFiltroRequest filtro)
        {
            var result = await _vueloService.FiltrarAsync(filtro);

            return Ok(ApiResponse<IEnumerable<VueloResponse>>.Ok(result));
        }

        // ============================================================
        // GET: api/v1/vuelos/{id}
        // ============================================================
        [HttpGet("{id_vuelo:int}")]
        [ProducesResponseType(typeof(ApiResponse<VueloDetalleResponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public async Task<IActionResult> GetById(int id_vuelo)
        {
            try
            {
                var result = await _vueloService.GetDetalleAsync(id_vuelo);

                return Ok(ApiResponse<VueloDetalleResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
        }

        // ============================================================
        // POST: api/v1/vuelos
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<VueloResponse>), 201)]
        public async Task<IActionResult> Crear([FromBody] CrearVueloRequest request)
        {
            try
            {
                var result = await _vueloService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_vuelo = result.IdVuelo },
                    ApiResponse<VueloResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (BusinessException ex)
            {
                return Conflict(ApiErrorResponse.FromBusiness(ex));
            }
        }

        // ============================================================
        // PUT: api/v1/vuelos/{id}
        // ============================================================
        [HttpPut("{id_vuelo:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<VueloResponse>), 200)]
        public async Task<IActionResult> Actualizar(
            int id_vuelo,
            [FromBody] ActualizarVueloRequest request)
        {
            try
            {
                var result = await _vueloService.ActualizarAsync(id_vuelo, request);

                return Ok(ApiResponse<VueloResponse>.Ok(result));
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
        }

        // ============================================================
        // PATCH: api/v1/vuelos/{id}/estado
        // ============================================================
        [HttpPatch("{id_vuelo:int}/estado")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> CambiarEstado(
            int id_vuelo,
            [FromBody] ActualizarEstadoVueloRequest request)
        {
            try
            {
                await _vueloService.CambiarEstadoAsync(id_vuelo, request);

                return NoContent();
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
        }

        // ============================================================
        // DELETE: api/v1/vuelos/{id}
        // ============================================================
        [HttpDelete("{id_vuelo:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Eliminar(int id_vuelo)
        {
            try
            {
                await _vueloService.EliminarAsync(id_vuelo);

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
        }
    }
}