using Asp.Versioning;
using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.DTOs.Internal.Auth;
using Microservicio.Vuelos.Business.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Microservicio.Vuelos.Api.Controllers.V1.Auth
{
    [ApiController]
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/auth")]
    [AllowAnonymous]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        // POST: api/v1/auth/login
        [HttpPost("login")]
        [ProducesResponseType(typeof(ApiResponse<LoginResponse>), 200)]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var result = await _authService.LoginAsync(request);

            return Ok(new ApiResponse<LoginResponse>
            {
                Success = true,
                Message = "Login exitoso",
                Data = result
            });
        }


        // POST: api/v1/auth/register
        [HttpPost("register")]
        [ProducesResponseType(typeof(ApiResponse<object>), 200)]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            await _authService.RegisterAsync(request);

            return Ok(new ApiResponse<object>
            {
                Success = true,
                Message = "Usuario registrado correctamente",
                Data = null
            });
        }



    }
}