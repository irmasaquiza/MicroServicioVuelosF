using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Reserva
{
    public class ActualizarReservaRequest
    {
        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? SubtotalReserva { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ValorIva { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? TotalReserva { get; set; }

        [EmailAddress]
        public string? ContactoEmail { get; set; }

        public string? ContactoTelefono { get; set; }

        public string? Observaciones { get; set; }
    }
}