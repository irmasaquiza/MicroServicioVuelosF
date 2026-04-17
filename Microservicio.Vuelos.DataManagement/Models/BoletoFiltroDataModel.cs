using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class BoletoFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdReserva { get; set; }
        public int? IdVuelo { get; set; }
        public int? IdAsiento { get; set; }
        public int? IdFactura { get; set; }

        public string CodigoBoleto { get; set; }
        public string Clase { get; set; }

        public string EstadoBoleto { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}