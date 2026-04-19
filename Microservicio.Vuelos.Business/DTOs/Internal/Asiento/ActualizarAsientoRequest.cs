using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Asiento
{
    public class ActualizarAsientoRequest
    {
        [StringLength(5)]
        public string? NumeroAsiento { get; set; }

 
    public string? Clase { get; set; }

        public bool? Disponible { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? PrecioExtra { get; set; }

        public string? Posicion { get; set; }
    }
 

}
