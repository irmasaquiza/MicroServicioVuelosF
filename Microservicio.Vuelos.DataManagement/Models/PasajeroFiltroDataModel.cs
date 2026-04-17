using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class PasajeroFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdCliente { get; set; }

        public string NombrePasajero { get; set; }
        public string ApellidoPasajero { get; set; }

        public string TipoDocumentoPasajero { get; set; }
        public string NumeroDocumentoPasajero { get; set; }

        public string NacionalidadPasajero { get; set; }

        public bool? RequiereAsistencia { get; set; }

        public string Estado { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaNacimientoInicio { get; set; }
        public DateTime? FechaNacimientoFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}