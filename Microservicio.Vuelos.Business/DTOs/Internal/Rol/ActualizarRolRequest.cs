using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Rol
{
    public class ActualizarRolRequest
    {
        [StringLength(50)]
        public string? NombreRol { get; set; }

        [StringLength(200)]
        public string? DescripcionRol { get; set; }

        public string? EstadoRol { get; set; }

        public bool? Activo { get; set; }
    }
}