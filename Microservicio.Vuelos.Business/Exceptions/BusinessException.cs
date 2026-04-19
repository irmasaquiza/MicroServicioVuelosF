using System;
using System.Collections.Generic;
using System.Text;

using System;

namespace Microservicio.Vuelos.Business.Exceptions
{
    /// <summary>
    /// Excepción base para la lógica de negocio.
    /// </summary>
    public class BusinessException : Exception
    {
        /// <summary>
        /// Código opcional del error
        /// </summary>
        public string CodigoError { get; }

        // Constructor vacío
        public BusinessException() : base()
        {
        }

        // Constructor con mensaje
        public BusinessException(string message) : base(message)
        {
        }

        // Constructor con mensaje y excepción interna
        public BusinessException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        // Constructor con código y mensaje
        public BusinessException(string codigoError, string message)
            : base(message)
        {
            CodigoError = codigoError;
        }
    }
}