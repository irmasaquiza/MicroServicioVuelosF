using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Reserva
{
    public class CrearReservaRequest
    {
//        [Required]
//        [Range(1, int.MaxValue)]
        public int IdCliente { get; set; }
        public int IdUsuario { get; set; } // 🔥 se llena en controller
        [Required]
        [Range(1, int.MaxValue)]
        public int IdPasajero { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdVuelo { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdAsiento { get; set; }

        [Required]
        public DateTime FechaInicio { get; set; }

        [Required]
        public DateTime FechaFin { get; set; }

        [Range(0, double.MaxValue)]
        public decimal SubtotalReserva { get; set; }

        [Range(0, double.MaxValue)]
        public decimal ValorIva { get; set; }

        [Range(0, double.MaxValue)]
        public decimal TotalReserva { get; set; }

        public string OrigenCanalReserva { get; set; } = "WEB";

        [EmailAddress]
        public string? ContactoEmail { get; set; }

        public string? ContactoTelefono { get; set; }

        public string? Observaciones { get; set; }
    }
}