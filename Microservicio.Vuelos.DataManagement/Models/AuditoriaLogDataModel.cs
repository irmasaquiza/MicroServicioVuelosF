using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class AuditoriaLogDataModel
    {
        public long IdAuditoria { get; set; }

        public Guid AuditoriaGuid { get; set; }

        public string TablaAfectada { get; set; }
        public string Operacion { get; set; }

        public string IdRegistroAfectado { get; set; }

        public string DatosAnteriores { get; set; }
        public string DatosNuevos { get; set; }

        public string UsuarioEjecutor { get; set; }
        public string IpOrigen { get; set; }

        public DateTime FechaEventoUtc { get; set; }

        public bool Activo { get; set; }
    }
}