using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class VueloEntity
    {
        public int IdVuelo { get; set; }

        public byte[] RowVersion { get; set; }

        public string CodigoVuelo { get; set; } = null!;

        public int IdAeropuertoOrigen { get; set; }
        public int IdAeropuertoDestino { get; set; }

        public DateTime FechaHoraSalida { get; set; }
        public DateTime FechaHoraLlegada { get; set; }

        public int DuracionMin { get; set; }

        // ⚠️ SOLO lo que EXISTE en tu BD
        public decimal PrecioBase { get; set; }
        public int CapacidadTotal { get; set; }

        public string EstadoVuelo { get; set; } = null!;

        public string Estado { get; set; } = null!;
        public bool EsEliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; } = null!;

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }

        // 🔗 Relaciones (nullable para evitar crashes)
        public virtual AeropuertoEntity? AeropuertoOrigen { get; set; }
        public virtual AeropuertoEntity? AeropuertoDestino { get; set; }

        public virtual ICollection<EscalaEntity> Escalas { get; set; } = new List<EscalaEntity>();
        public virtual ICollection<BoletoEntity> Boletos { get; set; } = new List<BoletoEntity>();
        public virtual ICollection<AsientoEntity> Asientos { get; set; } = new List<AsientoEntity>();
    }
}