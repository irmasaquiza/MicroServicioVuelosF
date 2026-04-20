using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Pais;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/paises")]
    [Authorize]
    public class PaisesController : ControllerBase
    {
        private readonly IPaisService _paisService;

        public PaisesController(IPaisService paisService)
        {
            _paisService = paisService;
        }

        // ============================================================
        // GET: api/v1/paises
        // ============================================================
        [HttpGet]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<PaisResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] PaisFiltroRequest filtro)
        {
            var result = await _paisService.FiltrarAsync(filtro);

            return Ok(ApiResponse<IEnumerable<PaisResponse>>.Ok(result));
        }

        // ============================================================
        // GET: api/v1/paises/{id_pais}
        // ============================================================
        [HttpGet("{id_pais:int}")]
        [ProducesResponseType(typeof(ApiResponse<PaisResponse>), 200)]
        [ProducesResponseType(typeof(ApiErrorResponse), 404)]
        public async Task<IActionResult> GetById(int id_pais)
        {
            var result = await _paisService.GetByIdAsync(id_pais);

            return Ok(ApiResponse<PaisResponse>.Ok(result));
        }

        // ============================================================
        // POST: api/v1/paises
        // ============================================================
        [HttpPost]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ProducesResponseType(typeof(ApiResponse<PaisResponse>), 201)]
        public async Task<IActionResult> Crear([FromBody] CrearPaisRequest request)
        {
            var result = await _paisService.CrearAsync(request);

            return CreatedAtAction(nameof(GetById),
                new { id_pais = result.IdPais },
                ApiResponse<PaisResponse>.Ok(result));
        }

        // ============================================================
        // PUT: api/v1/paises/{id_pais}
        // ============================================================
        [HttpPut("{id_pais:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ProducesResponseType(typeof(ApiResponse<PaisResponse>), 200)]
        public async Task<IActionResult> Actualizar(int id_pais, [FromBody] ActualizarPaisRequest request)
        {
            var result = await _paisService.ActualizarAsync(id_pais, request);

            return Ok(ApiResponse<PaisResponse>.Ok(result));
        }

        // ============================================================
        // DELETE: api/v1/paises/{id_pais}
        // ============================================================
        [HttpDelete("{id_pais:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> Eliminar(int id_pais)
        {
            await _paisService.EliminarAsync(id_pais);

            return NoContent();
        }
    }
}