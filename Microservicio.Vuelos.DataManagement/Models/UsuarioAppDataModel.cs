using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class UsuarioAppDataModel
    {
        public int IdUsuario { get; set; }

        public Guid UsuarioGuid { get; set; }

        // 🔗 Relación (nullable ✔)
        public int? IdCliente { get; set; }

        // 👤 Datos
        public string Username { get; set; }
        public string Correo { get; set; }

        public DateTime? FechaUltimoLogin { get; set; }

        // 📊 Estado
        public string EstadoUsuario { get; set; }

        public bool Activo { get; set; }
    }
}