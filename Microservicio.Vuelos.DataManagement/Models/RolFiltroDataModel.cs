using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class RolFiltroDataModel
    {
        // 🔍 Filtros
        public string NombreRol { get; set; }

        public string EstadoRol { get; set; }

        public bool? Activo { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}