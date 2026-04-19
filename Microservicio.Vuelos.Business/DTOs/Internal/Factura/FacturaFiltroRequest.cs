using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Factura
{
    public class FacturaFiltroRequest
    {
        public int? IdCliente { get; set; }

        public int? IdReserva { get; set; }

        public int? IdMetodo { get; set; }

        public string? NumeroFactura { get; set; }

        public string? Estado { get; set; }

        public string? OrigenCanalFactura { get; set; }

        public decimal? TotalMin { get; set; }

        public decimal? TotalMax { get; set; }

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