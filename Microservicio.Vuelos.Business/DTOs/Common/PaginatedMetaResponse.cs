using System;
using System.Collections.Generic;
using System.Text;
// ============================================================
// PaginatedMetaResponse.cs
// Coincide exactamente con el schema PaginatedMeta del YAML
// { page, page_size, total, total_pages }
// ============================================================
namespace Microservicio.Vuelos.Business.DTOs.Common
{
    /// <summary>
    /// Metadatos de paginación incluidos en toda respuesta
    /// de tipo lista paginada.
    /// Coincide con el schema PaginatedMeta del contrato YAML.
    /// </summary>
    public class PaginatedMetaResponse
    {
        // ─────────────────────────────────────────────
        // Página actual solicitada
        // ─────────────────────────────────────────────
        public int Page { get; set; }

        // ─────────────────────────────────────────────
        // Tamaño de página — registros por página
        // ─────────────────────────────────────────────
        public int PageSize { get; set; }

        // ─────────────────────────────────────────────
        // Total de registros que existen con los filtros
        // ─────────────────────────────────────────────
        public int Total { get; set; }

        // ─────────────────────────────────────────────
        // Total de páginas calculado
        // ─────────────────────────────────────────────
        public int TotalPages { get; set; }

        // ─────────────────────────────────────────────
        // Propiedades de navegación — útiles en el
        // Controller para construir links de paginación
        // ─────────────────────────────────────────────
        public bool TienePaginaAnterior => Page > 1;
        public bool TienePaginaSiguiente => Page < TotalPages;

        // ─────────────────────────────────────────────
        // Constructor vacío
        // ─────────────────────────────────────────────
        public PaginatedMetaResponse() { }

        // ─────────────────────────────────────────────
        // Constructor completo
        // ─────────────────────────────────────────────
        public PaginatedMetaResponse(int page, int pageSize,
            int total, int totalPages)
        {
            Page = page;
            PageSize = pageSize;
            Total = total;
            TotalPages = totalPages;
        }

        // ─────────────────────────────────────────────
        // Factory — desde el DataPagedResult de la Capa 2
        // ─────────────────────────────────────────────
        public static PaginatedMetaResponse FromMeta(
            DataManagement.Models.MetaData meta)
        {
            if (meta == null) return null;

            return new PaginatedMetaResponse(
                meta.Page,
                meta.PageSize,
                meta.Total,
                meta.TotalPages
            );
        }
    }
}