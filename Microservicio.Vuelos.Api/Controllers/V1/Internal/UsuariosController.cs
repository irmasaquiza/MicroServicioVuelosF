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
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioAppResponse>>), 200)]
        public async Task<IActionResult> GetAll([FromQuery] UsuarioAppFiltroRequest filtro)
        {
            try
            {
                var result = await _usuarioService.FiltrarAsync(filtro);

                return Ok(ApiResponse<IEnumerable<UsuarioAppResponse>>.Ok(result));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // POST: api/v1/usuarios
        // ============================================================
        [HttpPost]
        [ProducesResponseType(typeof(ApiResponse<UsuarioAppResponse>), 201)]
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
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (BusinessException ex)
            {
                return Conflict(ApiErrorResponse.FromBusiness(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // GET: api/v1/usuarios/{id}/roles
        // ============================================================
        [HttpGet("{id_usuario:int}/roles")]
        [ProducesResponseType(typeof(ApiResponse<IEnumerable<UsuarioRolResponse>>), 200)]
        public async Task<IActionResult> GetRoles(int id_usuario)
        {
            try
            {
                var result = await _usuarioRolService.GetByUsuarioAsync(id_usuario);

                return Ok(ApiResponse<IEnumerable<UsuarioRolResponse>>.Ok(result));
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
        // POST: api/v1/usuarios/{id}/roles
        // ============================================================
        [HttpPost("{id_usuario:int}/roles")]
        [ProducesResponseType(typeof(ApiResponse<UsuarioRolResponse>), 201)]
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
                return BadRequest(ApiErrorResponse.FromValidation(ex));
            }
            catch (NotFoundException ex)
            {
                return NotFound(ApiErrorResponse.FromNotFound(ex));
            }
            catch (BusinessException ex)
            {
                return Conflict(ApiErrorResponse.FromBusiness(ex));
            }
            catch (Exception)
            {
                return StatusCode(500, ApiErrorResponse.ErrorInterno());
            }
        }

        // ============================================================
        // DELETE: api/v1/usuarios/{id}/roles/{id_rol}
        // ============================================================
        [HttpDelete("{id_usuario:int}/roles/{id_rol:int}")]
        [ProducesResponseType(204)]
        public async Task<IActionResult> QuitarRol(int id_usuario, int id_rol)
        {
            try
            {
                var roles = await _usuarioRolService.GetByUsuarioAsync(id_usuario);

                var ur = roles.FirstOrDefault(r => r.IdRol == id_rol);

                if (ur == null)
                    return NotFound(ApiErrorResponse.Fail(
                        "USUARIOROL_NO_ENCONTRADO",
                        $"El usuario {id_usuario} no tiene asignado el rol {id_rol}."));

                await _usuarioRolService.EliminarAsync(ur.IdUsuarioRol);

                return NoContent();
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