using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class VueloDataModel
    {
        public int IdVuelo { get; set; }

        // ✈️ Datos principales
        public string CodigoVuelo { get; set; }

        public int IdAeropuertoOrigen { get; set; }
        public int IdAeropuertoDestino { get; set; }

        public DateTime FechaHoraSalida { get; set; }
        public DateTime FechaHoraLlegada { get; set; }

        public int DuracionMin { get; set; }

        // 📊 Estado
        public string EstadoVuelo { get; set; }

        public string TipoVuelo { get; set; } // NACIONAL / INTERNACIONAL

        // 🪑 Capacidad
        public int CapacidadTotal { get; set; }

        // 💰 Precio
        public decimal PrecioBase { get; set; }

        // ✈️ Info adicional
        public string Aerolinea { get; set; }

        public string NumeroGate { get; set; }
        public string Terminal { get; set; }

        public string Observaciones { get; set; }

        public string Estado { get; set; }
    }
}