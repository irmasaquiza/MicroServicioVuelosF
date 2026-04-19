using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Pasajero
{
    public class PasajeroResponse
    {
        public int IdPasajero { get; set; }

        public int? IdCliente { get; set; }

        public string NombrePasajero { get; set; } = string.Empty;

        public string ApellidoPasajero { get; set; } = string.Empty;

        public string TipoDocumentoPasajero { get; set; } = string.Empty;

        public string NumeroDocumentoPasajero { get; set; } = string.Empty;

        public DateTime? FechaNacimientoPasajero { get; set; }

        public string? NacionalidadPasajero { get; set; }

        public string? EmailContactoPasajero { get; set; }

        public string? TelefonoContactoPasajero { get; set; }

        public string? GeneroPasajero { get; set; }

        public bool RequiereAsistencia { get; set; }

        public string? ObservacionesPasajero { get; set; }
    }
}
