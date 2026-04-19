using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.Business.DTOs.Internal.Pais
{
    public class PaisFiltroRequest
    {
        public string? Nombre { get; set; }

     public string? CodigoIso2 { get; set; }

        public string? CodigoIso3 { get; set; }

        public string? Continente { get; set; }

        // ACTIVO / INACTIVO
        public string? Estado { get; set; }

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
