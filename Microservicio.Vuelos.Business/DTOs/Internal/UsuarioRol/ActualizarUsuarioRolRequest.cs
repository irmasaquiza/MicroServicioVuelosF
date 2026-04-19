using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol
{
    public class ActualizarUsuarioRolRequest
    {
        public string? EstadoUsuarioRol { get; set; }

        public bool? Activo { get; set; }
    }
}