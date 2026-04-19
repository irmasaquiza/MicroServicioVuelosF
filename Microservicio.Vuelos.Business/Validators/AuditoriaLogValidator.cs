using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class AuditoriaLogValidator
    {
        private static readonly string[] OPERACIONES_VALIDAS =
        {
            "INSERT",
            "UPDATE",
            "DELETE"
        };

        // ============================================================
        // 🔥 VALIDAR FILTRO
        // ============================================================
        public static void ValidarFiltro(AuditoriaLogFiltroRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Tabla
            if (!string.IsNullOrWhiteSpace(request.TablaAfectada) &&
                request.TablaAfectada.Length > 100)
                errors.Add("El nombre de la tabla no puede superar 100 caracteres");

            // Operación
            if (!string.IsNullOrWhiteSpace(request.Operacion) &&
                !OPERACIONES_VALIDAS.Contains(request.Operacion))
                errors.Add("La operación debe ser INSERT, UPDATE o DELETE");

            // Usuario
            if (!string.IsNullOrWhiteSpace(request.UsuarioEjecutor) &&
                request.UsuarioEjecutor.Length > 100)
                errors.Add("El usuario no puede superar 100 caracteres");

            // Fechas
            if (request.FechaDesde.HasValue && request.FechaHasta.HasValue &&
                request.FechaDesde > request.FechaHasta)
                errors.Add("La fecha desde no puede ser mayor a la fecha hasta");

            // Paginación
            if (request.Page <= 0)
                errors.Add("La página debe ser mayor a 0");

            if (request.PageSize <= 0)
                errors.Add("El tamaño de página debe ser mayor a 0");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}