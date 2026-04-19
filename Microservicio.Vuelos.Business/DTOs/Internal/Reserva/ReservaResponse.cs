using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Reserva
{
    public class ReservaResponse
    {
        public int IdReserva { get; set; }

        public Guid GuidReserva { get; set; }

        public string CodigoReserva { get; set; } = string.Empty;

        public int IdCliente { get; set; }

        public int IdPasajero { get; set; }

        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        public DateTime FechaReservaUtc { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public decimal SubtotalReserva { get; set; }

        public decimal ValorIva { get; set; }

        public decimal TotalReserva { get; set; }

        public string OrigenCanalReserva { get; set; } = string.Empty;

        public string EstadoReserva { get; set; } = string.Empty;

        public string? MotivoCancelacion { get; set; }

        public string? ContactoEmail { get; set; }

        public string? ContactoTelefono { get; set; }

        public string? Observaciones { get; set; }
    }
}