using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class AsientoBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static AsientoDataModel ToDataModel(CrearAsientoRequest request)
        {
            if (request == null) return null;

            return new AsientoDataModel
            {
                NumeroAsiento = request.NumeroAsiento?.ToUpper(),
                Clase = request.Clase,
                PrecioExtra = request.PrecioExtra,
                Posicion = request.Posicion,

                // Por defecto disponible
                Disponible = true
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel
        // ============================================================
        public static AsientoDataModel ToDataModel(ActualizarAsientoRequest request)
        {
            if (request == null) return null;

            var model = new AsientoDataModel
            {
                NumeroAsiento = request.NumeroAsiento?.ToUpper(),
                Clase = request.Clase,
                Posicion = request.Posicion
            };

            // 🔥 FIX 1
            if (request.Disponible.HasValue)
                model.Disponible = request.Disponible.Value;

            // 🔥 FIX 2
            if (request.PrecioExtra.HasValue)
                model.PrecioExtra = request.PrecioExtra.Value;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static AsientoResponse ToResponse(AsientoDataModel model)
        {
            if (model == null) return null;

            return new AsientoResponse
            {
                IdAsiento = model.IdAsiento,
                IdVuelo = model.IdVuelo,
                NumeroAsiento = model.NumeroAsiento,
                Clase = model.Clase,
                Disponible = model.Disponible,
                PrecioExtra = model.PrecioExtra,
                Posicion = model.Posicion
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<AsientoResponse> ToResponseList(IEnumerable<AsientoDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}