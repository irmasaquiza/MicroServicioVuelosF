using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class UsuarioRolEntity
    {
        public int IdUsuarioRol { get; set; } // 🔥 PK real

        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        public string EstadoUsuarioRol { get; set; }
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }

        // 🧾 Auditoría
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones
        public virtual UsuarioAppEntity Usuario { get; set; }
        public virtual RolEntity Rol { get; set; }
    }
}