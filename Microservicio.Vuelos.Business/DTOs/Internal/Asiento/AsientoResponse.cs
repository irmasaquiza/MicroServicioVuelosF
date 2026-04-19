using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Asiento
{
    public class AsientoResponse
    {
        public int IdAsiento { get; set; }

 
    public int IdVuelo { get; set; }

        public string NumeroAsiento { get; set; } = string.Empty;

        public string Clase { get; set; } = string.Empty;

        public bool Disponible { get; set; }

        public decimal PrecioExtra { get; set; }

        public string Posicion { get; set; } = string.Empty;
    }
 

}
