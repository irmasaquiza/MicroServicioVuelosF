using System;
using System.Collections.Generic;
using System.Text;

 
namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class MetodoPagoEntity
    {
        public int IdMetodo { get; set; }

        public byte[] RowVersion { get; set; }

        public int IdCliente { get; set; }
        public int IdTipoMetodo { get; set; }

        public string Ultimos4 { get; set; }
        public string ReferenciaVisible { get; set; }
        public string TokenPasarela { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        public string NombreTitular { get; set; }
        public string MarcaTarjeta { get; set; }
        public string BancoEmisor { get; set; }
        public string PaisEmision { get; set; }

        public bool EsPrincipal { get; set; }
        public string Alias { get; set; }

        public DateTime? FechaUltimoUso { get; set; }

        public string Estado { get; set; } // ACTIVO, EXPIRADO, BLOQUEADO
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Relaciones
        public virtual ClienteEntity Cliente { get; set; }
        public virtual TipoMetodoPagoEntity TipoMetodoPago { get; set; }

        public virtual ICollection<FacturaEntity> Facturas { get; set; }
    }
}