using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Vuelo/VueloDetalleResponse.cs
// Incluye escalas y asientos — para GET /vuelos/{id}
// ============================================================
using Microservicio.Vuelos.Business.DTOs.Internal.Escala;
using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Vuelo
{
    public class VueloDetalleResponse
    {
        public int IdVuelo { get; set; }
        public int IdAeropuertoOrigen { get; set; }
        public int IdAeropuertoDestino { get; set; }
        public string NumeroVuelo { get; set; }
        public DateTime FechaHoraSalida { get; set; }
        public DateTime FechaHoraLlegada { get; set; }
        public int DuracionMin { get; set; }
        public decimal PrecioBase { get; set; }
        public int CapacidadTotal { get; set; }
        public int CapacidadDisponible { get; set; }
        public string EstadoVuelo { get; set; }
        public string Estado { get; set; }

        // Escalas del vuelo ordenadas
        public IEnumerable<EscalaResponse> Escalas { get; set; }

        // Mapa de asientos del vuelo
        public IEnumerable<AsientoResponse> Asientos { get; set; }
    }
}