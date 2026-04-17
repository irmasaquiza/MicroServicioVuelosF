using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class UsuarioRolDataModel
    {
        public int IdUsuarioRol { get; set; } // 🔥 PK real

        // 🔗 Relaciones
        public int IdUsuario { get; set; }
        public int IdRol { get; set; }

        // 📊 Estado
        public string EstadoUsuarioRol { get; set; }

        public bool Activo { get; set; }
    }
}