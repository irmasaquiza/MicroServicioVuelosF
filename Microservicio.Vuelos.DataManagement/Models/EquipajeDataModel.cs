using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class EquipajeDataModel
    {
        public int IdEquipaje { get; set; }

        // 🔗 Relación
        public int IdBoleto { get; set; }

        // 🧳 Datos del equipaje
        public string Tipo { get; set; } // MANO, BODEGA

        public decimal PesoKg { get; set; }

        public string DescripcionEquipaje { get; set; }

        public decimal PrecioExtra { get; set; }

        public string DimensionesCm { get; set; }

        public string NumeroEtiqueta { get; set; }

        public string EstadoEquipaje { get; set; }

        // 📊 Estado general
        public string Estado { get; set; }
    }
}
