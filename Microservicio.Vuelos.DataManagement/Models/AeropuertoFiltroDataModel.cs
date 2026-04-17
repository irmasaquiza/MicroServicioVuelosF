using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class AeropuertoFiltroDataModel
    {
        // 🔍 Filtros
        public string CodigoIata { get; set; }
        public string CodigoIcao { get; set; }
        public string Nombre { get; set; }

        public int? IdCiudad { get; set; }
        public int? IdPais { get; set; }

        public string Estado { get; set; } // ACTIVO, INACTIVO

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}