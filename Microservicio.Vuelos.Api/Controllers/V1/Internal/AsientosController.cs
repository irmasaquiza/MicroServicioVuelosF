using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/vuelos/{id_vuelo:int}/asientos")]
    [Authorize]
    public class AsientosController : ControllerBase
    {
        private readonly IAsientoService _asientoService;

        public AsientosController(IAsientoService asientoService)
        {
            _asientoService = asientoService;
        }

        // ============================================================
        // GET: api/v1/vuelos/{id_vuelo}/asientos
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AsientoResponse>>), 200)]
        public async Task<IActionResult> GetByVuelo(
            int id_vuelo,
            [FromQuery] AsientoFiltroRequest filtro)
        {
            try
            {
                var result = await _asientoService.GetByVueloAsync(id_vuelo);

                return Ok(ApiResponse<IEnumerable<AsientoResponse>>.Ok(result));
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
        // GET: api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
        // ============================================================
        [HttpGet("{id_asiento:int}")]
        [ProducesResponseType(typeof(ApiResponse<AsientoResponse>), 200)]
        public async Task<IActionResult> GetById(int id_vuelo, int id_asiento)
        {
            try
            {
                var result = await _asientoService.GetByIdAsync(id_asiento);

                return Ok(ApiResponse<AsientoResponse>.Ok(result));
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
        // POST: api/v1/vuelos/{id_vuelo}/asientos
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<AsientoResponse>), 201)]
        public async Task<IActionResult> Crear(int id_vuelo, [FromBody] CrearAsientoRequest request)
        {
            try
            {
                var result = await _asientoService.CrearAsync(id_vuelo, request);

                return CreatedAtAction(nameof(GetById),
                    new { id_vuelo, id_asiento = result.IdAsiento },
                    ApiResponse<AsientoResponse>.Ok(result));
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
        // PATCH: api/v1/vuelos/{id_vuelo}/asientos/{id_asiento}
        // ============================================================
        [HttpPatch("{id_asiento:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<AsientoResponse>), 200)]
        public async Task<IActionResult> Actualizar(
            int id_vuelo,
            int id_asiento,
            [FromBody] ActualizarAsientoRequest request)
        {
            try
            {
                var result = await _asientoService.ActualizarAsync(id_asiento, request);

                return Ok(ApiResponse<AsientoResponse>.Ok(result));
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
    }
}