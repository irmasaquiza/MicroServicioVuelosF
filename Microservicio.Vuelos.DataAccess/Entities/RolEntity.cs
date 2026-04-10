using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class RolEntity
    {
        public int IdRol { get; set; }

        public string NombreRol { get; set; }   // ADMIN, CLIENTE, AGENTE
        public string DescripcionRol { get; set; }

        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        // 🔗 Relaciones
        public virtual ICollection<UsuarioRolEntity> UsuariosRoles { get; set; }
    }
}