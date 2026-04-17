using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class MetodoPagoFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdCliente { get; set; }
        public int? IdTipoMetodo { get; set; }

        public string Ultimos4 { get; set; }
        public string ReferenciaVisible { get; set; }

        public string MarcaTarjeta { get; set; }
        public string BancoEmisor { get; set; }
        public string PaisEmision { get; set; }

        public bool? EsPrincipal { get; set; }

        public string Estado { get; set; }

        // 📅 Fechas
        public DateTime? FechaExpiracionInicio { get; set; }
        public DateTime? FechaExpiracionFin { get; set; }

        public DateTime? FechaUltimoUsoInicio { get; set; }
        public DateTime? FechaUltimoUsoFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}