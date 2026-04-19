using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Cliente
{
    public class ClienteFiltroRequest
    {
        public string? TipoIdentificacion { get; set; }

 
    public string? NumeroIdentificacion { get; set; }

        public string? Nombres { get; set; }

        public string? Apellidos { get; set; }

        public string? Correo { get; set; }

        public int? IdCiudadResidencia { get; set; }

        public int? IdPaisNacionalidad { get; set; }

        public string? Estado { get; set; }

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
