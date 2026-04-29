using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Reserva
{
    public class ReservaBookingResponse
    {
        public int IdReserva { get; set; }

        public string CodigoReserva { get; set; }

        public int IdCliente { get; set; }

        public int IdPasajero { get; set; }

        public int IdVuelo { get; set; }

        public int IdAsiento { get; set; }

        public DateTime FechaInicio { get; set; }

        public DateTime FechaFin { get; set; }

        public decimal TotalReserva { get; set; }

        public string EstadoReserva { get; set; }
    }
}