using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// UnauthorizedBusinessException.cs
// ============================================================
using System;

namespace Microservicio.Vuelos.Business.Exceptions
{
    /// <summary>
    /// Se lanza cuando el usuario no tiene permisos para
    /// ejecutar la operación solicitada.
    /// El Controller debe retornar:
    ///   - HTTP 401 si no hay token / token inválido
    ///   - HTTP 403 si hay token válido pero sin permisos
    /// Ejemplo: intentar acceder a auditoría sin rol ADMIN,
    ///          modificar datos de otro cliente, etc.
    /// </summary>
    public class UnauthorizedBusinessException : Exception
    {
        // ─────────────────────────────────────────────
        // Código de error
        // "NO_AUTENTICADO"  → 401
        // "SIN_PERMISOS"    → 403
        // ─────────────────────────────────────────────
        public string Codigo { get; }

        // ─────────────────────────────────────────────
        // Indica si es falta de autenticación (401)
        // o falta de autorización (403)
        // ─────────────────────────────────────────────
        public bool EsNoAutenticado { get; }

        // ─────────────────────────────────────────────
        // Recurso u operación que se intentó acceder
        // ─────────────────────────────────────────────
        public string Recurso { get; }

        // ─────────────────────────────────────────────
        // Constructor — sin autenticación (401)
        // ─────────────────────────────────────────────
        public UnauthorizedBusinessException()
            : base("No se encuentra autenticado. Debe iniciar sesión.")
        {
            Codigo = "NO_AUTENTICADO";
            EsNoAutenticado = true;
            Recurso = null;
        }

        // ─────────────────────────────────────────────
        // Constructor — sin permisos (403)
        // ─────────────────────────────────────────────
        public UnauthorizedBusinessException(string recurso)
            : base($"No tiene permisos para acceder al recurso '{recurso}'.")
        {
            Codigo = "SIN_PERMISOS";
            EsNoAutenticado = false;
            Recurso = recurso;
        }

        // ─────────────────────────────────────────────
        // Constructor con código y mensaje personalizados
        // ─────────────────────────────────────────────
        public UnauthorizedBusinessException(string codigo, string mensaje,
            bool esNoAutenticado = false)
            : base(mensaje)
        {
            Codigo = codigo;
            EsNoAutenticado = esNoAutenticado;
            Recurso = null;
        }

        // ─────────────────────────────────────────────
        // Constructor completo
        // ─────────────────────────────────────────────
        public UnauthorizedBusinessException(string codigo, string recurso,
            string mensaje, bool esNoAutenticado = false)
            : base(mensaje)
        {
            Codigo = codigo;
            EsNoAutenticado = esNoAutenticado;
            Recurso = recurso;
        }
    }
}