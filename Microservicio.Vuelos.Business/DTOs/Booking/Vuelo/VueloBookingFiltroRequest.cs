using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Vuelo
{
    public class VueloBookingFiltroRequest
    {
        public int IdAeropuertoOrigen { get; set; }

        public int IdAeropuertoDestino { get; set; }

        public DateTime FechaSalida { get; set; }

        // Booking siempre debería usar PROGRAMADO
        public string EstadoVuelo { get; set; } = "PROGRAMADO";

        public int Page { get; set; } = 1;

        public int PageSize { get; set; } = 20;
    }
}