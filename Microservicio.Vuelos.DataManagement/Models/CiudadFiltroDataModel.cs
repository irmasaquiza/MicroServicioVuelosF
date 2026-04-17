using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class CiudadFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdPais { get; set; }

        public string Nombre { get; set; }

        public string CodigoPostal { get; set; }

        public string ZonaHoraria { get; set; }

        public string Estado { get; set; }

        // 🌍 Opcional: filtros geográficos
        public decimal? LatitudMin { get; set; }
        public decimal? LatitudMax { get; set; }

        public decimal? LongitudMin { get; set; }
        public decimal? LongitudMax { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}