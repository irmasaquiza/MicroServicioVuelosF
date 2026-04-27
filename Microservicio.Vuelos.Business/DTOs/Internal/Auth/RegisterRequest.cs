using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Auth
{
    public class RegisterRequest
    {
        // 👤 DATOS DEL CLIENTE
        [Required]
        public string Nombre { get; set; }

        [Required]
        public string Apellido { get; set; }

        [Required]
        public string Identificacion { get; set; }

        [Required]
        [EmailAddress]
        public string Correo { get; set; }

        public string Telefono { get; set; }

        // 🔐 DATOS DEL USUARIO
        [Required]
        public string Username { get; set; }

        [Required]
        [MinLength(6)]
        public string Password { get; set; }
    }
}