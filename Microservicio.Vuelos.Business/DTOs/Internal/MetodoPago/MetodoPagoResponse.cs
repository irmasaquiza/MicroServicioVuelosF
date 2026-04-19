using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago
{
    public class MetodoPagoResponse
    {
        public int IdMetodo { get; set; }

        public int IdCliente { get; set; }

        public int IdTipoMetodo { get; set; }

        public string Ultimos4 { get; set; } = string.Empty;

        public string? ReferenciaVisible { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        public string? NombreTitular { get; set; }

        public string? MarcaTarjeta { get; set; }

        public string? BancoEmisor { get; set; }

        public string? PaisEmision { get; set; }

        public bool EsPrincipal { get; set; }

        public string? Alias { get; set; }

        public string Estado { get; set; } = string.Empty;
    }
}