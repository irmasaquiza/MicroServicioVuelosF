using System;
using System.Collections.Generic;
using System.Text;

 
namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class UsuarioAppEntity
    {
        public int IdUsuario { get; set; }

        public Guid UsuarioGuid { get; set; }

        public int IdCliente { get; set; }

        public string Username { get; set; }
        public string Email { get; set; }

        public string PasswordHash { get; set; }

        public bool EmailVerificado { get; set; }

        public int IntentosFallidos { get; set; }
        public DateTime? FechaBloqueoUtc { get; set; }

        public DateTime? UltimoLoginUtc { get; set; }

        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones

        public virtual ClienteEntity Cliente { get; set; }

        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; }
    }
}