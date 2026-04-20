using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class EquipajeEntity
    {
        public int IdEquipaje { get; set; }

        public byte[] RowVersion { get; set; }

        public int IdBoleto { get; set; }

        public string Tipo { get; set; } = null!;            // ✔ obligatorio (MANO, BODEGA)

        public decimal PesoKg { get; set; }

        public string? DescripcionEquipaje { get; set; }     // 🔥 nullable

        public decimal PrecioExtra { get; set; }

        public string? DimensionesCm { get; set; }           // 🔥 nullable

        public string? NumeroEtiqueta { get; set; }          // 🔥 nullable

        public string EstadoEquipaje { get; set; } = null!;  // ✔ obligatorio

        public bool EsEliminado { get; set; }

        public string Estado { get; set; } = null!;          // ✔ obligatorio

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }    // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }          // 🔥 nullable

        // 🔗 Relación
        public virtual BoletoEntity? Boleto { get; set; }    // 🔥 nullable
    }
}