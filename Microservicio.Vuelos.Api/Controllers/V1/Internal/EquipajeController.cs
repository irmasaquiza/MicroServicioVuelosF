using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    /// <summary>
    /// Equipaje — acceso directo (admin/debug)
    /// Endpoints principales viven en BoletosController
    /// </summary>
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/equipaje")]
    [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
    public class EquipajeController : ControllerBase
    {
        private readonly IEquipajeService _equipajeService;

        public EquipajeController(IEquipajeService equipajeService)
        {
            _equipajeService = equipajeService;
        }

        // ============================================================
        // GET: api/v1/equipaje/{id}
        // ============================================================
        [HttpGet("{id_equipaje:int}")]
        [ProducesResponseType(typeof(ApiResponse<EquipajeResponse>), 200)]
        public async Task<IActionResult> GetById(int id_equipaje)
        {
            try
            {
                var result = await _equipajeService.GetByIdAsync(id_equipaje);

                return Ok(ApiResponse<EquipajeResponse>.Ok(result));
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