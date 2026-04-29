using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Booking.Cliente
{
    public class CrearClienteBookingRequest
    {
        public string TipoIdentificacion { get; set; }

        public string NumeroIdentificacion { get; set; }

        public string Nombres { get; set; }

        public string Apellidos { get; set; }

        public string Correo { get; set; }

        public string Telefono { get; set; }

        public string Direccion { get; set; }

        public int IdCiudadResidencia { get; set; }

        public int IdPaisNacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string Nacionalidad { get; set; }

        public string Genero { get; set; }
    }
}