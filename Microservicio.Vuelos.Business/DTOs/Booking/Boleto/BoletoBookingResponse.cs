using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Boleto
{
    public class BoletoBookingResponse
    {
        public int IdBoleto { get; set; }

        public string CodigoBoleto { get; set; }

        public int IdReserva { get; set; }

        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        public int IdFactura { get; set; }

        public string Clase { get; set; }

        public decimal PrecioFinal { get; set; }

        public string EstadoBoleto { get; set; }

        public DateTime FechaEmision { get; set; }
    }
}