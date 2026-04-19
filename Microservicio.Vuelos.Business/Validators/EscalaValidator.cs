using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Escala;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class EscalaValidator
    {
        private static readonly string[] TIPOS_VALIDOS =
        {
            "TECNICA",
            "COMERCIAL"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearEscalaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Aeropuerto
            if (request.IdAeropuerto <= 0)
                errors.Add("El aeropuerto es obligatorio");

            // Orden
            if (request.Orden <= 0)
                errors.Add("El orden debe ser mayor o igual a 1");

            // Fechas
            if (request.FechaHoraLlegada == default)
                errors.Add("La fecha de llegada es obligatoria");

            if (request.FechaHoraSalida == default)
                errors.Add("La fecha de salida es obligatoria");

            if (request.FechaHoraLlegada != default &&
                request.FechaHoraSalida != default &&
                request.FechaHoraLlegada > request.FechaHoraSalida)
                errors.Add("La llegada no puede ser posterior a la salida");

            // Duración
            if (request.DuracionMin < 0)
                errors.Add("La duración no puede ser negativa");

            // Tipo
            if (!string.IsNullOrWhiteSpace(request.TipoEscala) &&
                !TIPOS_VALIDOS.Contains(request.TipoEscala))
                errors.Add("El tipo de escala debe ser TECNICA o COMERCIAL");

            // Textos
            if (!string.IsNullOrWhiteSpace(request.Terminal) &&
                request.Terminal.Length > 20)
                errors.Add("La terminal no puede superar 20 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Puerta) &&
                request.Puerta.Length > 10)
                errors.Add("La puerta no puede superar 10 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Observaciones) &&
                request.Observaciones.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarEscalaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Orden
            if (request.Orden.HasValue && request.Orden <= 0)
                errors.Add("El orden debe ser mayor o igual a 1");

            // Fechas coherentes (solo si vienen ambas)
            if (request.FechaHoraLlegada.HasValue &&
                request.FechaHoraSalida.HasValue &&
                request.FechaHoraLlegada > request.FechaHoraSalida)
                errors.Add("La llegada no puede ser posterior a la salida");

            // Duración
            if (request.DuracionMin.HasValue && request.DuracionMin < 0)
                errors.Add("La duración no puede ser negativa");

            // Tipo
            if (!string.IsNullOrWhiteSpace(request.TipoEscala) &&
                !TIPOS_VALIDOS.Contains(request.TipoEscala))
                errors.Add("El tipo de escala debe ser TECNICA o COMERCIAL");

            // Textos
            if (!string.IsNullOrWhiteSpace(request.Terminal) &&
                request.Terminal.Length > 20)
                errors.Add("La terminal no puede superar 20 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Puerta) &&
                request.Puerta.Length > 10)
                errors.Add("La puerta no puede superar 10 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Observaciones) &&
                request.Observaciones.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}