using System;
using System.Collections.Generic;
using System.Text;

// ============================================================
// NotFoundException.cs
// ============================================================
using System;

namespace Microservicio.Vuelos.Business.Exceptions
{
    /// <summary>
    /// Se lanza cuando un recurso solicitado no existe o
    /// fue eliminado lógicamente.
    /// El Controller debe retornar HTTP 404 Not Found.
    /// Ejemplo: buscar un vuelo con ID que no existe,
    ///          obtener un cliente eliminado, etc.
    /// </summary>
    public class NotFoundException : Exception
    {
        // ─────────────────────────────────────────────
        // Código de error — siempre "RECURSO_NO_ENCONTRADO"
        // salvo que se especifique uno más específico
        // ─────────────────────────────────────────────
        public string Codigo { get; }

        // ─────────────────────────────────────────────
        // Nombre del recurso que no se encontró
        // Ej: "Vuelo", "Cliente", "Reserva"
        // ─────────────────────────────────────────────
        public string Recurso { get; }

        // ─────────────────────────────────────────────
        // Identificador buscado — para el mensaje de detalle
        // ─────────────────────────────────────────────
        public object Identificador { get; }

        // ─────────────────────────────────────────────
        // Constructor mínimo — solo mensaje
        // ─────────────────────────────────────────────
        public NotFoundException(string mensaje)
            : base(mensaje)
        {
            Codigo = "RECURSO_NO_ENCONTRADO";
            Recurso = null;
            Identificador = null;
        }

        // ─────────────────────────────────────────────
        // Constructor con recurso e identificador
        // Genera el mensaje automáticamente
        // ─────────────────────────────────────────────
        public NotFoundException(string recurso, object identificador)
            : base($"El recurso '{recurso}' con identificador '{identificador}' no fue encontrado.")
        {
            Codigo = "RECURSO_NO_ENCONTRADO";
            Recurso = recurso;
            Identificador = identificador;
        }

        // ─────────────────────────────────────────────
        // Constructor con código personalizado
        // Ej: "VUELO_NO_ENCONTRADO", "CLIENTE_NO_ENCONTRADO"
        // ─────────────────────────────────────────────
        public NotFoundException(string codigo, string recurso, object identificador)
            : base($"El recurso '{recurso}' con identificador '{identificador}' no fue encontrado.")
        {
            Codigo = codigo;
            Recurso = recurso;
            Identificador = identificador;
        }

        // ─────────────────────────────────────────────
        // Constructor con código y mensaje personalizado
        // ─────────────────────────────────────────────
        public NotFoundException(string codigo, string recurso,
            object identificador, string mensaje)
            : base(mensaje)
        {
            Codigo = codigo;
            Recurso = recurso;
            Identificador = identificador;
        }
    }
}