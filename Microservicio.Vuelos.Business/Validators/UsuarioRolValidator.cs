using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class UsuarioRolValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACT",
            "INA"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearUsuarioRolRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Rol
            if (request.IdRol <= 0)
                errors.Add("El rol es obligatorio");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarUsuarioRolRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.EstadoUsuarioRol) &&
                !ESTADOS_VALIDOS.Contains(request.EstadoUsuarioRol))
                errors.Add("El estado debe ser ACT o INA");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}