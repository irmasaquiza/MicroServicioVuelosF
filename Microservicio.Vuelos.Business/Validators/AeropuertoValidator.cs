using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class AeropuertoValidator
    {
        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearAeropuertoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // IATA
            if (string.IsNullOrWhiteSpace(request.CodigoIata))
                errors.Add("El código IATA es obligatorio");

            if (!string.IsNullOrWhiteSpace(request.CodigoIata) && request.CodigoIata.Length != 3)
                errors.Add("El código IATA debe tener exactamente 3 caracteres");

            // ICAO
            if (!string.IsNullOrWhiteSpace(request.CodigoIcao) && request.CodigoIcao.Length != 4)
                errors.Add("El código ICAO debe tener exactamente 4 caracteres");

            // Nombre
            if (string.IsNullOrWhiteSpace(request.Nombre))
                errors.Add("El nombre del aeropuerto es obligatorio");

            // País
            if (request.IdPais <= 0)
                errors.Add("El país es obligatorio");

            // Ciudad
            if (request.IdCiudad.HasValue && request.IdCiudad <= 0)
                errors.Add("La ciudad debe ser válida");

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
        public static void ValidarActualizar(ActualizarAeropuertoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // IATA
            if (!string.IsNullOrWhiteSpace(request.CodigoIata) &&
                request.CodigoIata.Length != 3)
                errors.Add("El código IATA debe tener exactamente 3 caracteres");

            // ICAO
            if (!string.IsNullOrWhiteSpace(request.CodigoIcao) &&
                request.CodigoIcao.Length != 4)
                errors.Add("El código ICAO debe tener exactamente 4 caracteres");

            // Nombre
            if (!string.IsNullOrWhiteSpace(request.Nombre) &&
                request.Nombre.Length > 150)
                errors.Add("El nombre no puede superar 150 caracteres");

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