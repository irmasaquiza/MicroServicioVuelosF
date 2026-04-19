using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Aeropuerto/CrearAeropuertoRequest.cs
// Coincide con schema AeropuertoCreate del YAML
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto
{
    public class CrearAeropuertoRequest
    {
        [Required(ErrorMessage = "El código IATA es obligatorio.")]
        [StringLength(3, MinimumLength = 3,
        ErrorMessage = "El código IATA debe tener exactamente 3 caracteres.")]
        public string CodigoIata { get; set; } = string.Empty;

  
    [StringLength(4, MinimumLength = 4,
        ErrorMessage = "El código ICAO debe tener exactamente 4 caracteres.")]
        public string? CodigoIcao { get; set; }

        [Required(ErrorMessage = "El nombre del aeropuerto es obligatorio.")]
        [StringLength(150, ErrorMessage = "El nombre no puede superar 150 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        public int? IdCiudad { get; set; }

        [Required(ErrorMessage = "El ID del país es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del país debe ser mayor a 0.")]
        public int IdPais { get; set; }

        [StringLength(50)]
        public string? ZonaHoraria { get; set; }

        [Range(-90.0, 90.0)]
        public decimal? Latitud { get; set; }

        [Range(-180.0, 180.0)]
        public decimal? Longitud { get; set; }
    }
 

}
