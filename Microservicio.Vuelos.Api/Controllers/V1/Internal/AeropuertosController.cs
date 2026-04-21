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
        // GET ALL
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] AeropuertoFiltroRequest filtro)
        {
            try
            {
                var result = await _aeropuertoService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<AeropuertoResponse>>.Ok(result));
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
        // GET BY ID
        // ============================================================
        [HttpGet("{id_aeropuerto:int}")]
        public async Task<IActionResult> GetById(int id_aeropuerto)
        {
            try
            {
                var result = await _aeropuertoService.GetByIdAsync(id_aeropuerto);

                return Ok(ApiResponse<AeropuertoResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    error = ex.Message
                });
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
        // CREATE
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
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
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return Conflict(new
                {
                    success = false,
                    error = ex.Message
                });
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
        // UPDATE
        // ============================================================
        [HttpPut("{id_aeropuerto:int}")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
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
                return BadRequest(new
                {
                    success = false,
                    error = ex.Message
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    error = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    success = false,
                    error = ex.Message
                });
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
        // DELETE
        // ============================================================
        [HttpDelete("{id_aeropuerto:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar(int id_aeropuerto)
        {
            try
            {
                await _aeropuertoService.EliminarAsync(id_aeropuerto);

                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    error = ex.Message
                });
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
    }
}