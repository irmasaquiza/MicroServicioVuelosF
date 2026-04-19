using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Factura
{
    public class ActualizarFacturaRequest
    {
        [Range(0, double.MaxValue)]
        public decimal? Subtotal { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? ValorIva { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? CargoServicio { get; set; }

        [Range(0, double.MaxValue)]
        public decimal? Total { get; set; }

        public string? ObservacionesFactura { get; set; }

        public string? OrigenCanalFactura { get; set; }

        // ABI / APR / INA
        public string? Estado { get; set; }
    }
}