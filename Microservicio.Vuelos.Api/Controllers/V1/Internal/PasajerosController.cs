using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Pasajero;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/pasajeros")]
    [Authorize]
    public class PasajerosController : ControllerBase
    {
        private readonly IPasajeroService _pasajeroService;

        public PasajerosController(IPasajeroService pasajeroService)
        {
            _pasajeroService = pasajeroService;
        }

        // ============================================================
        // GET: api/v1/pasajeros
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetAll([FromQuery] PasajeroFiltroRequest filtro)
        {
            try
            {
                var result = await _pasajeroService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<PasajeroResponse>>.Ok(result));
            }
            catch (Exception)
            {
                throw; // 🔥 AHORA EL MIDDLEWARE MUESTRA EL ERROR REAL
            }
        }

        // ============================================================
        // GET: api/v1/pasajeros/{id}
        // ============================================================
        [HttpGet("{id_pasajero:int}")]
        public async Task<IActionResult> GetById(int id_pasajero)
        {
            try
            {
                var result = await _pasajeroService.GetByIdAsync(id_pasajero);

                return Ok(ApiResponse<PasajeroResponse>.Ok(result));
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

        // ============================================================
        // POST: api/v1/pasajeros
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearPasajeroRequest request)
        {
            try
            {
                var result = await _pasajeroService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_pasajero = result.IdPasajero },
                    ApiResponse<PasajeroResponse>.Ok(result));
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
                throw;
            }
        }

        // ============================================================
        // PUT: api/v1/pasajeros/{id}
        // ============================================================
        [HttpPut("{id_pasajero:int}")]
        public async Task<IActionResult> Actualizar(
            int id_pasajero,
            [FromBody] ActualizarPasajeroRequest request)
        {
            try
            {
                var result = await _pasajeroService.ActualizarAsync(id_pasajero, request);

                return Ok(ApiResponse<PasajeroResponse>.Ok(result));
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
                throw;
            }
        }
    }
}