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

        public string Tipo { get; set; } // MANO, BODEGA

        public decimal PesoKg { get; set; }

        public string DescripcionEquipaje { get; set; }

        public decimal PrecioExtra { get; set; }

        public string DimensionesCm { get; set; }

        public string NumeroEtiqueta { get; set; }

        public string EstadoEquipaje { get; set; }

        public bool EsEliminado { get; set; }

        public string Estado { get; set; }

        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Relación
        public virtual BoletoEntity Boleto { get; set; }
    }
}