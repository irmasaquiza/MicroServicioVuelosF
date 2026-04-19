using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Pais/ActualizarPaisRequest.cs
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Pais
{
    public class ActualizarPaisRequest
    {
        [StringLength(2, MinimumLength = 2,
        ErrorMessage = "El código ISO2 debe tener exactamente 2 caracteres.")]
        public string? CodigoIso2 { get; set; }

 
    [StringLength(3, MinimumLength = 3,
        ErrorMessage = "El código ISO3 debe tener exactamente 3 caracteres.")]
        public string? CodigoIso3 { get; set; }

        [StringLength(100, ErrorMessage = "El nombre no puede superar 100 caracteres.")]
        public string? Nombre { get; set; }

        [StringLength(50, ErrorMessage = "El continente no puede superar 50 caracteres.")]
        public string? Continente { get; set; }

        // ACTIVO / INACTIVO
        public string? Estado { get; set; }
    }
 

}
