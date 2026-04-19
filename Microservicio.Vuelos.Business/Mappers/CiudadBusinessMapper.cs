using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Ciudad;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class CiudadBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static CiudadDataModel ToDataModel(CrearCiudadRequest request)
        {
            if (request == null) return null;

            return new CiudadDataModel
            {
                IdPais = request.IdPais,
                Nombre = request.Nombre,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud,

                // valor por defecto
                Estado = "ACTIVO"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static CiudadDataModel ToDataModel(ActualizarCiudadRequest request)
        {
            if (request == null) return null;

            var model = new CiudadDataModel
            {
                Nombre = request.Nombre,
                ZonaHoraria = request.ZonaHoraria
            };

            // 🔥 nullable fields
            if (request.Latitud.HasValue)
                model.Latitud = request.Latitud.Value;

            if (request.Longitud.HasValue)
                model.Longitud = request.Longitud.Value;

            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static CiudadResponse ToResponse(CiudadDataModel model)
        {
            if (model == null) return null;

            return new CiudadResponse
            {
                IdCiudad = model.IdCiudad,
                IdPais = model.IdPais,
                Nombre = model.Nombre,
                ZonaHoraria = model.ZonaHoraria,
                Latitud = model.Latitud,
                Longitud = model.Longitud,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<CiudadResponse> ToResponseList(IEnumerable<CiudadDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}