using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Ciudad/CiudadResponse.cs
// ============================================================
namespace Microservicio.Vuelos.Business.DTOs.Internal.Ciudad
{
    public class CiudadResponse
    {
        public int IdCiudad { get; set; }

 
    public int IdPais { get; set; }

        public string Nombre { get; set; } = string.Empty;

        public string ZonaHoraria { get; set; } = string.Empty;

        public decimal? Latitud { get; set; }

        public decimal? Longitud { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
 

}
