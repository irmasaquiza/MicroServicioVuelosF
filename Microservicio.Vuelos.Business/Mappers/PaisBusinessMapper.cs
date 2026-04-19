using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Pais;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class PaisBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static PaisDataModel ToDataModel(CrearPaisRequest request)
        {
            if (request == null) return null;

            return new PaisDataModel
            {
                CodigoIso2 = request.CodigoIso2?.ToUpper(),
                CodigoIso3 = request.CodigoIso3?.ToUpper(),
                Nombre = request.Nombre,
                Continente = request.Continente,

                // 💀 default
                Estado = "ACTIVO"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static PaisDataModel ToDataModel(ActualizarPaisRequest request)
        {
            if (request == null) return null;

            var model = new PaisDataModel
            {
                Nombre = request.Nombre,
                Continente = request.Continente
            };

            // 🔥 solo si viene valor
            if (!string.IsNullOrWhiteSpace(request.CodigoIso2))
                model.CodigoIso2 = request.CodigoIso2.ToUpper();

            if (!string.IsNullOrWhiteSpace(request.CodigoIso3))
                model.CodigoIso3 = request.CodigoIso3.ToUpper();

            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static PaisResponse ToResponse(PaisDataModel model)
        {
            if (model == null) return null;

            return new PaisResponse
            {
                IdPais = model.IdPais,
                CodigoIso2 = model.CodigoIso2,
                CodigoIso3 = model.CodigoIso3,
                Nombre = model.Nombre,
                Continente = model.Continente,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<PaisResponse> ToResponseList(IEnumerable<PaisDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}