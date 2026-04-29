using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Pasajero
{
    public class CrearPasajeroBookingRequest
    {
        public string NombrePasajero { get; set; }

        public string ApellidoPasajero { get; set; }

        public string TipoDocumentoPasajero { get; set; }

        public string NumeroDocumentoPasajero { get; set; }

        public int? IdCliente { get; set; }

        public DateTime? FechaNacimientoPasajero { get; set; }

        public string NacionalidadPasajero { get; set; }

        public string EmailContactoPasajero { get; set; }

        public string TelefonoContactoPasajero { get; set; }

        public string GeneroPasajero { get; set; }

        public bool RequiereAsistencia { get; set; } = false;

        public string ObservacionesPasajero { get; set; }
    }
}