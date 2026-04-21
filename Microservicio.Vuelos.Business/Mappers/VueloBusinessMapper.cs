using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Vuelo;
using Microservicio.Vuelos.Business.DTOs.Internal.Escala;
using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class VueloBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static VueloDataModel ToDataModel(CrearVueloRequest request)
        {
            if (request == null) return null;

            return new VueloDataModel
            {
                IdAeropuertoOrigen = request.IdAeropuertoOrigen,
                IdAeropuertoDestino = request.IdAeropuertoDestino,

                // 🔥 BD usa numero_vuelo → en model es CodigoVuelo
                CodigoVuelo = request.NumeroVuelo?.ToUpper(),

                FechaHoraSalida = request.FechaHoraSalida,
                FechaHoraLlegada = request.FechaHoraLlegada,
                DuracionMin = request.DuracionMin,
                PrecioBase = request.PrecioBase,
                CapacidadTotal = request.CapacidadTotal,

                // ⚠️ SIN CapacidadDisponible (no existe en BD)
                EstadoVuelo = "PROGRAMADO",
                Estado = "ACTIVO"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static VueloDataModel ToDataModel(ActualizarVueloRequest request)
        {
            if (request == null) return null;

            var model = new VueloDataModel();

            if (!string.IsNullOrWhiteSpace(request.NumeroVuelo))
                model.CodigoVuelo = request.NumeroVuelo.ToUpper();

            if (request.FechaHoraSalida.HasValue)
                model.FechaHoraSalida = request.FechaHoraSalida.Value;

            if (request.FechaHoraLlegada.HasValue)
                model.FechaHoraLlegada = request.FechaHoraLlegada.Value;

            if (request.DuracionMin.HasValue)
                model.DuracionMin = request.DuracionMin.Value;

            if (request.PrecioBase.HasValue)
                model.PrecioBase = request.PrecioBase.Value;

            if (request.CapacidadTotal.HasValue)
                model.CapacidadTotal = request.CapacidadTotal.Value;

            return model;
        }

        // ============================================================
        // 🔄 Estado → DataModel
        // ============================================================
        public static VueloDataModel ToDataModel(ActualizarEstadoVueloRequest request)
        {
            if (request == null) return null;

            return new VueloDataModel
            {
                EstadoVuelo = request.EstadoVuelo
            };
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static VueloResponse ToResponse(VueloDataModel model)
        {
            if (model == null) return null;

            return new VueloResponse
            {
                IdVuelo = model.IdVuelo,
                IdAeropuertoOrigen = model.IdAeropuertoOrigen,
                IdAeropuertoDestino = model.IdAeropuertoDestino,

                // 🔥 inverso
                NumeroVuelo = model.CodigoVuelo,

                FechaHoraSalida = model.FechaHoraSalida,
                FechaHoraLlegada = model.FechaHoraLlegada,
                DuracionMin = model.DuracionMin,
                PrecioBase = model.PrecioBase,
                CapacidadTotal = model.CapacidadTotal,

                // ⚠️ SIN CapacidadDisponible
                EstadoVuelo = model.EstadoVuelo,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Detalle
        // ============================================================
        public static VueloDetalleResponse ToDetalleResponse(
            VueloDataModel model,
            IEnumerable<EscalaDataModel> escalas,
            IEnumerable<AsientoDataModel> asientos)
        {
            if (model == null) return null;

            return new VueloDetalleResponse
            {
                IdVuelo = model.IdVuelo,
                IdAeropuertoOrigen = model.IdAeropuertoOrigen,
                IdAeropuertoDestino = model.IdAeropuertoDestino,
                NumeroVuelo = model.CodigoVuelo,
                FechaHoraSalida = model.FechaHoraSalida,
                FechaHoraLlegada = model.FechaHoraLlegada,
                DuracionMin = model.DuracionMin,
                PrecioBase = model.PrecioBase,
                CapacidadTotal = model.CapacidadTotal,

                // ⚠️ SIN CapacidadDisponible
                EstadoVuelo = model.EstadoVuelo,
                Estado = model.Estado,

                Escalas = escalas?.Select(EscalaBusinessMapper.ToResponse),
                Asientos = asientos?.Select(AsientoBusinessMapper.ToResponse)
            };
        }

        // ============================================================
        // 🔄 Lista
        // ============================================================
        public static IEnumerable<VueloResponse> ToResponseList(IEnumerable<VueloDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}