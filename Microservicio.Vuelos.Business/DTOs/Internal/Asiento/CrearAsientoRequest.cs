using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Asiento
{
    public class CrearAsientoRequest
    {
        [Required(ErrorMessage = "El número de asiento es obligatorio.")]
        [StringLength(5, ErrorMessage = "Máximo 5 caracteres.")]
        public string NumeroAsiento { get; set; } = string.Empty;

 
    // ECONOMICA / EJECUTIVA / PRIMERA
    public string Clase { get; set; } = "ECONOMICA";

        [Range(0, double.MaxValue)]
        public decimal PrecioExtra { get; set; } = 0;

        // VENTANA / PASILLO / CENTRO
        public string? Posicion { get; set; }
    }
 

}
