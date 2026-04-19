using System;
using System.Collections.Generic;
using System.Text;
// ============================================================
// PaginatedResponse.cs
// Wrapper para respuestas de listas paginadas
// { data: T[], meta: { page, page_size, total, total_pages } }
// ============================================================
using System.Collections.Generic;

namespace Microservicio.Vuelos.Business.DTOs.Common
{
    /// <summary>
    /// Wrapper genérico para respuestas paginadas (listas).
    /// Coincide con la estructura del contrato YAML:
    /// {
    ///   "data": [ ...lista... ],
    ///   "meta": {
    ///     "page": 1,
    ///     "page_size": 20,
    ///     "total": 150,
    ///     "total_pages": 8
    ///   }
    /// }
    /// </summary>
    public class PaginatedResponse<T>
    {
        // ─────────────────────────────────────────────
        // Lista de items de la página actual
        // ─────────────────────────────────────────────
        public IEnumerable<T> Data { get; set; }

        // ─────────────────────────────────────────────
        // Metadatos de paginación
        // ─────────────────────────────────────────────
        public PaginatedMetaResponse Meta { get; set; }

        // ─────────────────────────────────────────────
        // Constructor vacío
        // ─────────────────────────────────────────────
        public PaginatedResponse() { }

        // ─────────────────────────────────────────────
        // Constructor completo
        // ─────────────────────────────────────────────
        public PaginatedResponse(
            IEnumerable<T> data,
            PaginatedMetaResponse meta)
        {
            Data = data;
            Meta = meta;
        }

        // ─────────────────────────────────────────────
        // Factory — desde DataPagedResult de la Capa 2
        // El mapper de cada módulo convierte los DataModels
        // en Response DTOs antes de llegar aquí
        // ─────────────────────────────────────────────
        public static PaginatedResponse<T> FromPagedResult(
            IEnumerable<T> data,
            DataManagement.Models.MetaData meta)
        {
            return new PaginatedResponse<T>(
                data,
                PaginatedMetaResponse.FromMeta(meta)
            );
        }

        // ─────────────────────────────────────────────
        // Factory — forma rápida con meta ya convertida
        // ─────────────────────────────────────────────
        public static PaginatedResponse<T> Ok(
            IEnumerable<T> data,
            PaginatedMetaResponse meta)
        {
            return new PaginatedResponse<T>(data, meta);
        }
    }
}