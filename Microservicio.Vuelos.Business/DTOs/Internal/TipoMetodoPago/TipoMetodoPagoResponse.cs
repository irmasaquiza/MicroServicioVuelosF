using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.TipoMetodoPago
{
    public class TipoMetodoPagoResponse
    {
        public int IdTipoMetodo { get; set; }

        public string NombreTipo { get; set; } = string.Empty;

        public string? Descripcion { get; set; }

        // ACTIVO / INACTIVO
        public string Estado { get; set; } = string.Empty;
    }
}