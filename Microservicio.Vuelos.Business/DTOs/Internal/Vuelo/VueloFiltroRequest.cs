using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// Vuelo/VueloFiltroRequest.cs
// ============================================================
namespace Microservicio.Vuelos.Business.DTOs.Internal.Vuelo
{
    public class VueloFiltroRequest
    {
        public string? NumeroVuelo { get; set; }

 
    public int? IdAeropuertoOrigen { get; set; }

        public int? IdAeropuertoDestino { get; set; }

        public string? EstadoVuelo { get; set; }

        public DateTime? FechaSalidaInicio { get; set; }

        public DateTime? FechaSalidaFin { get; set; }

        public decimal? PrecioMin { get; set; }

        public decimal? PrecioMax { get; set; }

        public int? CapacidadDisponibleMin { get; set; }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 20 : value;
        }
    }
 

}
