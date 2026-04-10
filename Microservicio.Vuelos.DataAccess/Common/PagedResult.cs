using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Models
{
    public class PagedResult<T>
    {
        // 📦 Datos
        public IEnumerable<T> Items { get; set; }

        // 🔢 Paginación
        public int TotalRegistros { get; set; }
        public int TotalPaginas { get; set; }
        public int PaginaActual { get; set; }
        public int TamanoPagina { get; set; }

        // 📊 Extras útiles
        public bool TienePaginaAnterior => PaginaActual > 1;
        public bool TienePaginaSiguiente => PaginaActual < TotalPaginas;

        // 🏗️ Constructor vacío
        public PagedResult()
        {
            Items = new List<T>();
        }

        // 🏗️ Constructor completo (pro)
        public PagedResult(IEnumerable<T> items, int totalRegistros, int paginaActual, int tamanoPagina)
        {
            Items = items;
            TotalRegistros = totalRegistros;
            PaginaActual = paginaActual;
            TamanoPagina = tamanoPagina;

            TotalPaginas = (int)Math.Ceiling(totalRegistros / (double)tamanoPagina);
        }
    }
}
