using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<BoletoResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] BoletoFiltroRequest filtro)
        {
            try
            {
                var result = await _boletoService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<BoletoResponse>>.Ok(result));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // GET: api/v1/boletos/{id}
        // ============================================================
        [HttpGet("{id_boleto:int}")]
        [ProducesResponseType(typeof(ApiResponse<BoletoResponse>), 200)]
        public async Task<IActionResult> GetById(int id_boleto)
        {
            try
            {
                var result = await _boletoService.GetByIdAsync(id_boleto);

                return Ok(ApiResponse<BoletoResponse>.Ok(result));
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
        // POST: api/v1/boletos
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(typeof(ApiResponse<BoletoResponse>), 201)]
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
                return BadRequest(ApiErrorResponse.FromValidation(ex));
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
        // GET: api/v1/boletos/{id}/equipaje
        // ============================================================
        [HttpGet("{id_boleto:int}/equipaje")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<EquipajeResponse>>), 200)]
        public async Task<IActionResult> GetEquipaje(int id_boleto)
        {
            try
            {
                var result = await _equipajeService.GetByBoletoAsync(id_boleto);

                return Ok(ApiResponse<IEnumerable<EquipajeResponse>>.Ok(result));
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
        // POST: api/v1/boletos/{id}/equipaje
        // ============================================================
        [HttpPost("{id_boleto:int}/equipaje")]
        [ProducesResponseType(typeof(ApiResponse<EquipajeResponse>), 201)]
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
        // PATCH: api/v1/boletos/{id}/equipaje/{id_equipaje}/estado
        // ============================================================
        [HttpPatch("{id_boleto:int}/equipaje/{id_equipaje:int}/estado")]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        [ProducesResponseType(204)]
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