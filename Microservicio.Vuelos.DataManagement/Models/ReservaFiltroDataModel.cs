using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class ReservaFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdCliente { get; set; }
        public int? IdPasajero { get; set; }
        public int? IdVuelo { get; set; }

        public string CodigoReserva { get; set; }

        public string EstadoReserva { get; set; }

        public string OrigenCanalReserva { get; set; }

        // 💰 Rangos
        public decimal? TotalMin { get; set; }
        public decimal? TotalMax { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaReservaInicio { get; set; }
        public DateTime? FechaReservaFin { get; set; }

        public DateTime? FechaInicioViaje { get; set; }
        public DateTime? FechaFinViaje { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}