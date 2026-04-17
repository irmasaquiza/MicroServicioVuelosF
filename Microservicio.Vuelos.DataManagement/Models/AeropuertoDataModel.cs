using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class AeropuertoDataModel
    {
        public int IdAeropuerto { get; set; }

        public string CodigoIata { get; set; }
        public string CodigoIcao { get; set; }

        public string Nombre { get; set; }

        public int? IdCiudad { get; set; }
        public int IdPais { get; set; }

        public string ZonaHoraria { get; set; }

        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public string Estado { get; set; }
    }
}
