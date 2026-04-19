// ============================================================
// Services/AuthService.cs
// ============================================================
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Security.Cryptography;
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

            // ── Buscar usuario por username o correo ──────────────────
            var usuario = await _usuarioDataService.GetByUsernameAsync(request.Login)
                       ?? await _usuarioDataService.GetByCorreoAsync(request.Login);

            if (usuario == null)
                throw new UnauthorizedBusinessException(
                    "CREDENCIALES_INVALIDAS",
                    "Usuario o contraseña incorrectos.",
                    true);

            // ── Verificar contraseña ──────────────────────────────────  ✅ NUEVO
            var passwordHash = HashPassword(request.Password);

            if (usuario.Password != passwordHash)
                throw new UnauthorizedBusinessException(
                    "CREDENCIALES_INVALIDAS",
                    "Usuario o contraseña incorrectos.",
                    true);

            // ── Verificar estado ──────────────────────────────────────
            if (usuario.EstadoUsuario != "ACT" || !usuario.Activo)
                throw new UnauthorizedBusinessException(
                    "USUARIO_INACTIVO",
                    "El usuario se encuentra inactivo.",
                    true);

            // ── Obtener roles del usuario ─────────────────────────────
            var usuarioRoles = await _usuarioRolDataService.GetByUsuarioAsync(usuario.IdUsuario);
            var roles = new List<string>();

            foreach (var ur in usuarioRoles.Where(r => r.Activo))
            {
                var rol = await _rolDataService.GetByIdAsync(ur.IdRol);
                if (rol != null && rol.Activo)
                    roles.Add(rol.NombreRol);
            }

            // ── Generar JWT ───────────────────────────────────────────
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

        // ── Hash SHA256 ───────────────────────────────────────────────  ✅ NUEVO
        private string HashPassword(string password)
        {
            using var sha256 = SHA256.Create();
            var bytes = Encoding.UTF8.GetBytes(password);
            var hash = sha256.ComputeHash(bytes);
            return Convert.ToBase64String(hash);
        }

        private (string token, DateTime expira) GenerarJwt(
            int idUsuario, string username, List<string> roles)
        {
            // ✅ CORREGIDO: lanza error si no está configurado
            var jwtKey = _config["Jwt:Key"]
                ?? throw new InvalidOperationException("Jwt:Key no está configurado.");

            var jwtIssuer = _config["Jwt:Issuer"] ?? "Microservicio.Vuelos";
            var jwtAud = _config["Jwt:Audience"] ?? "Microservicio.Vuelos.Client";

            // ✅ CORREGIDO: TryParse en lugar de Parse
            var expMinutes = int.TryParse(_config["Jwt:ExpirationMinutes"], out var min) ? min : 60;

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey));
            var credentials = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expira = DateTime.UtcNow.AddMinutes(expMinutes);

            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Sub,        idUsuario.ToString()),
                new Claim(JwtRegisteredClaimNames.UniqueName, username),
                new Claim(JwtRegisteredClaimNames.Jti,        Guid.NewGuid().ToString())
            };

            foreach (var rol in roles)
                claims.Add(new Claim(ClaimTypes.Role, rol));

            var token = new JwtSecurityToken(
                issuer: jwtIssuer,
                audience: jwtAud,
                claims: claims,
                expires: expira,
                signingCredentials: credentials);

            return (new JwtSecurityTokenHandler().WriteToken(token), expira);
        }
    }
}