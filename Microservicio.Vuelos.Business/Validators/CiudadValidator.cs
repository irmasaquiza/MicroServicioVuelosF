using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Ciudad;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class CiudadValidator
    {
        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearCiudadRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // País
            if (request.IdPais <= 0)
                errors.Add("El país es obligatorio");

            // Nombre
            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre de la ciudad es obligatorio");

            if (!string.IsNullOrWhiteSpace(request.Nombre) &&
                request.Nombre.Length > 100)
                errors.Add("El nombre no puede superar 100 caracteres");

            // Zona horaria
            if (!string.IsNullOrWhiteSpace(request.ZonaHoraria) &&
                request.ZonaHoraria.Length > 50)
                errors.Add("La zona horaria no puede superar 50 caracteres");

            // Latitud
            if (request.Latitud.HasValue &&
                (request.Latitud < -90 || request.Latitud > 90))
                errors.Add("La latitud debe estar entre -90 y 90");

            // Longitud
            if (request.Longitud.HasValue &&
                (request.Longitud < -180 || request.Longitud > 180))
                errors.Add("La longitud debe estar entre -180 y 180");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarCiudadRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Nombre
            if (!string.IsNullOrWhiteSpace(request.Nombre) &&
                request.Nombre.Length > 100)
                errors.Add("El nombre no puede superar 100 caracteres");

            // Zona horaria
            if (!string.IsNullOrWhiteSpace(request.ZonaHoraria) &&
                request.ZonaHoraria.Length > 50)
                errors.Add("La zona horaria no puede superar 50 caracteres");

            // Latitud
            if (request.Latitud.HasValue &&
                (request.Latitud < -90 || request.Latitud > 90))
                errors.Add("La latitud debe estar entre -90 y 90");

            // Longitud
            if (request.Longitud.HasValue &&
                (request.Longitud < -180 || request.Longitud > 180))
                errors.Add("La longitud debe estar entre -180 y 180");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                request.Estado != "ACTIVO" &&
                request.Estado != "INACTIVO")
                errors.Add("El estado debe ser ACTIVO o INACTIVO");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}