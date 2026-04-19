using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Pais;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class PaisValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACTIVO",
            "INACTIVO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearPaisRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // ISO2
            if (string.IsNullOrWhiteSpace(request.CodigoIso2))
                errors.Add("El código ISO2 es obligatorio");
            else if (request.CodigoIso2.Length != 2)
                errors.Add("El código ISO2 debe tener exactamente 2 caracteres");

            // ISO3
            if (!string.IsNullOrWhiteSpace(request.CodigoIso3) &&
                request.CodigoIso3.Length != 3)
                errors.Add("El código ISO3 debe tener exactamente 3 caracteres");

            // Nombre
            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre del país es obligatorio");
            else if (request.Nombre.Length > 100)
                errors.Add("El nombre no puede superar 100 caracteres");

            // Continente
            if (!string.IsNullOrWhiteSpace(request.Continente) &&
                request.Continente.Length > 50)
                errors.Add("El continente no puede superar 50 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarPaisRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // ISO2
            if (!string.IsNullOrWhiteSpace(request.CodigoIso2) &&
                request.CodigoIso2.Length != 2)
                errors.Add("El código ISO2 debe tener exactamente 2 caracteres");

            // ISO3
            if (!string.IsNullOrWhiteSpace(request.CodigoIso3) &&
                request.CodigoIso3.Length != 3)
                errors.Add("El código ISO3 debe tener exactamente 3 caracteres");

            // Nombre
            if (!string.IsNullOrWhiteSpace(request.Nombre) &&
                request.Nombre.Length > 100)
                errors.Add("El nombre no puede superar 100 caracteres");

            // Continente
            if (!string.IsNullOrWhiteSpace(request.Continente) &&
                request.Continente.Length > 50)
                errors.Add("El continente no puede superar 50 caracteres");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                !ESTADOS_VALIDOS.Contains(request.Estado))
                errors.Add("El estado debe ser ACTIVO o INACTIVO");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}