using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class ReservaDataModel
    {
        public int IdReserva { get; set; }

        public Guid GuidReserva { get; set; }

        public string CodigoReserva { get; set; }

        // 🔗 Relaciones
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
        public string EstadoReserva { get; set; }

        public string OrigenCanalReserva { get; set; }

        public string MotivoCancelacion { get; set; }

        // 📞 Contacto
        public string ContactoEmail { get; set; }
        public string ContactoTelefono { get; set; }

        public string Observaciones { get; set; }

        // 🔌 Integración
        public string ServicioOrigen { get; set; }

        // ⚠️ Inhabilitación
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; }
    }
}