using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class UsuarioAppEntity
    {
        public int IdUsuario { get; set; }

        public Guid UsuarioGuid { get; set; }

        public int? IdCliente { get; set; } // 🔥 nullable

        public string Username { get; set; }
        public string Correo { get; set; }

        public string PasswordHash { get; set; }
        public string PasswordSalt { get; set; }

        public DateTime? FechaUltimoLogin { get; set; }

        public string EstadoUsuario { get; set; }
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }

        // 🧾 Auditoría
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones
        public virtual ClienteEntity? Cliente { get; set; }

        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; } = new List<UsuarioRolEntity>();
    }
}