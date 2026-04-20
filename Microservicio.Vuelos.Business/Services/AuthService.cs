using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Microservicio.Vuelos.Business.DTOs.Internal.Auth;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUsuarioAppDataService _usuarioDataService;
        private readonly IUsuarioRolDataService _usuarioRolDataService;
        private readonly IRolDataService _rolDataService;
        private readonly IConfiguration _config;

        public AuthService(
            IUsuarioAppDataService usuarioDataService,
            IUsuarioRolDataService usuarioRolDataService,
            IRolDataService rolDataService,
            IConfiguration config)
        {
            _usuarioDataService = usuarioDataService;
            _usuarioRolDataService = usuarioRolDataService;
            _rolDataService = rolDataService;
            _config = config;
        }

        public async Task<LoginResponse> LoginAsync(LoginRequest request)
        {
            if (request == null)
                throw new ValidationException("La solicitud de login es requerida.");

            if (string.IsNullOrWhiteSpace(request.Login))
                throw new ValidationException("El usuario o correo es obligatorio.");

            if (string.IsNullOrWhiteSpace(request.Password))
                throw new ValidationException("La contraseña es obligatoria.");

            // 🔥 USAR ENTITY (NO DataModel)
            var usuario = await _usuarioDataService.GetByCredentialsAsync(request.Login);

            if (usuario == null)
                throw new UnauthorizedBusinessException(
                    "CREDENCIALES_INVALIDAS",
                    "Usuario o contraseña incorrectos.",
                    true);

            // 🔥 VALIDACIÓN SIMPLE (PARA PRUEBAS)
            if (usuario.PasswordHash != request.Password)
                throw new UnauthorizedBusinessException(
                    "CREDENCIALES_INVALIDAS",
                    "Usuario o contraseña incorrectos.",
                    true);

            // ── Estado
            if (usuario.EstadoUsuario != "ACT" || !usuario.Activo)
                throw new UnauthorizedBusinessException(
                    "USUARIO_INACTIVO",
                    "El usuario se encuentra inactivo.",
                    true);

            // ── Roles
            var usuarioRoles = await _usuarioRolDataService.GetByUsuarioAsync(usuario.IdUsuario);
            var roles = new List<string>();

            foreach (var ur in usuarioRoles.Where(r => r.Activo))
            {
                var rol = await _rolDataService.GetByIdAsync(ur.IdRol);
                if (rol != null && rol.Activo)
                    roles.Add(rol.NombreRol);
            }

            // ── JWT
            var (token, expira) = GenerarJwt(usuario.IdUsuario, usuario.Username, roles);

            return new LoginResponse
            {
                Token = token,
                TokenType = "Bearer",
                ExpiraEnUtc = expira,
                Roles = roles,
                Usuario = new UsuarioLoginInfo
                {
                    IdUsuario = usuario.IdUsuario,
                    UsuarioGuid = usuario.UsuarioGuid,
                    Username = usuario.Username,
                    Correo = usuario.Correo,
                    EstadoUsuario = usuario.EstadoUsuario
                }
            };
        }

        private (string token, DateTime expira) GenerarJwt(
            int idUsuario, string username, List<string> roles)
        {
            var jwtSettings = _config.GetSection("JwtSettings");

            var key = jwtSettings["SecretKey"];
            var issuer = jwtSettings["Issuer"];
            var audience = jwtSettings["Audience"];
            var expMinutes = int.TryParse(jwtSettings["ExpirationMinutes"], out var min) ? min : 60;

            if (string.IsNullOrEmpty(key))
                throw new Exception("JWT SecretKey no configurada");

            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var expira = DateTime.UtcNow.AddMinutes(expMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub, idUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
                claims.Add(new Claim(ClaimTypes.Role, rol));

            var token = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: expira,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expira);
        }
    }
}