using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Vuelo
{
    public class VueloBookingResponse
    {
        public int IdVuelo { get; set; }

        public string NumeroVuelo { get; set; }

        public int IdAeropuertoOrigen { get; set; }

        public int IdAeropuertoDestino { get; set; }

        public DateTime FechaHoraSalida { get; set; }

        public DateTime FechaHoraLlegada { get; set; }

        public int DuracionMin { get; set; }

        public decimal PrecioBase { get; set; }

        public int CapacidadTotal { get; set; }

        public string EstadoVuelo { get; set; }
    }
}