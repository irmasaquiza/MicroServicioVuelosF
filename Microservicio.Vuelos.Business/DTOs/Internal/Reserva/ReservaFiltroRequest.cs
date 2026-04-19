using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Reserva
{
    public class ReservaFiltroRequest
    {
        public int? IdCliente { get; set; }

        public int? IdPasajero { get; set; }

        public int? IdVuelo { get; set; }

        public string? CodigoReserva { get; set; }

        public string? EstadoReserva { get; set; }

        public string? OrigenCanalReserva { get; set; }

        public decimal? TotalMin { get; set; }

        public decimal? TotalMax { get; set; }

        public DateTime? FechaReservaInicio { get; set; }

        public DateTime? FechaReservaFin { get; set; }

        public DateTime? FechaInicioViaje { get; set; }

        public DateTime? FechaFinViaje { get; set; }

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