using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Vuelo/VueloResponse.cs
// Coincide con schema Vuelo del contrato YAML
// ============================================================

namespace Microservicio.Vuelos.Business.DTOs.Internal.Vuelo
{
    public class VueloResponse
    {
        public int IdVuelo { get; set; }

 
    public int IdAeropuertoOrigen { get; set; }

        public int IdAeropuertoDestino { get; set; }

        public string NumeroVuelo { get; set; } = string.Empty;

        public DateTime FechaHoraSalida { get; set; }

        public DateTime FechaHoraLlegada { get; set; }

        public int DuracionMin { get; set; }

        public decimal PrecioBase { get; set; }

        public int CapacidadTotal { get; set; }

        public int CapacidadDisponible { get; set; }

        public string EstadoVuelo { get; set; } = string.Empty;

        public string Estado { get; set; } = string.Empty;
    }
 

}
