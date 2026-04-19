using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Vuelo/CrearVueloRequest.cs
// Coincide con schema VueloCreate del YAML
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Vuelo
{
    public class CrearVueloRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdAeropuertoOrigen { get; set; }

 
    [Required]
        [Range(1, int.MaxValue)]
        public int IdAeropuertoDestino { get; set; }

        [Required]
        [StringLength(10)]
        public string NumeroVuelo { get; set; } = string.Empty;

        [Required]
        public DateTime FechaHoraSalida { get; set; }

        [Required]
        public DateTime FechaHoraLlegada { get; set; }

        [Range(0, int.MaxValue)]
        public int DuracionMin { get; set; }

        [Required]
        [Range(0.01, double.MaxValue)]
        public decimal PrecioBase { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int CapacidadTotal { get; set; }
    }
 

}
