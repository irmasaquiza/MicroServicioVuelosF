using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class EquipajeFiltroDataModel
    {
        // 🔍 Filtros
        public int? IdBoleto { get; set; }

        public string Tipo { get; set; }

        public string EstadoEquipaje { get; set; }

        public string NumeroEtiqueta { get; set; }

        // ⚖️ Rango de peso
        public decimal? PesoMin { get; set; }
        public decimal? PesoMax { get; set; }

        // 💰 Rango de precio
        public decimal? PrecioMin { get; set; }
        public decimal? PrecioMax { get; set; }

        public string Estado { get; set; }

        // 📄 Paginación
        public int Page { get; set; } = 1;
        public int PageSize { get; set; } = 20;
    }
}