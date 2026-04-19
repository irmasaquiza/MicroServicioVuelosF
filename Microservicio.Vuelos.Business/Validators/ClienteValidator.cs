using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Cliente;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class ClienteValidator
    {
        private static readonly string[] TIPOS_IDENTIFICACION =
        {
            "CEDULA",
            "PASAPORTE",
            "RUC",
            "TARJETA_IDENTIDAD",
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
        public static void ValidarCrear(CrearClienteRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Tipo identificación
            if (string.IsNullOrWhiteSpace(request.TipoIdentificacion))
                errors.Add("El tipo de identificación es obligatorio");
            else if (!TIPOS_IDENTIFICACION.Contains(request.TipoIdentificacion))
                errors.Add("Tipo de identificación inválido");

            // Número identificación
            if (string.IsNullOrWhiteSpace(request.NumeroIdentificacion))
                errors.Add("El número de identificación es obligatorio");
            else if (request.NumeroIdentificacion.Length > 30)
                errors.Add("La identificación no puede superar 30 caracteres");

            // Nombres
            if (!string.IsNullOrWhiteSpace(request.Nombres) &&
                request.Nombres.Length > 160)
                errors.Add("Los nombres no pueden superar 160 caracteres");

            // Apellidos
            if (!string.IsNullOrWhiteSpace(request.Apellidos) &&
                request.Apellidos.Length > 160)
                errors.Add("Los apellidos no pueden superar 160 caracteres");

            // Razón social
            if (!string.IsNullOrWhiteSpace(request.RazonSocial) &&
                request.RazonSocial.Length > 200)
                errors.Add("La razón social no puede superar 200 caracteres");

            // Correo
            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio");
            else if (!request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido");

            // Teléfono
            if (string.IsNullOrWhiteSpace(request.Telefono))
                errors.Add("El teléfono es obligatorio");

            // Dirección
            if (string.IsNullOrWhiteSpace(request.Direccion))
                errors.Add("La dirección es obligatoria");

            // Ciudad
            if (request.IdCiudadResidencia <= 0)
                errors.Add("La ciudad de residencia es obligatoria");

            // País
            if (request.IdPaisNacionalidad <= 0)
                errors.Add("El país de nacionalidad es obligatorio");

            // Nacionalidad
            if (!string.IsNullOrWhiteSpace(request.Nacionalidad) &&
                request.Nacionalidad.Length > 80)
                errors.Add("La nacionalidad no puede superar 80 caracteres");

            // Género
            if (!string.IsNullOrWhiteSpace(request.Genero) &&
                !GENEROS_VALIDOS.Contains(request.Genero))
                errors.Add("El género debe ser MASCULINO, FEMENINO u OTRO");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarClienteRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            if (!string.IsNullOrWhiteSpace(request.Nombres) &&
                request.Nombres.Length > 160)
                errors.Add("Los nombres no pueden superar 160 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Apellidos) &&
                request.Apellidos.Length > 160)
                errors.Add("Los apellidos no pueden superar 160 caracteres");

            if (!string.IsNullOrWhiteSpace(request.RazonSocial) &&
                request.RazonSocial.Length > 200)
                errors.Add("La razón social no puede superar 200 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Correo) &&
                !request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido");

            if (!string.IsNullOrWhiteSpace(request.Nacionalidad) &&
                request.Nacionalidad.Length > 80)
                errors.Add("La nacionalidad no puede superar 80 caracteres");

            if (request.IdCiudadResidencia.HasValue &&
                request.IdCiudadResidencia <= 0)
                errors.Add("La ciudad debe ser válida");

            if (request.IdPaisNacionalidad.HasValue &&
                request.IdPaisNacionalidad <= 0)
                errors.Add("El país debe ser válido");

            if (!string.IsNullOrWhiteSpace(request.Genero) &&
                !GENEROS_VALIDOS.Contains(request.Genero))
                errors.Add("El género debe ser MASCULINO, FEMENINO u OTRO");

            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                request.Estado != "ACT" &&
                request.Estado != "INA")
                errors.Add("El estado debe ser ACT o INA");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}