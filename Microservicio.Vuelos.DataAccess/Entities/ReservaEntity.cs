using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class ReservaEntity
    {
        public int IdReserva { get; set; }

        public Guid GuidReserva { get; set; }

        public string CodigoReserva { get; set; } = null!;

        // 🔗 Relaciones (FKs)
        public int IdCliente { get; set; }
        public int IdPasajero { get; set; }
        public int IdVuelo { get; set; }
        public int IdAsiento { get; set; }

        // 📅 Fechas
        public DateTime FechaReservaUtc { get; set; }
        public DateTime FechaInicio { get; set; }
        public DateTime FechaFin { get; set; }

        public DateTime? FechaConfirmacionUtc { get; set; }
        public DateTime? FechaCancelacionUtc { get; set; }

        // 💰 Valores
        public decimal SubtotalReserva { get; set; }
        public decimal ValorIva { get; set; }
        public decimal TotalReserva { get; set; }

        // ⚙️ Estado
        public string EstadoReserva { get; set; } = null!;
        public string OrigenCanalReserva { get; set; } = null!;

        public string? MotivoCancelacion { get; set; }

        // 📞 Contacto
        public string? ContactoEmail { get; set; }
        public string? ContactoTelefono { get; set; }
        public string? Observaciones { get; set; }

        // 🔌 Integración
        public string ServicioOrigen { get; set; } = null!;

        // ⚠️ Inhabilitación
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string? MotivoInhabilitacion { get; set; }

        // 🧾 Auditoría
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }

        public byte[] RowVersion { get; set; }

        // ============================================================
        // 🔥 NAVIGATION PROPERTIES
        // ============================================================

        public virtual ClienteEntity? Cliente { get; set; }
        public virtual PasajeroEntity? Pasajero { get; set; }
        public virtual VueloEntity? Vuelo { get; set; }
        public virtual AsientoEntity? Asiento { get; set; }

        public virtual ICollection<BoletoEntity> Boletos { get; set; } = new List<BoletoEntity>();
        public virtual ICollection<FacturaEntity> Facturas { get; set; } = new List<FacturaEntity>();
    }
}