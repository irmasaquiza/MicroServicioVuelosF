using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Factura
{
    public class CrearFacturaRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdCliente { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdReserva { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdMetodo { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Subtotal { get; set; }

        [Required]
        [Range(0, double.MaxValue)]
        public decimal ValorIva { get; set; }

        [Range(0, double.MaxValue)]
        public decimal CargoServicio { get; set; } = 0;

        [Required]
        [Range(0, double.MaxValue)]
        public decimal Total { get; set; }

        public string? ObservacionesFactura { get; set; }

        public string? OrigenCanalFactura { get; set; }
    }
}