using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp
{
    public class UsuarioAppFiltroRequest
    {
        public int? IdCliente { get; set; }

        public string? Username { get; set; }

        public string? Correo { get; set; }

        public string? EstadoUsuario { get; set; }

        public bool? Activo { get; set; }

        public DateTime? UltimoLoginInicio { get; set; }

        public DateTime? UltimoLoginFin { get; set; }

        private int _page = 1;
        public int Page
        {
            get => _page;
            set => _page = value <= 0 ? 1 : value;
        }

        private int _pageSize = 20;
        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = value <= 0 ? 20 : value;
        }
    }
}