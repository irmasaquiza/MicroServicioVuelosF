using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Vuelo;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class VueloValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "PROGRAMADO",
            "EN_VUELO",
            "ATERRIZADO",
            "CANCELADO",
            "DEMORADO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearVueloRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Aeropuertos
            if (request.IdAeropuertoOrigen <= 0)
                errors.Add("El aeropuerto de origen es obligatorio");

            if (request.IdAeropuertoDestino <= 0)
                errors.Add("El aeropuerto de destino es obligatorio");

            if (request.IdAeropuertoOrigen == request.IdAeropuertoDestino)
                errors.Add("El aeropuerto de origen y destino no pueden ser iguales");

            // Número vuelo
            if (string.IsNullOrWhiteSpace(request.NumeroVuelo))
                errors.Add("El número de vuelo es obligatorio");
            else if (request.NumeroVuelo.Length > 10)
                errors.Add("El número de vuelo no puede superar 10 caracteres");

            // Fechas
            if (request.FechaHoraSalida == default)
                errors.Add("La fecha de salida es obligatoria");

            if (request.FechaHoraLlegada == default)
                errors.Add("La fecha de llegada es obligatoria");

            if (request.FechaHoraSalida != default &&
                request.FechaHoraLlegada != default &&
                request.FechaHoraSalida >= request.FechaHoraLlegada)
                errors.Add("La fecha de salida debe ser menor que la fecha de llegada");

            // Duración
            if (request.DuracionMin < 0)
                errors.Add("La duración no puede ser negativa");

            // Precio
            if (request.PrecioBase <= 0)
                errors.Add("El precio base debe ser mayor a 0");

            // Capacidad
            if (request.CapacidadTotal <= 0)
                errors.Add("La capacidad total debe ser mayor a 0");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarVueloRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Número vuelo
            if (!string.IsNullOrWhiteSpace(request.NumeroVuelo) &&
                request.NumeroVuelo.Length > 10)
                errors.Add("El número de vuelo no puede superar 10 caracteres");

            // Fechas
            if (request.FechaHoraSalida.HasValue &&
                request.FechaHoraLlegada.HasValue &&
                request.FechaHoraSalida >= request.FechaHoraLlegada)
                errors.Add("La fecha de salida debe ser menor que la fecha de llegada");

            // Duración
            if (request.DuracionMin.HasValue && request.DuracionMin < 0)
                errors.Add("La duración no puede ser negativa");

            // Precio
            if (request.PrecioBase.HasValue && request.PrecioBase <= 0)
                errors.Add("El precio base debe ser mayor a 0");

            // Capacidad
            if (request.CapacidadTotal.HasValue && request.CapacidadTotal <= 0)
                errors.Add("La capacidad total debe ser mayor a 0");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ESTADO
        // ============================================================
        public static void ValidarEstado(ActualizarEstadoVueloRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            if (string.IsNullOrWhiteSpace(request.EstadoVuelo))
                errors.Add("El estado del vuelo es obligatorio");
            else if (!ESTADOS_VALIDOS.Contains(request.EstadoVuelo))
                errors.Add("Estado de vuelo inválido");

            // 💀 regla crítica
            if (request.EstadoVuelo == "CANCELADO" &&
                string.IsNullOrWhiteSpace(request.Motivo))
                errors.Add("Debe indicar el motivo de cancelación");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}