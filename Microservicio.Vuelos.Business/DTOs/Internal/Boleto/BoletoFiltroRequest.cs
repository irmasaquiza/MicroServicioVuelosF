using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Boleto
{
    public class BoletoFiltroRequest
    {
        public int? IdReserva { get; set; }

        public int? IdVuelo { get; set; }

        public int? IdAsiento { get; set; }

        public int? IdFactura { get; set; }

        public string? CodigoBoleto { get; set; }

        public string? Clase { get; set; }

        public string? EstadoBoleto { get; set; }

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