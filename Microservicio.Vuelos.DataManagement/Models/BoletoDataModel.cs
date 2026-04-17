using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class BoletoDataModel
    {
        public int IdBoleto { get; set; }

        // 🔗 Relaciones
        public int IdReserva { get; set; }
        public int IdVuelo { get; set; }
        public int IdAsiento { get; set; }
        public int IdFactura { get; set; }

        // 🎟️ Datos del boleto
        public string CodigoBoleto { get; set; }
        public string Clase { get; set; }

        // 💰 Valores
        public decimal PrecioVueloBase { get; set; }
        public decimal PrecioAsientoExtra { get; set; }
        public decimal ImpuestosBoleto { get; set; }
        public decimal CargoEquipaje { get; set; }
        public decimal PrecioFinal { get; set; }

        // 📊 Estado
        public string EstadoBoleto { get; set; }

        public DateTime FechaEmision { get; set; }
    }
}