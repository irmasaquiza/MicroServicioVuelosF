using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class VueloFiltroDataModel
    {
        // 🔍 Filtros
        public string CodigoVuelo { get; set; }

        public int? IdAeropuertoOrigen { get; set; }
        public int? IdAeropuertoDestino { get; set; }

        public string EstadoVuelo { get; set; }

        public string TipoVuelo { get; set; }

        public string Aerolinea { get; set; }

        public string Terminal { get; set; }

        // 📅 Fechas
        public DateTime? FechaSalidaInicio { get; set; }
        public DateTime? FechaSalidaFin { get; set; }

        // 💰 Precio
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }

        // 🪑 Disponibilidad
        public int? CapacidadDisponibleMin { get; set; }

        public string Estado { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}