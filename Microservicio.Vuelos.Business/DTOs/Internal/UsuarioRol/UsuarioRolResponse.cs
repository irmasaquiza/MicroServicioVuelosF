using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol
{
    public class UsuarioRolResponse
    {
        public int IdUsuarioRol { get; set; }

        public int IdUsuario { get; set; }

        public int IdRol { get; set; }

        // ACT / INA
        public string EstadoUsuarioRol { get; set; } = string.Empty;

        public bool Activo { get; set; }

        // OPCIONAL PRO (si lo tienes en BD)
        // public DateTime FechaRegistro { get; set; }
    }
}