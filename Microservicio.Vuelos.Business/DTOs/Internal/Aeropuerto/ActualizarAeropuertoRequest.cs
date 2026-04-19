using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

// ============================================================
// Aeropuerto/ActualizarAeropuertoRequest.cs
// ============================================================
using System.ComponentModel.DataAnnotations;
namespace Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto
{
    public class ActualizarAeropuertoRequest
    {
        [StringLength(3, MinimumLength = 3)]
        public string? CodigoIata { get; set; }
 
    [StringLength(4, MinimumLength = 4)]
        public string? CodigoIcao { get; set; }

        [StringLength(150)]
        public string? Nombre { get; set; }

        public int? IdCiudad { get; set; }

        public int? IdPais { get; set; }

        [StringLength(50)]
        public string? ZonaHoraria { get; set; }

        [Range(-90.0, 90.0)]
        public decimal? Latitud { get; set; }

        [Range(-180.0, 180.0)]
        public decimal? Longitud { get; set; }

        // ACTIVO / INACTIVO
        public string? Estado { get; set; }
    }
 

}
