using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Rol;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class RolValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACT",
            "INA"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearRolRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Nombre
            if (string.IsNullOrWhiteSpace(request.NombreRol))
                errors.Add("El nombre del rol es obligatorio");
            else if (request.NombreRol.Length > 50)
                errors.Add("El nombre del rol no puede superar 50 caracteres");

            // Descripción
            if (!string.IsNullOrWhiteSpace(request.DescripcionRol) &&
                request.DescripcionRol.Length > 200)
                errors.Add("La descripción no puede superar 200 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarRolRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Nombre
            if (!string.IsNullOrWhiteSpace(request.NombreRol) &&
                request.NombreRol.Length > 50)
                errors.Add("El nombre del rol no puede superar 50 caracteres");

            // Descripción
            if (!string.IsNullOrWhiteSpace(request.DescripcionRol) &&
                request.DescripcionRol.Length > 200)
                errors.Add("La descripción no puede superar 200 caracteres");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.EstadoRol) &&
                !ESTADOS_VALIDOS.Contains(request.EstadoRol))
                errors.Add("El estado debe ser ACT o INA");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}