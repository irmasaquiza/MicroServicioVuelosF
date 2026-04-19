using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Escala
{
    public class CrearEscalaRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdAeropuerto { get; set; }
 
    [Required]
        [Range(1, int.MaxValue)]
        public int Orden { get; set; }

        [Required]
        public DateTime FechaHoraLlegada { get; set; }

        [Required]
        public DateTime FechaHoraSalida { get; set; }

        [Range(0, int.MaxValue)]
        public int DuracionMin { get; set; }

        public string TipoEscala { get; set; } = "COMERCIAL";

        public string? Terminal { get; set; }

        public string? Puerta { get; set; }

        public string? Observaciones { get; set; }
    }
 

}
