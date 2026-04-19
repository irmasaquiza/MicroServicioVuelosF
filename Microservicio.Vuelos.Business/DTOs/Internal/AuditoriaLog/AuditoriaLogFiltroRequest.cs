using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog
{
    public class AuditoriaLogFiltroRequest
    {
        public string? TablaAfectada { get; set; }

        // INSERT / UPDATE / DELETE
        public string? Operacion { get; set; }

        public string? UsuarioEjecutor { get; set; }

        public DateTime? FechaDesde { get; set; }

        public DateTime? FechaHasta { get; set; }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }

        private int _pageSize = 50;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 50 : value;
        }
    }
}