using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Reserva;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/reservas")]
    [Authorize]
    public class ReservasController : ControllerBase
    {
        private readonly IReservaService _reservaService;

        public ReservasController(IReservaService reservaService)
        {
            _reservaService = reservaService;
        }

        // ============================================================
        // GET: api/v1/reservas
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] ReservaFiltroRequest filtro)
        {
            try
            {
                var result = await _reservaService.FiltrarAsync(filtro);
                return Ok(ApiResponse<IEnumerable<ReservaResponse>>.Ok(result));
            }
            catch (Exception)
            {
                throw; // 🔥 MUESTRA ERROR REAL
            }
        }

        // ============================================================
        // GET: api/v1/reservas/{id}
        // ============================================================
        [HttpGet("{id_reserva:int}")]
        public async Task<IActionResult> GetById(int id_reserva)
        {
            try
            {
                var result = await _reservaService.GetDetalleAsync(id_reserva);
                return Ok(ApiResponse<ReservaDetalleResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (Exception)
            {
                throw; // 🔥 MUESTRA ERROR REAL
            }
        }

        // ============================================================
        // POST: api/v1/reservas
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearReservaRequest request)
        {
            try
            {
                var result = await _reservaService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_reserva = result.IdReserva },
                    ApiResponse<ReservaResponse>.Ok(result));
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
                throw; // 🔥 MUESTRA ERROR REAL
            }
        }

        // ============================================================
        // PATCH: api/v1/reservas/{id}/estado
        // ============================================================
        [HttpPatch("{id_reserva:int}/estado")]
        public async Task<IActionResult> CambiarEstado(
            int id_reserva,
            [FromBody] ActualizarEstadoReservaRequest request)
        {
            try
            {
                await _reservaService.CambiarEstadoAsync(id_reserva, request);
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
            catch (Exception)
            {
                throw; // 🔥 MUESTRA ERROR REAL
            }
        }
    }
}