using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class VueloEntity
    {
        public int IdVuelo { get; set; }

        public byte[] RowVersion { get; set; }

        public string CodigoVuelo { get; set; } // Ej: AV1234

        public int IdAeropuertoOrigen { get; set; }
        public int IdAeropuertoDestino { get; set; }

        public DateTime FechaHoraSalida { get; set; }
        public DateTime FechaHoraLlegada { get; set; }

        public int DuracionMin { get; set; }

        public string EstadoVuelo { get; set; } // PROGRAMADO, EN_CURSO, FINALIZADO, CANCELADO

        public string TipoVuelo { get; set; } // NACIONAL, INTERNACIONAL

        public int CapacidadTotal { get; set; }
        public int CapacidadDisponible { get; set; }

        public decimal PrecioBase { get; set; }

        public string Aerolinea { get; set; }

        public string NumeroGate { get; set; }
        public string Terminal { get; set; }

        public string Observaciones { get; set; }

        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Relaciones

        public virtual AeropuertoEntity AeropuertoOrigen { get; set; }
        public virtual AeropuertoEntity AeropuertoDestino { get; set; }

        public virtual ICollection<EscalaEntity> Escalas { get; set; }
        public virtual ICollection<BoletoEntity> Boletos { get; set; }
        public virtual ICollection<AsientoEntity> Asientos { get; set; }
    }
}