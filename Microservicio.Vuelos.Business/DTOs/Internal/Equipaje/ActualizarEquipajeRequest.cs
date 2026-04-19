using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Equipaje
{
    public class ActualizarEquipajeRequest
    {
        [Required]
        public string EstadoEquipaje { get; set; } = string.Empty;
    }
}