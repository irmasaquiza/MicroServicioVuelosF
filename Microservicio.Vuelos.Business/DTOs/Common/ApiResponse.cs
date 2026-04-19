using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// ApiResponse.cs
// Wrapper genérico para respuestas exitosas de un solo objeto
// { data: T }
// ============================================================
namespace Microservicio.Vuelos.Business.DTOs.Common
{
    /// <summary>
    /// Wrapper genérico para respuestas exitosas de un
    /// solo objeto (GET por ID, POST, PUT).
    /// Ejemplo de JSON resultante:
    /// {
    ///   "data": { ...objeto... }
    /// }
    /// </summary>
    public class ApiResponse<T>
    {
        // ─────────────────────────────────────────────
        // El objeto de datos retornado
        // ─────────────────────────────────────────────
        public T Data { get; set; }

        // ─────────────────────────────────────────────
        // Constructor vacío
        // ─────────────────────────────────────────────
        public ApiResponse() { }

        // ─────────────────────────────────────────────
        // Constructor con data
        // ─────────────────────────────────────────────
        public ApiResponse(T data)
        {
            Data = data;
        }

        // ─────────────────────────────────────────────
        // Factory — forma rápida de construir la respuesta
        // ─────────────────────────────────────────────
        public static ApiResponse<T> Ok(T data)
        {
            return new ApiResponse<T>(data);
        }
    }
}