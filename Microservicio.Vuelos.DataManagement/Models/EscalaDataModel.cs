using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class EscalaDataModel
    {
        public int IdEscala { get; set; }

        // 🔗 Relaciones
        public int IdVuelo { get; set; }
        public int IdAeropuerto { get; set; }

        // ✈️ Datos de la escala
        public int Orden { get; set; }

        public DateTime FechaHoraLlegada { get; set; }
        public DateTime FechaHoraSalida { get; set; }

        public int DuracionMin { get; set; }

        public string TipoEscala { get; set; } // TECNICA, COMERCIAL

        public string Terminal { get; set; }
        public string Puerta { get; set; }

        public string Observaciones { get; set; }

        // 📊 Estado
        public string Estado { get; set; }
    }
}