using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class PaisFiltroDataModel
    {
        // 🔍 Filtros
        public string Nombre { get; set; }

        public string CodigoIso2 { get; set; }
        public string CodigoIso3 { get; set; }

        public string Continente { get; set; }

        public string Estado { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}