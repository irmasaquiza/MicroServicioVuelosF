/*ing System;
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

        public string? Ultimos4 { get; set; }              // 🔥 nullable (no aplica a todos los métodos)
        public string? ReferenciaVisible { get; set; }     // 🔥 nullable
        public string? TokenPasarela { get; set; }         // 🔥 nullable (depende de pasarela)

        public DateTime? FechaExpiracion { get; set; }

        public string? NombreTitular { get; set; }         // 🔥 nullable (no todos los métodos)
        public string? MarcaTarjeta { get; set; }          // 🔥 nullable
        public string? BancoEmisor { get; set; }           // 🔥 nullable
        public string? PaisEmision { get; set; }           // 🔥 nullable

        public bool EsPrincipal { get; set; }

        public string? Alias { get; set; }                 // 🔥 nullable

        public DateTime? FechaUltimoUso { get; set; }

        public string Estado { get; set; } = null!;        // ✔ obligatorio
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }  // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }        // 🔥 nullable

        // 🔗 Relaciones
        public virtual ClienteEntity? Cliente { get; set; }             // 🔥 nullable
        public virtual TipoMetodoPagoEntity? TipoMetodoPago { get; set; } // 🔥 nullable

        public virtual ICollection<FacturaEntity> Facturas { get; set; } = new List<FacturaEntity>();
    }
}*/