using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class TipoMetodoPagoFiltroDataModel
    {
        // 🔍 Filtros
        public string NombreTipo { get; set; }

        public string Estado { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}