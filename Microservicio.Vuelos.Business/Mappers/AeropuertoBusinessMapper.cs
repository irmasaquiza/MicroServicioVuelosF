using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class AeropuertoBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static AeropuertoDataModel ToDataModel(CrearAeropuertoRequest request)
        {
            if (request == null) return null;

            return new AeropuertoDataModel
            {
                CodigoIata = request.CodigoIata?.ToUpper(),
                CodigoIcao = request.CodigoIcao?.ToUpper(),
                Nombre = request.Nombre,
                IdCiudad = request.IdCiudad,
                IdPais = request.IdPais,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud,

                Estado = "ACTIVO"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel
        // ============================================================
        public static AeropuertoDataModel ToDataModel(ActualizarAeropuertoRequest request)
        {
            if (request == null) return null;

            return new AeropuertoDataModel
            {
                CodigoIata = request.CodigoIata?.ToUpper(),
                CodigoIcao = request.CodigoIcao?.ToUpper(),
                Nombre = request.Nombre,
                ZonaHoraria = request.ZonaHoraria,
                Latitud = request.Latitud,
                Longitud = request.Longitud,
                Estado = request.Estado
            };
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static AeropuertoResponse ToResponse(AeropuertoDataModel model)
        {
            if (model == null) return null;

            return new AeropuertoResponse
            {
                IdAeropuerto = model.IdAeropuerto,
                CodigoIata = model.CodigoIata,
                CodigoIcao = model.CodigoIcao,
                Nombre = model.Nombre,
                IdCiudad = model.IdCiudad,
                IdPais = model.IdPais,
                ZonaHoraria = model.ZonaHoraria,
                Latitud = model.Latitud,
                Longitud = model.Longitud,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<AeropuertoResponse> ToResponseList(IEnumerable<AeropuertoDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}