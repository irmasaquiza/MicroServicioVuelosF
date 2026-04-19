using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Ciudad/CrearCiudadRequest.cs
// Coincide con schema CiudadCreate del YAML
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Ciudad
{
    public class CrearCiudadRequest
    {
        [Required(ErrorMessage = "El ID del país es obligatorio.")]
        [Range(1, int.MaxValue, ErrorMessage = "El ID del país debe ser mayor a 0.")]
        public int IdPais { get; set; }

 
    [Required(ErrorMessage = "El nombre de la ciudad es obligatorio.")]
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string Nombre { get; set; } = string.Empty;

        [StringLength(50, ErrorMessage = "La zona horaria no puede superar 50 caracteres.")]
        public string? ZonaHoraria { get; set; }

        [Range(-90.0, 90.0, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public decimal? Latitud { get; set; }

        [Range(-180.0, 180.0, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public decimal? Longitud { get; set; }
    }
 

}
