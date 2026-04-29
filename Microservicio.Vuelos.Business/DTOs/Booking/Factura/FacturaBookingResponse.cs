using System;
using System.Collections.Generic;
using System.Text;


namespace Microservicio.Vuelos.Business.DTOs.Booking.Factura
{
    public class FacturaBookingResponse
    {
        public int IdFactura { get; set; }

        public int IdReserva { get; set; }

        public string NumeroFactura { get; set; }

        public DateTime FechaEmision { get; set; }

        public decimal Subtotal { get; set; }

        public decimal ValorIva { get; set; }

        public decimal Total { get; set; }

        public string Estado { get; set; }
    }
}