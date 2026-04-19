using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Factura
{
    public class FacturaResponse
    {
        public int IdFactura { get; set; }

        public Guid GuidFactura { get; set; }

        public string NumeroFactura { get; set; } = string.Empty;

        public int IdCliente { get; set; }

        public int IdReserva { get; set; }

        public int IdMetodo { get; set; }

        public DateTime FechaEmision { get; set; }

        public decimal Subtotal { get; set; }

        public decimal ValorIva { get; set; }

        public decimal CargoServicio { get; set; }

        public decimal Total { get; set; }

        // ABI / APR / INA
        public string Estado { get; set; } = string.Empty;

        public string? ObservacionesFactura { get; set; }

        public string? OrigenCanalFactura { get; set; }
    }
}