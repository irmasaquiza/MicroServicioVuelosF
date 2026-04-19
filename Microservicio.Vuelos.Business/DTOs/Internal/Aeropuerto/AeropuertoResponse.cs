using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Aeropuerto/AeropuertoResponse.cs
// ============================================================
namespace Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto
{
    public class AeropuertoResponse
    {
        public int IdAeropuerto { get; set; }
 
    public string CodigoIata { get; set; } = string.Empty;

        public string CodigoIcao { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public int? IdCiudad { get; set; }

        public int IdPais { get; set; }

        public string ZonaHoraria { get; set; } = string.Empty;

        public decimal? Latitud { get; set; }

        public decimal? Longitud { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
 

}
