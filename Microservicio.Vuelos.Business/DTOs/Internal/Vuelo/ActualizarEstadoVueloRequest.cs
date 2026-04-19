using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Vuelo/ActualizarEstadoVueloRequest.cs
// Coincide con schema VueloEstadoUpdate del YAML
// Solo cambia el estado — PATCH /vuelos/{id}/estado
// ============================================================
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Vuelo
{
    public class ActualizarEstadoVueloRequest
    {
        // PROGRAMADO, EN_VUELO, ATERRIZADO, CANCELADO, DEMORADO
        [Required(ErrorMessage = "El estado del vuelo es obligatorio.")]
        public string EstadoVuelo { get; set; }

        // Requerido solo cuando EstadoVuelo = CANCELADO
        public string Motivo { get; set; }
    }
}