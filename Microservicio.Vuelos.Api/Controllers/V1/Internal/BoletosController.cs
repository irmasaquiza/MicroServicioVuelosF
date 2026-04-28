using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;
using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/boletos")]
    [Authorize]
    public class BoletosController : ControllerBase
    {
        private readonly IBoletoService _boletoService;
        private readonly IEquipajeService _equipajeService;

        public BoletosController(
            IBoletoService boletoService,
            IEquipajeService equipajeService)
        {
            _boletoService = boletoService;
            _equipajeService = equipajeService;
        }

        // ============================================================
        // GET: api/v1/boletos
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetAll([FromQuery] BoletoFiltroRequest filtro)
        {
            try
            {
                var result = await _boletoService.FiltrarAsync(filtro);
                return Ok(ApiResponse<IEnumerable<BoletoResponse>>.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // 🔥 MIS BOLETOS (CLIENTE)
        // ============================================================
        [HttpGet("mis-boletos")]
        public async Task<IActionResult> GetMisBoletos()
        {
            try
            {
                var idClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value
                           ?? User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value;

                if (string.IsNullOrEmpty(idClaim))
                    throw new Exception("No se pudo obtener el idUsuario del token");

                var idUsuario = int.Parse(idClaim);

                var result = await _boletoService.GetByUsuarioAsync(idUsuario);

                return Ok(ApiResponse<IEnumerable<BoletoResponse>>.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message
                });
            }
        }


        // ============================================================
        // GET: api/v1/boletos/{id}
        // ============================================================
        [HttpGet("{id_boleto:int}")]
        public async Task<IActionResult> GetById(int id_boleto)
        {
            try
            {
                var result = await _boletoService.GetByIdAsync(id_boleto);
                return Ok(ApiResponse<BoletoResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    tipo = "NOT_FOUND",
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // POST: api/v1/boletos
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> Crear([FromBody] CrearBoletoRequest request)
        {
            try
            {
                var result = await _boletoService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_boleto = result.IdBoleto },
                    ApiResponse<BoletoResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    tipo = "VALIDATION_ERROR",
                    mensaje = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    success = false,
                    tipo = "BUSINESS_ERROR",
                    mensaje = ex.Message,
                    codigo = ex.CodigoError
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // GET: api/v1/boletos/{id}/equipaje
        // ============================================================
        [HttpGet("{id_boleto:int}/equipaje")]
        public async Task<IActionResult> GetEquipaje(int id_boleto)
        {
            try
            {
                var result = await _equipajeService.GetByBoletoAsync(id_boleto);

                return Ok(ApiResponse<IEnumerable<EquipajeResponse>>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    tipo = "NOT_FOUND",
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // POST: api/v1/boletos/{id}/equipaje
        // ============================================================
        [HttpPost("{id_boleto:int}/equipaje")]
        public async Task<IActionResult> CrearEquipaje(
            int id_boleto,
            [FromBody] CrearEquipajeRequest request)
        {
            try
            {
                request.IdBoleto = id_boleto;

                var result = await _equipajeService.CrearAsync(request);

                return CreatedAtAction(nameof(GetEquipaje),
                    new { id_boleto },
                    ApiResponse<EquipajeResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    tipo = "VALIDATION_ERROR",
                    mensaje = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    success = false,
                    tipo = "BUSINESS_ERROR",
                    mensaje = ex.Message,
                    codigo = ex.CodigoError
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // PATCH: estado equipaje
        // ============================================================
        [HttpPatch("{id_boleto:int}/equipaje/{id_equipaje:int}/estado")]
        public async Task<IActionResult> ActualizarEstadoEquipaje(
            int id_boleto,
            int id_equipaje,
            [FromBody] ActualizarEquipajeRequest request)
        {
            try
            {
                await _equipajeService.CambiarEstadoAsync(
                    id_equipaje, request.EstadoEquipaje);

                return NoContent();
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    tipo = "VALIDATION_ERROR",
                    mensaje = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    success = false,
                    tipo = "BUSINESS_ERROR",
                    mensaje = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    success = false,
                    tipo = "ERROR_INTERNO",
                    mensaje = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}