using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Ciudad/ActualizarCiudadRequest.cs
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Ciudad
{
    public class ActualizarCiudadRequest
    {
        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string? Nombre { get; set; }

     [StringLength(50)]
        public string? ZonaHoraria { get; set; }

        [Range(-90.0, 90.0, ErrorMessage = "La latitud debe estar entre -90 y 90.")]
        public decimal? Latitud { get; set; }

        [Range(-180.0, 180.0, ErrorMessage = "La longitud debe estar entre -180 y 180.")]
        public decimal? Longitud { get; set; }

        // ACTIVO / INACTIVO
        public string? Estado { get; set; }
    }
 
}
