using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog
{
    public class AuditoriaLogResponse
    {
        public long IdAuditoria { get; set; }

        public Guid AuditoriaGuid { get; set; }

        public string TablaAfectada { get; set; } = string.Empty;

        // INSERT / UPDATE / DELETE
        public string Operacion { get; set; } = string.Empty;

        public string? IdRegistroAfectado { get; set; }

        public string? DatosAnteriores { get; set; }

        public string? DatosNuevos { get; set; }

        public string? UsuarioEjecutor { get; set; }

        public string? IpOrigen { get; set; }

        public DateTime FechaEventoUtc { get; set; }
    }
}