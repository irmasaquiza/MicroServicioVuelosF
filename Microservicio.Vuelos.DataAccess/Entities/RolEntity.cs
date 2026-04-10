using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class RolEntity
    {
        public int IdRol { get; set; }

        public Guid RolGuid { get; set; }

        public string NombreRol { get; set; }   // VARCHAR(50)
        public string? DescripcionRol { get; set; } // VARCHAR(200) NULL

        public string EstadoRol { get; set; }   // CHAR(3) -> ACT / INA
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }

        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones
        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; } = new List<UsuarioRolEntity>();
    }
}