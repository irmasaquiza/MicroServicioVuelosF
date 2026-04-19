using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Cliente
{
    public class CrearClienteRequest
    {
        [Required]
        public string TipoIdentificacion { get; set; } = string.Empty;

     [Required]
        [StringLength(30)]
        public string NumeroIdentificacion { get; set; } = string.Empty;

        [StringLength(160)]
        public string? Nombres { get; set; }

        [StringLength(160)]
        public string? Apellidos { get; set; }

        [StringLength(200)]
        public string? RazonSocial { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Correo { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string Telefono { get; set; } = string.Empty;

        [Required]
        [StringLength(250)]
        public string Direccion { get; set; } = string.Empty;

        [Required]
        [Range(1, int.MaxValue)]
        public int IdCiudadResidencia { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdPaisNacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }

        [StringLength(80)]
        public string? Nacionalidad { get; set; }

        public string? Genero { get; set; }
    }
 
}

