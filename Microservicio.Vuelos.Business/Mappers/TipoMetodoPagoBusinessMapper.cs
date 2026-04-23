/*using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.TipoMetodoPago;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class TipoMetodoPagoBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static TipoMetodoPagoDataModel ToDataModel(CrearTipoMetodoPagoRequest request)
        {
            if (request == null) return null;

            return new TipoMetodoPagoDataModel
            {
                NombreTipo = request.NombreTipo,
                Descripcion = request.Descripcion,

                // 💀 default
                Estado = "ACTIVO"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static TipoMetodoPagoDataModel ToDataModel(ActualizarTipoMetodoPagoRequest request)
        {
            if (request == null) return null;

            var model = new TipoMetodoPagoDataModel
            {
                NombreTipo = request.NombreTipo,
                Descripcion = request.Descripcion
            };

            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static TipoMetodoPagoResponse ToResponse(TipoMetodoPagoDataModel model)
        {
            if (model == null) return null;

            return new TipoMetodoPagoResponse
            {
                IdTipoMetodo = model.IdTipoMetodo,
                NombreTipo = model.NombreTipo,
                Descripcion = model.Descripcion,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<TipoMetodoPagoResponse> ToResponseList(IEnumerable<TipoMetodoPagoDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}*/