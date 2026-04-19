using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol
{
    public class UsuarioRolFiltroRequest
    {
        public int? IdUsuario { get; set; }

        public int? IdRol { get; set; }

        public string? EstadoUsuarioRol { get; set; }

        public bool? Activo { get; set; }

        public DateTime? FechaRegistroInicio { get; set; }

        public DateTime? FechaRegistroFin { get; set; }

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