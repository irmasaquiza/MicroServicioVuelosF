using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class UsuarioRolFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdUsuario { get; set; }
        public int? IdRol { get; set; }

        public string EstadoUsuarioRol { get; set; }

        public bool? Activo { get; set; }

        // 📅 Fechas (registro)
        public DateTime? FechaRegistroInicio { get; set; }
        public DateTime? FechaRegistroFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}