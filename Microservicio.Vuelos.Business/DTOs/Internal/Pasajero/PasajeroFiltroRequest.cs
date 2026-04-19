using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Pasajero
{
    public class PasajeroFiltroRequest
    {
        public int? IdCliente { get; set; }

        public string? NombrePasajero { get; set; }

        public string? ApellidoPasajero { get; set; }

        public string? TipoDocumentoPasajero { get; set; }

        public string? NumeroDocumentoPasajero { get; set; }

        public string? NacionalidadPasajero { get; set; }

        public bool? RequiereAsistencia { get; set; }

        public DateTime? FechaNacimientoInicio { get; set; }

        public DateTime? FechaNacimientoFin { get; set; }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 20 : value;
        }
    }
}