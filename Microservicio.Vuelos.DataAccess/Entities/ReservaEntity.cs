using System;
using System.Collections.Generic;
using System.Text;
 
namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class ReservaEntity
    {
        public int IdReserva { get; set; }

        public Guid ReservaGuid { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones
        public int IdCliente { get; set; }
        public int IdPasajero { get; set; }

        // Datos de reserva
        public string CodigoReserva { get; set; } // PNR

        public DateTime FechaReserva { get; set; }
        public DateTime? FechaLimitePago { get; set; }

        public string EstadoReserva { get; set; } // CREADA, PAGADA, CANCELADA, EXPIRADA

        public decimal TotalReserva { get; set; }

        public string ObservacionesReserva { get; set; }

        public string CanalOrigen { get; set; } // WEB, APP, AGENCIA

        // Estado técnico
        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Navigation Properties

        public virtual ClienteEntity Cliente { get; set; }
        public virtual PasajeroEntity Pasajero { get; set; }

        public virtual ICollection<BoletoEntity> Boletos { get; set; }
        public virtual ICollection<FacturaEntity> Facturas { get; set; }
    }
}