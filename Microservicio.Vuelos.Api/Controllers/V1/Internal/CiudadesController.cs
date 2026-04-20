using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Ciudad;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/ciudades")]
    [Authorize]
    public class CiudadesController : ControllerBase
    {
        private readonly ICiudadService _ciudadService;

        public CiudadesController(ICiudadService ciudadService)
        {
            _ciudadService = ciudadService;
        }

        // ============================================================
        // GET: api/v1/ciudades
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] CiudadFiltroRequest filtro)
        {
            try
            {
                var result = await _ciudadService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<CiudadResponse>>.Ok(result));
            }
            catch (Exception ex)
            {
                // 🔥 AQUÍ ESTÁ LA CLAVE
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }

        // ============================================================
        // GET: api/v1/ciudades/{id}
        // ============================================================
        [HttpGet("{id_ciudad:int}")]
        public async Task<IActionResult> GetById(int id_ciudad)
        {
            try
            {
                var result = await _ciudadService.GetByIdAsync(id_ciudad);

                return Ok(ApiResponse<CiudadResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }

        // ============================================================
        // POST
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearCiudadRequest request)
        {
            try
            {
                var result = await _ciudadService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_ciudad = result.IdCiudad },
                    ApiResponse<CiudadResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (BusinessException ex)
            {
                return Conflict(ApiErrorResponse.FromBusiness(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }

        // ============================================================
        // PUT
        // ============================================================
        [HttpPut("{id_ciudad:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Actualizar(
            int id_ciudad,
            [FromBody] ActualizarCiudadRequest request)
        {
            try
            {
                var result = await _ciudadService.ActualizarAsync(id_ciudad, request);

                return Ok(ApiResponse<CiudadResponse>.Ok(result));
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
                    error = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpDelete("{id_ciudad:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar(int id_ciudad)
        {
            try
            {
                await _ciudadService.EliminarAsync(id_ciudad);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message
                });
            }
        }
    }
}