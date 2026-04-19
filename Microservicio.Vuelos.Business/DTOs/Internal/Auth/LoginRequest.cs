using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Auth
{
    /// <summary>
    /// Request para autenticación de usuario.
    /// </summary>
    public class LoginRequest
    {
        // Usuario o correo
        [Required(ErrorMessage = "El usuario o correo es obligatorio.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Login { get; set; } = string.Empty;

 
    // Contraseña en texto plano
    [Required(ErrorMessage = "La contraseña es obligatoria.")]
        [MinLength(8, ErrorMessage = "Debe tener al menos 8 caracteres.")]
        [MaxLength(100, ErrorMessage = "Máximo 100 caracteres.")]
        public string Password { get; set; } = string.Empty;
    }
 

}
