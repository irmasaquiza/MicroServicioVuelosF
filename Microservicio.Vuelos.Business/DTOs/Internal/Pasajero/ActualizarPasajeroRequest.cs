using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Pasajero
{
    public class ActualizarPasajeroRequest
    {
        [StringLength(100)]
        public string? NombrePasajero { get; set; }

        [StringLength(100)]
        public string? ApellidoPasajero { get; set; }

        public string? TipoDocumentoPasajero { get; set; }

        [StringLength(30)]
        public string? NumeroDocumentoPasajero { get; set; }

        public DateTime? FechaNacimientoPasajero { get; set; }

        [StringLength(80)]
        public string? NacionalidadPasajero { get; set; }

        [EmailAddress]
        public string? EmailContactoPasajero { get; set; }

        public string? TelefonoContactoPasajero { get; set; }

        public string? GeneroPasajero { get; set; }

        public bool? RequiereAsistencia { get; set; }

        public string? ObservacionesPasajero { get; set; }
    }
}