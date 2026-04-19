using System;
using System.Collections.Generic;
using System.Text;

using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.TipoMetodoPago
{
    public class CrearTipoMetodoPagoRequest
    {
        [Required]
        [StringLength(50)]
        public string NombreTipo { get; set; } = string.Empty;

        [StringLength(150)]
        public string? Descripcion { get; set; }
    }
}