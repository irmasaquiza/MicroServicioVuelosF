using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Pasajero
{
    public class PasajeroBookingResponse
    {
        public int IdPasajero { get; set; }

        public string NombrePasajero { get; set; }

        public string ApellidoPasajero { get; set; }

        public string TipoDocumentoPasajero { get; set; }

        public string NumeroDocumentoPasajero { get; set; }

        public DateTime? FechaNacimientoPasajero { get; set; }

        public bool RequiereAsistencia { get; set; }
    }
}