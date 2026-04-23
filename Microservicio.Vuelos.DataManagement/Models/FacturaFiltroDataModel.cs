using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class FacturaFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdCliente { get; set; }
        public int? IdReserva { get; set; }
     //   public int? IdMetodo { get; set; }

        public string NumeroFactura { get; set; }

        public string Estado { get; set; }

        public string OrigenCanalFactura { get; set; }

        // 💰 Rangos
        public decimal? TotalMin { get; set; }
        public decimal? TotalMax { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}