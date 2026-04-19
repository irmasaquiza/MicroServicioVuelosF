using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Pasajero
{
    public class CrearPasajeroRequest
    {
        public int? IdCliente { get; set; }

        [Required]
        [StringLength(100)]
        public string NombrePasajero { get; set; } = string.Empty;

        [Required]
        [StringLength(100)]
        public string ApellidoPasajero { get; set; } = string.Empty;

        [Required]
        public string TipoDocumentoPasajero { get; set; } = string.Empty;

        [Required]
        [StringLength(30)]
        public string NumeroDocumentoPasajero { get; set; } = string.Empty;

        public DateTime? FechaNacimientoPasajero { get; set; }

        [StringLength(80)]
        public string? NacionalidadPasajero { get; set; }

        [EmailAddress]
        public string? EmailContactoPasajero { get; set; }

        public string? TelefonoContactoPasajero { get; set; }

        public string? GeneroPasajero { get; set; }

        public bool RequiereAsistencia { get; set; } = false;

        public string? ObservacionesPasajero { get; set; }
    }
}