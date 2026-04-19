using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol
{
    public class CrearUsuarioRolRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdRol { get; set; }
    }
}