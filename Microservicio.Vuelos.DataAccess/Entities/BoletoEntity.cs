using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class BoletoEntity
    {
        public int IdBoleto { get; set; }

        public byte[] RowVersion { get; set; }

        public int IdReserva { get; set; }
        public int IdVuelo { get; set; }
        public int IdAsiento { get; set; }
        public int IdFactura { get; set; }

        public string CodigoBoleto { get; set; } = null!;   // ✔ obligatorio

        public string Clase { get; set; } = null!;          // ✔ obligatorio

        public decimal PrecioVueloBase { get; set; }
        public decimal PrecioAsientoExtra { get; set; }
        public decimal ImpuestosBoleto { get; set; }
        public decimal CargoEquipaje { get; set; }
        public decimal PrecioFinal { get; set; }

        public string EstadoBoleto { get; set; } = null!;   // ✔ obligatorio

        public DateTime FechaEmision { get; set; }

        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;         // ✔ obligatorio

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }   // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }         // 🔥 nullable

        // 🔗 Relaciones
        public virtual ReservaEntity? Reserva { get; set; }   // 🔥 nullable
        public virtual VueloEntity? Vuelo { get; set; }       // 🔥 nullable
        public virtual AsientoEntity? Asiento { get; set; }   // 🔥 nullable
        public virtual FacturaEntity? Factura { get; set; }   // 🔥 nullable
    }
}