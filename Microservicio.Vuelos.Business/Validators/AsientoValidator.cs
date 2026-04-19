using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class AsientoValidator
    {
        // Catálogos válidos
        private static readonly string[] CLASES_VALIDAS =
            { "ECONOMICA", "EJECUTIVA", "PRIMERA" };

        private static readonly string[] POSICIONES_VALIDAS =
            { "VENTANA", "PASILLO", "CENTRO" };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearAsientoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Número de asiento
            if (string.IsNullOrWhiteSpace(request.NumeroAsiento))
                errors.Add("El número de asiento es obligatorio");

            if (!string.IsNullOrWhiteSpace(request.NumeroAsiento) &&
                request.NumeroAsiento.Length > 5)
                errors.Add("El número de asiento no puede superar 5 caracteres");

            // Clase
            if (!string.IsNullOrWhiteSpace(request.Clase) &&
                !CLASES_VALIDAS.Contains(request.Clase))
                errors.Add("La clase debe ser ECONOMICA, EJECUTIVA o PRIMERA");

            // Precio extra
            if (request.PrecioExtra < 0)
                errors.Add("El precio extra no puede ser negativo");

            // Posición
            if (!string.IsNullOrWhiteSpace(request.Posicion) &&
                !POSICIONES_VALIDAS.Contains(request.Posicion))
                errors.Add("La posición debe ser VENTANA, PASILLO o CENTRO");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarAsientoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Número
            if (!string.IsNullOrWhiteSpace(request.NumeroAsiento) &&
                request.NumeroAsiento.Length > 5)
                errors.Add("El número de asiento no puede superar 5 caracteres");

            // Clase
            if (!string.IsNullOrWhiteSpace(request.Clase) &&
                !CLASES_VALIDAS.Contains(request.Clase))
                errors.Add("La clase debe ser ECONOMICA, EJECUTIVA o PRIMERA");

            // Precio
            if (request.PrecioExtra.HasValue && request.PrecioExtra < 0)
                errors.Add("El precio extra no puede ser negativo");

            // Posición
            if (!string.IsNullOrWhiteSpace(request.Posicion) &&
                !POSICIONES_VALIDAS.Contains(request.Posicion))
                errors.Add("La posición debe ser VENTANA, PASILLO o CENTRO");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}