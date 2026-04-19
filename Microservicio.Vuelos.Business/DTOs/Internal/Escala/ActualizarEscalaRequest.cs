using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Escala
{
    public class ActualizarEscalaRequest
    {
        [Range(1, int.MaxValue)]
        public int? Orden { get; set; }
 
    public DateTime? FechaHoraLlegada { get; set; }

        public DateTime? FechaHoraSalida { get; set; }

        [Range(0, int.MaxValue)]
        public int? DuracionMin { get; set; }

        public string? TipoEscala { get; set; }

        public string? Terminal { get; set; }

        public string? Puerta { get; set; }

        public string? Observaciones { get; set; }
    }
 

}
