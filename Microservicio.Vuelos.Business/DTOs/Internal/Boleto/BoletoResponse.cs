using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Boleto
{
    public class BoletoResponse
    {
        public int IdBoleto { get; set; }

        public string CodigoBoleto { get; set; } = string.Empty;

        public int IdReserva { get; set; }

        public int IdVuelo { get; set; }

        public int? IdAsiento { get; set; }

        public int IdFactura { get; set; }

        public string Clase { get; set; } = string.Empty;

        public decimal PrecioVueloBase { get; set; }

        public decimal? PrecioAsientoExtra { get; set; }

        public decimal ImpuestosBoleto { get; set; }

        public decimal CargoEquipaje { get; set; }

        public decimal PrecioFinal { get; set; }

        public string EstadoBoleto { get; set; } = string.Empty;

        public DateTime FechaEmision { get; set; }
    }
}