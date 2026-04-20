using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class FacturaEntity
    {
        public int IdFactura { get; set; }

        public Guid GuidFactura { get; set; }

        // 🔗 Relaciones
        public int IdCliente { get; set; }
        public int IdReserva { get; set; }
        public int IdMetodo { get; set; }

        // Datos factura
        public string NumeroFactura { get; set; } = null!;
        public DateTime FechaEmision { get; set; }

        // Valores económicos
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal CargoServicio { get; set; }
        public decimal Total { get; set; }

        public string? ObservacionesFactura { get; set; }   // 🔥 nullable
        public string? OrigenCanalFactura { get; set; }     // 🔥 nullable

        // Estado
        public string Estado { get; set; } = null!;         // ABI, APR, INA
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }   // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }         // 🔥 nullable

        // Integración
        public string? ServicioOrigen { get; set; }         // 🔥 nullable

        // Opcionales
        public string? MotivoInhabilitacion { get; set; }   // 🔥 nullable

        public byte[] RowVersion { get; set; }

        // 🔗 Navigation Properties
        public virtual ClienteEntity? Cliente { get; set; }     // 🔥 nullable
        public virtual ReservaEntity? Reserva { get; set; }     // 🔥 nullable
        public virtual MetodoPagoEntity? MetodoPago { get; set; } // 🔥 nullable

        public virtual ICollection<BoletoEntity> Boletos { get; set; } = new List<BoletoEntity>();
    }
}