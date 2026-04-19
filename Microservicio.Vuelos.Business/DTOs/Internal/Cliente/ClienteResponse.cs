using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Cliente
{
    public class ClienteResponse
    {
        public int IdCliente { get; set; }

 
    public Guid ClienteGuid { get; set; }

        public string TipoIdentificacion { get; set; } = string.Empty;

        public string NumeroIdentificacion { get; set; } = string.Empty;

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }

        public string? RazonSocial { get; set; }

        public string Correo { get; set; } = string.Empty;

        public string Telefono { get; set; } = string.Empty;

        public string Direccion { get; set; } = string.Empty;

        public int IdCiudadResidencia { get; set; }

        public int IdPaisNacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        public string? Nacionalidad { get; set; }

        public string Genero { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
 

}
