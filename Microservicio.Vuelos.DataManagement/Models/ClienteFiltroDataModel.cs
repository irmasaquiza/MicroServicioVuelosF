using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class ClienteFiltroDataModel
    {
        // 🔍 Filtros
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

        public string Nombres { get; set; }
        public string Apellidos { get; set; }

        public string Correo { get; set; }

        public int? IdCiudadResidencia { get; set; }
        public int? IdPaisNacionalidad { get; set; }

        public string Estado { get; set; }

        public string ServicioOrigen { get; set; }

        // 📅 Rango de fechas
        public DateTime? FechaNacimientoInicio { get; set; }
        public DateTime? FechaNacimientoFin { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}