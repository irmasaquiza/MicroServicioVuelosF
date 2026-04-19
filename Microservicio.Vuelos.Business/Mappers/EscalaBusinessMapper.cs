using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Escala;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class EscalaBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static EscalaDataModel ToDataModel(CrearEscalaRequest request)
        {
            if (request == null) return null;

            return new EscalaDataModel
            {
                IdAeropuerto = request.IdAeropuerto,
                Orden = request.Orden,
                FechaHoraLlegada = request.FechaHoraLlegada,
                FechaHoraSalida = request.FechaHoraSalida,
                DuracionMin = request.DuracionMin,
                TipoEscala = request.TipoEscala,
                Terminal = request.Terminal,
                Puerta = request.Puerta,
                Observaciones = request.Observaciones
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static EscalaDataModel ToDataModel(ActualizarEscalaRequest request)
        {
            if (request == null) return null;

            var model = new EscalaDataModel
            {
                TipoEscala = request.TipoEscala,
                Terminal = request.Terminal,
                Puerta = request.Puerta,
                Observaciones = request.Observaciones
            };

            // 🔥 nullable fields
            if (request.Orden.HasValue)
                model.Orden = request.Orden.Value;

            if (request.FechaHoraLlegada.HasValue)
                model.FechaHoraLlegada = request.FechaHoraLlegada.Value;

            if (request.FechaHoraSalida.HasValue)
                model.FechaHoraSalida = request.FechaHoraSalida.Value;

            if (request.DuracionMin.HasValue)
                model.DuracionMin = request.DuracionMin.Value;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static EscalaResponse ToResponse(EscalaDataModel model)
        {
            if (model == null) return null;

            return new EscalaResponse
            {
                IdEscala = model.IdEscala,
                IdVuelo = model.IdVuelo,
                IdAeropuerto = model.IdAeropuerto,
                Orden = model.Orden,
                FechaHoraLlegada = model.FechaHoraLlegada,
                FechaHoraSalida = model.FechaHoraSalida,
                DuracionMin = model.DuracionMin,
                TipoEscala = model.TipoEscala,
                Terminal = model.Terminal,
                Puerta = model.Puerta,
                Observaciones = model.Observaciones
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<EscalaResponse> ToResponseList(IEnumerable<EscalaDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}