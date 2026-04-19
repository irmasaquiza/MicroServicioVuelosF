using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Vuelo/ActualizarVueloRequest.cs
// ============================================================
using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Vuelo
{
    public class ActualizarVueloRequest
    {
        [StringLength(10)]
        public string? NumeroVuelo { get; set; }

        public DateTime? FechaHoraSalida { get; set; }
        public DateTime? FechaHoraLlegada { get; set; }

        [Range(0, int.MaxValue)]
        public int? DuracionMin { get; set; }

        [Range(0.01, double.MaxValue)]
        public decimal? PrecioBase { get; set; }

        [Range(1, int.MaxValue)]
        public int? CapacidadTotal { get; set; }
    }
}