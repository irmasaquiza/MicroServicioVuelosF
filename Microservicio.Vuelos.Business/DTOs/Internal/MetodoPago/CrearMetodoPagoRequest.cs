/*using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.ComponentModel.DataAnnotations;

namespace Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago
{
    public class CrearMetodoPagoRequest
    {
        [Required]
        [Range(1, int.MaxValue)]
        public int IdCliente { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int IdTipoMetodo { get; set; }

        [Required]
        [StringLength(255)]
        public string TokenPasarela { get; set; } = string.Empty;

        [StringLength(4, MinimumLength = 4)]
        public string? Ultimos4 { get; set; }

        public string? ReferenciaVisible { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        public string? NombreTitular { get; set; }

        public string? MarcaTarjeta { get; set; }

        public string? BancoEmisor { get; set; }

        public string? PaisEmision { get; set; }

        public bool EsPrincipal { get; set; } = false;

        public string? Alias { get; set; }
    }
}*/