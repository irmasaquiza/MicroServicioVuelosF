using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Rol
{
    public class RolResponse
    {
        public int IdRol { get; set; }

        public Guid RolGuid { get; set; }

        public string NombreRol { get; set; } = string.Empty;

        public string? DescripcionRol { get; set; }

        // ACT / INA
        public string EstadoRol { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}