using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class AsientoFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdVuelo { get; set; }

        public string NumeroAsiento { get; set; }
        public string Clase { get; set; }

        public bool? Disponible { get; set; }

        public string Posicion { get; set; } // VENTANA, PASILLO, CENTRO

        public string Estado { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}