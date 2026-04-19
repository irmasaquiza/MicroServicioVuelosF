using System;
using System.Collections.Generic;
using System.Linq;

namespace Microservicio.Vuelos.Business.Exceptions
{
    public class ValidationException : Exception
    {
        public string Codigo { get; }

        public Dictionary<string, string[]> Errores { get; }

        // Constructor simple
        public ValidationException(string mensaje)
            : base(mensaje)
        {
            Codigo = "VALIDACION_ERROR";
            Errores = new Dictionary<string, string[]>();
        }

        // 🔥 NUEVO → soporte para List<string>
        public ValidationException(List<string> errores)
            : base("Se produjeron uno o más errores de validación.")
        {
            Codigo = "VALIDACION_ERROR";

            Errores = new Dictionary<string, string[]>
            {
                { "General", errores.ToArray() }
            };
        }

        // Constructor con errores por campo
        public ValidationException(Dictionary<string, string[]> errores)
            : base("Se produjeron uno o más errores de validación.")
        {
            Codigo = "VALIDACION_ERROR";
            Errores = errores ?? new Dictionary<string, string[]>();
        }

        // Constructor completo
        public ValidationException(string codigo, string mensaje,
            Dictionary<string, string[]> errores)
            : base(mensaje)
        {
            Codigo = codigo;
            Errores = errores ?? new Dictionary<string, string[]>();
        }

        // Constructor para un solo error
        public ValidationException(string campo, string error)
            : base("Error de validación.")
        {
            Codigo = "VALIDACION_ERROR";

            Errores = new Dictionary<string, string[]>
            {
                { campo, new[] { error } }
            };
        }
    }
}