using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Cliente
{
    public class ClienteBookingResponse
    {
        public int IdCliente { get; set; }

        public string ClienteGuid { get; set; }

        public string TipoIdentificacion { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public string Estado { get; set; }
    }
}