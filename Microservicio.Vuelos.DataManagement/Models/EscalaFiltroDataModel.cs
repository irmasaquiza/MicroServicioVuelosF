using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class EscalaFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdVuelo { get; set; }
        public int? IdAeropuerto { get; set; }

        public int? Orden { get; set; }

        public string TipoEscala { get; set; }

        public string Terminal { get; set; }
        public string Puerta { get; set; }

        public string Estado { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // ⏱️ Duración
        public int? DuracionMin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}