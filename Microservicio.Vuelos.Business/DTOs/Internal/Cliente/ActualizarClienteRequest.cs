using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Cliente
{
    public class ActualizarClienteRequest
    {
        [StringLength(160)]
        public string? Nombres { get; set; }
 
    [StringLength(160)]
        public string? Apellidos { get; set; }

        [StringLength(200)]
        public string? RazonSocial { get; set; }

        [EmailAddress]
        [StringLength(150)]
        public string? Correo { get; set; }

        [StringLength(30)]
        public string? Telefono { get; set; }

        [StringLength(250)]
        public string? Direccion { get; set; }

        [Range(1, int.MaxValue)]
        public int? IdCiudadResidencia { get; set; }

        [Range(1, int.MaxValue)]
        public int? IdPaisNacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(80)]
        public string? Nacionalidad { get; set; }

        public string? Genero { get; set; }

        public string? Estado { get; set; }
    }
 

}
