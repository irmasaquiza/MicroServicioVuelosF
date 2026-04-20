using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class RolEntity
    {
        public int IdRol { get; set; }

        public Guid RolGuid { get; set; }

        public string NombreRol { get; set; } = null!;        // ✔ obligatorio
        public string? DescripcionRol { get; set; }           // ✔ nullable

        public string EstadoRol { get; set; } = null!;        // ✔ ACT / INA
        public bool EsEliminado { get; set; }
        public bool Activo { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones
        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; } = new List<UsuarioRolEntity>();
    }
}