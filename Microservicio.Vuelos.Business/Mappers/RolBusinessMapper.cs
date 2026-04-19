using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Rol;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class RolBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static RolDataModel ToDataModel(CrearRolRequest request)
        {
            if (request == null) return null;

            return new RolDataModel
            {
                NombreRol = request.NombreRol,
                DescripcionRol = request.DescripcionRol,

                // 💀 defaults
                EstadoRol = "ACT",
                Activo = true
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static RolDataModel ToDataModel(ActualizarRolRequest request)
        {
            if (request == null) return null;

            var model = new RolDataModel
            {
                NombreRol = request.NombreRol,
                DescripcionRol = request.DescripcionRol
            };

            // 🔥 nullable fields
            if (!string.IsNullOrWhiteSpace(request.EstadoRol))
                model.EstadoRol = request.EstadoRol;

            if (request.Activo.HasValue)
                model.Activo = request.Activo.Value;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static RolResponse ToResponse(RolDataModel model)
        {
            if (model == null) return null;

            return new RolResponse
            {
                IdRol = model.IdRol,
                RolGuid = model.RolGuid,
                NombreRol = model.NombreRol,
                DescripcionRol = model.DescripcionRol,
                EstadoRol = model.EstadoRol,
                Activo = model.Activo
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<RolResponse> ToResponseList(IEnumerable<RolDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}