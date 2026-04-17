using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class TipoMetodoPagoDataModel
    {
        public int IdTipoMetodo { get; set; }

        public string NombreTipo { get; set; }
        public string Descripcion { get; set; }

        public string Estado { get; set; }
    }
}