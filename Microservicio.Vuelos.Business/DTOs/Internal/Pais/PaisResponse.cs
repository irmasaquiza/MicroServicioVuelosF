using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Pais/PaisResponse.cs
// Coincide con schema Pais del contrato YAML
// ============================================================
namespace Microservicio.Vuelos.Business.DTOs.Internal.Pais
{
    public class PaisResponse
    {
        public int IdPais { get; set; }
    public string CodigoIso2 { get; set; } = string.Empty;

        public string CodigoIso3 { get; set; } = string.Empty;

        public string Nombre { get; set; } = string.Empty;

        public string Continente { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
 

}
