using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auditoria")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class AuditoriaController : ControllerBase
    {
        private readonly IAuditoriaLogService _auditoriaService;

        public AuditoriaController(IAuditoriaLogService auditoriaService)
        {
            _auditoriaService = auditoriaService;
        }

        // ============================================================
        // GET: api/v1/auditoria
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<AuditoriaLogResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] AuditoriaLogFiltroRequest filtro)
        {
            try
            {
                var result = await _auditoriaService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<AuditoriaLogResponse>>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // GET: api/v1/auditoria/{id}
        // ============================================================
        [HttpGet("{id_auditoria:long}")]
        [ProducesResponseType(typeof(ApiResponse<AuditoriaLogResponse>), 200)]
        public async Task<IActionResult> GetById(long id_auditoria)
        {
            try
            {
                var result = await _auditoriaService.GetByIdAsync(id_auditoria);

                return Ok(ApiResponse<AuditoriaLogResponse>.Ok(result));
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