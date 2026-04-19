using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Reserva
{
    public class ActualizarEstadoReservaRequest
    {
        [Required]
        public string EstadoReserva { get; set; } = string.Empty;

        public string? MotivoCancelacion { get; set; }
    }
}