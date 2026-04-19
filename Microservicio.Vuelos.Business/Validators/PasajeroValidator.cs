using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Pasajero;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class PasajeroValidator
    {
        private static readonly string[] TIPOS_DOCUMENTO =
        {
            "CEDULA",
            "PASAPORTE",
            "RUC",
            "OTRO"
        };

        private static readonly string[] GENEROS_VALIDOS =
        {
            "MASCULINO",
            "FEMENINO",
            "OTRO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearPasajeroRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Nombre
            if (string.IsNullOrWhiteSpace(request.NombrePasajero))
                errors.Add("El nombre del pasajero es obligatorio");
            else if (request.NombrePasajero.Length > 100)
                errors.Add("El nombre no puede superar 100 caracteres");

            // Apellido
            if (string.IsNullOrWhiteSpace(request.ApellidoPasajero))
                errors.Add("El apellido del pasajero es obligatorio");
            else if (request.ApellidoPasajero.Length > 100)
                errors.Add("El apellido no puede superar 100 caracteres");

            // Tipo documento
            if (string.IsNullOrWhiteSpace(request.TipoDocumentoPasajero))
                errors.Add("El tipo de documento es obligatorio");
            else if (!TIPOS_DOCUMENTO.Contains(request.TipoDocumentoPasajero))
                errors.Add("Tipo de documento inválido");

            // Número documento
            if (string.IsNullOrWhiteSpace(request.NumeroDocumentoPasajero))
                errors.Add("El número de documento es obligatorio");
            else if (request.NumeroDocumentoPasajero.Length > 30)
                errors.Add("El documento no puede superar 30 caracteres");

            // Nacionalidad
            if (!string.IsNullOrWhiteSpace(request.NacionalidadPasajero) &&
                request.NacionalidadPasajero.Length > 80)
                errors.Add("La nacionalidad no puede superar 80 caracteres");

            // Email
            if (!string.IsNullOrWhiteSpace(request.EmailContactoPasajero) &&
                !request.EmailContactoPasajero.Contains("@"))
                errors.Add("El email no tiene un formato válido");

            // Género
            if (!string.IsNullOrWhiteSpace(request.GeneroPasajero) &&
                !GENEROS_VALIDOS.Contains(request.GeneroPasajero))
                errors.Add("El género debe ser MASCULINO, FEMENINO u OTRO");

            // Observaciones
            if (!string.IsNullOrWhiteSpace(request.ObservacionesPasajero) &&
                request.ObservacionesPasajero.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarPasajeroRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            if (!string.IsNullOrWhiteSpace(request.NombrePasajero) &&
                request.NombrePasajero.Length > 100)
                errors.Add("El nombre no puede superar 100 caracteres");

            if (!string.IsNullOrWhiteSpace(request.ApellidoPasajero) &&
                request.ApellidoPasajero.Length > 100)
                errors.Add("El apellido no puede superar 100 caracteres");

            if (!string.IsNullOrWhiteSpace(request.TipoDocumentoPasajero) &&
                !TIPOS_DOCUMENTO.Contains(request.TipoDocumentoPasajero))
                errors.Add("Tipo de documento inválido");

            if (!string.IsNullOrWhiteSpace(request.NumeroDocumentoPasajero) &&
                request.NumeroDocumentoPasajero.Length > 30)
                errors.Add("El documento no puede superar 30 caracteres");

            if (!string.IsNullOrWhiteSpace(request.NacionalidadPasajero) &&
                request.NacionalidadPasajero.Length > 80)
                errors.Add("La nacionalidad no puede superar 80 caracteres");

            if (!string.IsNullOrWhiteSpace(request.EmailContactoPasajero) &&
                !request.EmailContactoPasajero.Contains("@"))
                errors.Add("El email no tiene un formato válido");

            if (!string.IsNullOrWhiteSpace(request.GeneroPasajero) &&
                !GENEROS_VALIDOS.Contains(request.GeneroPasajero))
                errors.Add("El género debe ser MASCULINO, FEMENINO u OTRO");

            if (!string.IsNullOrWhiteSpace(request.ObservacionesPasajero) &&
                request.ObservacionesPasajero.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}