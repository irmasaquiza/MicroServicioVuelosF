using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class EscalaEntity
    {
        public int IdEscala { get; set; }

        public byte[] RowVersion { get; set; }

        public int IdVuelo { get; set; }
        public int IdAeropuerto { get; set; }

        public int Orden { get; set; }

        public DateTime FechaHoraLlegada { get; set; }
        public DateTime FechaHoraSalida { get; set; }

        public int DuracionMin { get; set; }

        public string TipoEscala { get; set; } = null!;   // ✔ obligatorio (TECNICA, COMERCIAL)

        public string? Terminal { get; set; }             // 🔥 nullable
        public string? Puerta { get; set; }               // 🔥 nullable

        public string? Observaciones { get; set; }        // 🔥 nullable

        public string Estado { get; set; } = null!;
        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; } = null!;

        public string? ModificadoPorUsuario { get; set; } // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }       // 🔥 nullable

        // 🔗 Relaciones
        public virtual VueloEntity? Vuelo { get; set; }         // 🔥 nullable
        public virtual AeropuertoEntity? Aeropuerto { get; set; } // 🔥 nullable
    }
}