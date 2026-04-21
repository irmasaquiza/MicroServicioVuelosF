using Asp.Versioning;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp;
using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;

namespace Microservicio.Vuelos.Api.Controllers.V1.Internal
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/usuarios")]
    [Authorize(Roles = "ADMINISTRADOR")]
    public class UsuariosController : ControllerBase
    {
        private readonly IUsuarioAppService _usuarioService;
        private readonly IUsuarioRolService _usuarioRolService;

        public UsuariosController(
            IUsuarioAppService usuarioService,
            IUsuarioRolService usuarioRolService)
        {
            _usuarioService = usuarioService;
            _usuarioRolService = usuarioRolService;
        }

        // ============================================================
        // GET: api/v1/usuarios
        // ============================================================
        [HttpGet]
        public async Task<IActionResult> GetAll([FromQuery] UsuarioAppFiltroRequest filtro)
        {
            try
            {
                var result = await _usuarioService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<UsuarioAppResponse>>.Ok(result));
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
        // POST: api/v1/usuarios
        // ============================================================
        [HttpPost]
        public async Task<IActionResult> Crear([FromBody] CrearUsuarioAppRequest request)
        {
            try
            {
                var result = await _usuarioService.CrearAsync(request);

                return CreatedAtAction(nameof(GetAll),
                    null,
                    ApiResponse<UsuarioAppResponse>.Ok(result));
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
                return Conflict(new
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
        // GET: api/v1/usuarios/{id}/roles
        // ============================================================
        [HttpGet("{id_usuario:int}/roles")]
        public async Task<IActionResult> GetRoles(int id_usuario)
        {
            try
            {
                var result = await _usuarioRolService.GetByUsuarioAsync(id_usuario);

                return Ok(ApiResponse<IEnumerable<UsuarioRolResponse>>.Ok(result));
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
        // POST: api/v1/usuarios/{id}/roles
        // ============================================================
        [HttpPost("{id_usuario:int}/roles")]
        public async Task<IActionResult> AsignarRol(
            int id_usuario,
            [FromBody] CrearUsuarioRolRequest request)
        {
            try
            {
                var result = await _usuarioRolService.CrearAsync(id_usuario, request);

                return CreatedAtAction(nameof(GetRoles),
                    new { id_usuario },
                    ApiResponse<UsuarioRolResponse>.Ok(result));
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
            catch (NotFoundException ex)
            {
                return NotFound(new
                {
                    success = false,
                    tipo = "NOT_FOUND",
                    mensaje = ex.Message
                });
            }
            catch (BusinessException ex)
            {
                return Conflict(new
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

        // ============================================================
        // DELETE: api/v1/usuarios/{id}/roles/{id_rol}
        // ============================================================
        [HttpDelete("{id_usuario:int}/roles/{id_rol:int}")]
        public async Task<IActionResult> QuitarRol(int id_usuario, int id_rol)
        {
            try
            {
                var roles = await _usuarioRolService.GetByUsuarioAsync(id_usuario);

                var ur = roles.FirstOrDefault(r => r.IdRol == id_rol);

                if (ur == null)
                    return NotFound(new
                    {
                        success = false,
                        tipo = "NOT_FOUND",
                        mensaje = $"El usuario {id_usuario} no tiene el rol {id_rol}"
                    });

                await _usuarioRolService.EliminarAsync(ur.IdUsuarioRol);

                return NoContent();
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