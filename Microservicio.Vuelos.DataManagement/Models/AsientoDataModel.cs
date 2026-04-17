using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class AsientoDataModel
    {
        public int IdAsiento { get; set; }

        public int IdVuelo { get; set; }

        public string NumeroAsiento { get; set; }
        public string Clase { get; set; }

        public bool Disponible { get; set; }

        public decimal PrecioExtra { get; set; }

        public string Posicion { get; set; }

        public string Estado { get; set; }
    }
}