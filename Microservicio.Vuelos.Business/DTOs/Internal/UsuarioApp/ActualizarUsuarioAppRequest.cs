using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp
{
    public class ActualizarUsuarioAppRequest
    {
        [StringLength(50)]
        public string? Username { get; set; }

        [EmailAddress]
        [StringLength(120)]
        public string? Correo { get; set; }

        // ACT / INA
        public string? EstadoUsuario { get; set; }

        public bool? Activo { get; set; }
    }
}