using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Auth/LoginResponse.cs
// ============================================================
 
namespace Microservicio.Vuelos.Business.DTOs.Internal.Auth
{
    /// <summary>
    /// Respuesta del login con JWT y datos del usuario.
    /// </summary>
    public class LoginResponse
    {
        // Token JWT generado
        public string Token { get; set; } = string.Empty;

    // Tipo de token (Bearer)
    public string TokenType { get; set; } = "Bearer";

        // Fecha de expiración en UTC
        public DateTime ExpiraEnUtc { get; set; }

        // Roles del usuario (muy importante para autorización)
        public List<string> Roles { get; set; } = new();

        // Información del usuario autenticado
        public UsuarioLoginInfo Usuario { get; set; } = new UsuarioLoginInfo();
    }

    public class UsuarioLoginInfo
    {
        public int IdUsuario { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public string EstadoUsuario { get; set; } = string.Empty;

        // Opcional pro
        public Guid UsuarioGuid { get; set; }
    }
 
}
