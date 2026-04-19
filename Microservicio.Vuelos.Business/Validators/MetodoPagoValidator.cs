using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class MetodoPagoValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACTIVO",
            "EXPIRADO",
            "BLOQUEADO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearMetodoPagoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Cliente
            if (request.IdCliente <= 0)
                errors.Add("El cliente es obligatorio");

            // Tipo método
            if (request.IdTipoMetodo <= 0)
                errors.Add("El tipo de método es obligatorio");

            // Token
            if (string.IsNullOrWhiteSpace(request.TokenPasarela))
                errors.Add("El token de la pasarela es obligatorio");

            // Últimos 4
            if (!string.IsNullOrWhiteSpace(request.Ultimos4))
            {
                if (request.Ultimos4.Length != 4 || !request.Ultimos4.All(char.IsDigit))
                    errors.Add("Los últimos 4 deben ser exactamente 4 dígitos");
            }

            // Fecha expiración
            if (request.FechaExpiracion.HasValue &&
                request.FechaExpiracion < DateTime.UtcNow.Date)
                errors.Add("La tarjeta ya está expirada");

            // Textos
            if (!string.IsNullOrWhiteSpace(request.NombreTitular) &&
                request.NombreTitular.Length > 150)
                errors.Add("El nombre del titular no puede superar 150 caracteres");

            if (!string.IsNullOrWhiteSpace(request.MarcaTarjeta) &&
                request.MarcaTarjeta.Length > 50)
                errors.Add("La marca de tarjeta no puede superar 50 caracteres");

            if (!string.IsNullOrWhiteSpace(request.BancoEmisor) &&
                request.BancoEmisor.Length > 100)
                errors.Add("El banco emisor no puede superar 100 caracteres");

            if (!string.IsNullOrWhiteSpace(request.PaisEmision) &&
                request.PaisEmision.Length > 50)
                errors.Add("El país no puede superar 50 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Alias) &&
                request.Alias.Length > 100)
                errors.Add("El alias no puede superar 100 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarMetodoPagoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                !ESTADOS_VALIDOS.Contains(request.Estado))
                errors.Add("El estado debe ser ACTIVO, EXPIRADO o BLOQUEADO");

            // Fecha expiración
            if (request.FechaExpiracion.HasValue &&
                request.FechaExpiracion < DateTime.UtcNow.Date)
                errors.Add("La tarjeta ya está expirada");

            // Textos
            if (!string.IsNullOrWhiteSpace(request.NombreTitular) &&
                request.NombreTitular.Length > 150)
                errors.Add("El nombre del titular no puede superar 150 caracteres");

            if (!string.IsNullOrWhiteSpace(request.MarcaTarjeta) &&
                request.MarcaTarjeta.Length > 50)
                errors.Add("La marca de tarjeta no puede superar 50 caracteres");

            if (!string.IsNullOrWhiteSpace(request.BancoEmisor) &&
                request.BancoEmisor.Length > 100)
                errors.Add("El banco emisor no puede superar 100 caracteres");

            if (!string.IsNullOrWhiteSpace(request.PaisEmision) &&
                request.PaisEmision.Length > 50)
                errors.Add("El país no puede superar 50 caracteres");

            if (!string.IsNullOrWhiteSpace(request.Alias) &&
                request.Alias.Length > 100)
                errors.Add("El alias no puede superar 100 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}