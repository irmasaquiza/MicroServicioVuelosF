using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class UsuarioAppValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACT",
            "INA"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearUsuarioAppRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Username
            if (string.IsNullOrWhiteSpace(request.Username))
                errors.Add("El username es obligatorio");
            else if (request.Username.Length > 50)
                errors.Add("El username no puede superar 50 caracteres");

            // Correo
            if (string.IsNullOrWhiteSpace(request.Correo))
                errors.Add("El correo es obligatorio");
            else if (!request.Correo.Contains("@"))
                errors.Add("El correo no tiene un formato válido");
            else if (request.Correo.Length > 120)
                errors.Add("El correo no puede superar 120 caracteres");

            // Password
            if (string.IsNullOrWhiteSpace(request.Password))
                errors.Add("La contraseña es obligatoria");
            else if (request.Password.Length < 8)
                errors.Add("La contraseña debe tener al menos 8 caracteres");

            // Cliente opcional
            if (request.IdCliente.HasValue && request.IdCliente <= 0)
                errors.Add("El cliente debe ser válido");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarUsuarioAppRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Username
            if (!string.IsNullOrWhiteSpace(request.Username) &&
                request.Username.Length > 50)
                errors.Add("El username no puede superar 50 caracteres");

            // Correo
            if (!string.IsNullOrWhiteSpace(request.Correo))
            {
                if (!request.Correo.Contains("@"))
                    errors.Add("El correo no tiene un formato válido");

                if (request.Correo.Length > 120)
                    errors.Add("El correo no puede superar 120 caracteres");
            }

            // Estado
            if (!string.IsNullOrWhiteSpace(request.EstadoUsuario) &&
                !ESTADOS_VALIDOS.Contains(request.EstadoUsuario))
                errors.Add("El estado debe ser ACT o INA");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}