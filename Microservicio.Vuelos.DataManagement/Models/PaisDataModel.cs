using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class PaisDataModel
    {
        public int IdPais { get; set; }

        public string CodigoIso2 { get; set; }
        public string CodigoIso3 { get; set; }

        public string Nombre { get; set; }
        public string Continente { get; set; }

        public string Estado { get; set; }
    }
}