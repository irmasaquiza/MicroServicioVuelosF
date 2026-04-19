using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp
{
    public class UsuarioAppResponse
    {
        public int IdUsuario { get; set; }

        public Guid UsuarioGuid { get; set; }

        public int? IdCliente { get; set; }

        public string Username { get; set; } = string.Empty;

        public string Correo { get; set; } = string.Empty;

        public DateTime? FechaUltimoLogin { get; set; }

        // ACT / INA
        public string EstadoUsuario { get; set; } = string.Empty;

        public bool Activo { get; set; }
    }
}