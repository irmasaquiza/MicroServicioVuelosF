using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Escala
{
    public class EscalaFiltroRequest
    {
        public int? IdVuelo { get; set; }

 
    public int? IdAeropuerto { get; set; }

        public int? Orden { get; set; }

        public string? TipoEscala { get; set; }

        public DateTime? FechaInicio { get; set; }

        public DateTime? FechaFin { get; set; }

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
