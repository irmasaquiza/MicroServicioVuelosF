using System;
using System.Collections.Generic;
using System.Text;
 
namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class UsuarioRolEntity
    {
        // 🔑 Clave compuesta
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        public DateTime FechaAsignacion { get; set; }

        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        // 🔗 Relaciones
        public virtual UsuarioAppEntity Usuario { get; set; }
        public virtual RolEntity Rol { get; set; }
    }
}