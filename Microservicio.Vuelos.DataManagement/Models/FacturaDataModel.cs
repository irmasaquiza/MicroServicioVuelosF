using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class FacturaDataModel
    {
        public int IdFactura { get; set; }

        public Guid GuidFactura { get; set; }

        // 🔗 Relaciones
        public int IdCliente { get; set; }
        public int IdReserva { get; set; }
        public int IdMetodo { get; set; }

        // 🧾 Datos factura
        public string NumeroFactura { get; set; }
        public DateTime FechaEmision { get; set; }

        // 💰 Valores económicos
        public decimal Subtotal { get; set; }
        public decimal ValorIva { get; set; }
        public decimal CargoServicio { get; set; }
        public decimal Total { get; set; }

        // 📊 Información adicional
        public string ObservacionesFactura { get; set; }
        public string OrigenCanalFactura { get; set; }

        // 📊 Estado
        public string Estado { get; set; }

        public DateTime? FechaInhabilitacionUtc { get; set; }

        // 🔗 Integración
        public string ServicioOrigen { get; set; }

        public string MotivoInhabilitacion { get; set; }
    }
}