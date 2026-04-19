using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Reserva;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class ReservaValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "PEN", // Pendiente
            "CON", // Confirmada
            "CAN", // Cancelada
            "EXP", // Expirada
            "FIN", // Finalizada
            "EMI"  // Emitida
        };

        private static readonly string[] CANALES_VALIDOS =
        {
            "WEB",
            "APP",
            "BOOKING",
            "TELEFONO",
            "PRESENCIAL"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearReservaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // IDs
            if (request.IdCliente <= 0)
                errors.Add("El cliente es obligatorio");

            if (request.IdPasajero <= 0)
                errors.Add("El pasajero es obligatorio");

            if (request.IdVuelo <= 0)
                errors.Add("El vuelo es obligatorio");

            if (request.IdAsiento <= 0)
                errors.Add("El asiento es obligatorio");

            // Fechas
            if (request.FechaInicio == default)
                errors.Add("La fecha de inicio es obligatoria");

            if (request.FechaFin == default)
                errors.Add("La fecha de fin es obligatoria");

            if (request.FechaInicio != default &&
                request.FechaFin != default &&
                request.FechaInicio > request.FechaFin)
                errors.Add("La fecha de inicio no puede ser mayor a la fecha de fin");

            // Valores
            if (request.SubtotalReserva < 0)
                errors.Add("El subtotal no puede ser negativo");

            if (request.ValorIva < 0)
                errors.Add("El IVA no puede ser negativo");

            if (request.TotalReserva < 0)
                errors.Add("El total no puede ser negativo");

            // 💀 Regla clave
            var totalEsperado = request.SubtotalReserva + request.ValorIva;

            if (request.TotalReserva != totalEsperado)
                errors.Add("El total debe ser igual a Subtotal + IVA");

            // Canal
            if (!string.IsNullOrWhiteSpace(request.OrigenCanalReserva) &&
                !CANALES_VALIDOS.Contains(request.OrigenCanalReserva))
                errors.Add("Canal inválido");

            // Email
            if (!string.IsNullOrWhiteSpace(request.ContactoEmail) &&
                !request.ContactoEmail.Contains("@"))
                errors.Add("El email no es válido");

            // Observaciones
            if (!string.IsNullOrWhiteSpace(request.Observaciones) &&
                request.Observaciones.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarReservaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Fechas
            if (request.FechaInicio.HasValue &&
                request.FechaFin.HasValue &&
                request.FechaInicio > request.FechaFin)
                errors.Add("La fecha de inicio no puede ser mayor a la fecha de fin");

            // Valores
            if (request.SubtotalReserva.HasValue && request.SubtotalReserva < 0)
                errors.Add("El subtotal no puede ser negativo");

            if (request.ValorIva.HasValue && request.ValorIva < 0)
                errors.Add("El IVA no puede ser negativo");

            if (request.TotalReserva.HasValue && request.TotalReserva < 0)
                errors.Add("El total no puede ser negativo");

            // Email
            if (!string.IsNullOrWhiteSpace(request.ContactoEmail) &&
                !request.ContactoEmail.Contains("@"))
                errors.Add("El email no es válido");

            // Observaciones
            if (!string.IsNullOrWhiteSpace(request.Observaciones) &&
                request.Observaciones.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ESTADO
        // ============================================================
        public static void ValidarEstado(ActualizarEstadoReservaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            if (string.IsNullOrWhiteSpace(request.EstadoReserva))
                errors.Add("El estado es obligatorio");
            else if (!ESTADOS_VALIDOS.Contains(request.EstadoReserva))
                errors.Add("Estado inválido");

            // 💀 REGLA CRÍTICA
            if (request.EstadoReserva == "CAN" &&
                string.IsNullOrWhiteSpace(request.MotivoCancelacion))
                errors.Add("Debe indicar el motivo de cancelación");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}