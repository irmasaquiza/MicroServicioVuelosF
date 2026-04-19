using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.TipoMetodoPago;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class TipoMetodoPagoValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACTIVO",
            "INACTIVO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearTipoMetodoPagoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Nombre
            if (string.IsNullOrWhiteSpace(request.NombreTipo))
                errors.Add("El nombre del tipo de método es obligatorio");
            else if (request.NombreTipo.Length > 50)
                errors.Add("El nombre no puede superar 50 caracteres");

            // Descripción
            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 150)
                errors.Add("La descripción no puede superar 150 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarTipoMetodoPagoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Nombre
            if (!string.IsNullOrWhiteSpace(request.NombreTipo) &&
                request.NombreTipo.Length > 50)
                errors.Add("El nombre no puede superar 50 caracteres");

            // Descripción
            if (!string.IsNullOrWhiteSpace(request.Descripcion) &&
                request.Descripcion.Length > 150)
                errors.Add("La descripción no puede superar 150 caracteres");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                !ESTADOS_VALIDOS.Contains(request.Estado))
                errors.Add("El estado debe ser ACTIVO o INACTIVO");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}