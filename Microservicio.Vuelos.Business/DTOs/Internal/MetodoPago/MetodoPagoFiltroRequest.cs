/*using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago
{
    public class MetodoPagoFiltroRequest
    {
        public int? IdCliente { get; set; }

        public int? IdTipoMetodo { get; set; }

        public string? MarcaTarjeta { get; set; }

        public string? BancoEmisor { get; set; }

        public bool? EsPrincipal { get; set; }

        public string? Estado { get; set; }

        public DateTime? FechaExpiracionInicio { get; set; }

        public DateTime? FechaExpiracionFin { get; set; }

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
}*/