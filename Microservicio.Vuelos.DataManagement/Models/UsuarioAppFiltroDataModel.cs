using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class UsuarioAppFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdCliente { get; set; }

        public string Username { get; set; }
        public string Correo { get; set; }

        public string EstadoUsuario { get; set; }

        public bool? Activo { get; set; }

        // 📅 Fechas
        public DateTime? FechaUltimoLoginInicio { get; set; }
        public DateTime? FechaUltimoLoginFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}