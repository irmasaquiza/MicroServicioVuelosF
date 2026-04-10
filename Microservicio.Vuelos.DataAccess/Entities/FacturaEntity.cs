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
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }

        // Valores económicos
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal CargoServicio { get; set; }
        public decimal Total { get; set; }

        public string ObservacionesFactura { get; set; }
        public string OrigenCanalFactura { get; set; }

        // Estado
        public string Estado { get; set; } // ABI, APR, INA
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // Integración
        public string ServicioOrigen { get; set; }

        // Opcionales
        public string MotivoInhabilitacion { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Navigation Properties
        public virtual ClienteEntity Cliente { get; set; }
        public virtual ReservaEntity Reserva { get; set; }
        public virtual MetodoPagoEntity MetodoPago { get; set; }

        public virtual ICollection<BoletoEntity> Boletos { get; set; }
    }
}