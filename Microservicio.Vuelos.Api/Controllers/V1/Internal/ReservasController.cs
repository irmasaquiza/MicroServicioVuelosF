using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;
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
        [HttpGet("mis-reservas")]
        [Authorize(Roles = "CLIENTE")]
        public async Task<IActionResult> GetMisReservas()
        {
            try
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(idClaim))
                    return Unauthorized();

                var idUsuario = int.Parse(idClaim);

                var result = await _reservaService.GetByUsuarioAsync(idUsuario);

                return Ok(ApiResponse<IEnumerable<ReservaResponse>>.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message
                });
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
                // 🔥 1. obtener idUsuario del JWT
                var idUsuario = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Sub).Value
                );

                // 🔥 2. asignarlo al request
                request.IdUsuario = idUsuario;

                // 🔥 3. llamar al service
                var result = await _reservaService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_reserva = result.IdReserva },
                    ApiResponse<ReservaResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    mensaje = ex.Message,
                    errores = ex.Errores
                });
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



        [HttpPost("{id_reserva:int}/confirmar")]
        public async Task<IActionResult> Confirmar(int id_reserva)
        {
            try
            {
                // 🔥 sacar idUsuario del JWT
                var idUsuario = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Sub).Value
                );

                await _reservaService.ConfirmarAsync(id_reserva, idUsuario);

                return NoContent();
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(ApiErrorResponse.FromBusiness(ex));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (Exception)
            {
                throw;
            }
        }

        [HttpPost("{id_reserva:int}/cancelar")]
        public async Task<IActionResult> Cancelar(int id_reserva)
        {
            try
            {
                var idUsuario = int.Parse(
                    User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                    ?? User.FindFirst(JwtRegisteredClaimNames.Sub).Value
                );

                await _reservaService.CancelarClienteAsync(id_reserva, idUsuario);

                return NoContent();
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(ApiErrorResponse.FromBusiness(ex));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}