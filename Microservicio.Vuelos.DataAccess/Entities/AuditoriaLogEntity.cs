using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class AuditoriaLogEntity
    {
        public long IdAuditoria { get; set; }

        public Guid AuditoriaGuid { get; set; }

        public string TablaAfectada { get; set; } = null!;   // ✔ obligatorio
        public string Operacion { get; set; } = null!;       // ✔ obligatorio

        public string? IdRegistroAfectado { get; set; }      // 🔥 nullable

        public string? DatosAnteriores { get; set; }         // 🔥 nullable (ej: INSERT no tiene)
        public string? DatosNuevos { get; set; }             // 🔥 nullable (ej: DELETE no tiene)

        public string UsuarioEjecutor { get; set; } = null!; // ✔ obligatorio
        public string? IpOrigen { get; set; }                // 🔥 nullable

        public DateTime FechaEventoUtc { get; set; }

        public bool Activo { get; set; }

        public byte[] RowVersion { get; set; }
    }
}