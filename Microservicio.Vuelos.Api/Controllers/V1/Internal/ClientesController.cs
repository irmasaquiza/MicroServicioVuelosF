using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Cliente;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/clientes")]
    [Authorize]
    public class ClientesController : ControllerBase
    {
        private readonly IClienteService _clienteService;

        public ClientesController(IClienteService clienteService)
        {
            _clienteService = clienteService;
        }

        // ============================================================
        // GET ALL
        // ============================================================
        [HttpGet]
        [Authorize(Roles = "ADMINISTRADOR,AEROLINEA")]
        public async Task<IActionResult> GetAll([FromQuery] ClienteFiltroRequest filtro)
        {
            try
            {
                var result = await _clienteService.FiltrarAsync(filtro);
                return Ok(ApiResponse<IEnumerable<ClienteResponse>>.Ok(result));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        [HttpGet("{id_cliente:int}")]
        public async Task<IActionResult> GetById(int id_cliente)
        {
            try
            {
                var result = await _clienteService.GetByIdAsync(id_cliente);
                return Ok(ApiResponse<ClienteResponse>.Ok(result));
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
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
        public async Task<IActionResult> Crear([FromBody] CrearClienteRequest request)
        {
            try
            {
                var result = await _clienteService.CrearAsync(request);

                return CreatedAtAction(nameof(GetById),
                    new { id_cliente = result.IdCliente },
                    ApiResponse<ClienteResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return Conflict(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // UPDATE
        // ============================================================
        [HttpPut("{id_cliente:int}")]
        public async Task<IActionResult> Actualizar(
            int id_cliente,
            [FromBody] ActualizarClienteRequest request)
        {
            try
            {
                var result = await _clienteService.ActualizarAsync(id_cliente, request);
                return Ok(ApiResponse<ClienteResponse>.Ok(result));
            }
            catch (ValidationException ex)
            {
                return BadRequest(new
                {
                    error = ex.Message
                });
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return UnprocessableEntity(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }

        // ============================================================
        // DELETE
        // ============================================================
        [HttpDelete("{id_cliente:int}")]
        [Authorize(Roles = "ADMINISTRADOR")]
        public async Task<IActionResult> Eliminar(int id_cliente)
        {
            try
            {
                await _clienteService.EliminarAsync(id_cliente);
                return NoContent();
            }
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    error = ex.Message,
                    detalle = ex.InnerException?.Message,
                    stackTrace = ex.StackTrace
                });
            }
        }
    }
}