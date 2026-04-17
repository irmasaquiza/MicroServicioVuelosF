using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class AuditoriaLogFiltroDataModel
    {
        // 🔍 Filtros
        public string TablaAfectada { get; set; }

        public string Operacion { get; set; } // INSERT, UPDATE, DELETE

        public string IdRegistroAfectado { get; set; }

        public string UsuarioEjecutor { get; set; }

        public bool? Activo { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaInicio { get; set; }
        public DateTime? FechaFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}