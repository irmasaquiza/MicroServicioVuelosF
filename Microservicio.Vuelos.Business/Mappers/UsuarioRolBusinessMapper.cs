using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class UsuarioRolBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        // ⚠️ IdUsuario NO viene en el request (viene en la URL)
        public static UsuarioRolDataModel ToDataModel(int idUsuario, CrearUsuarioRolRequest request)
        {
            if (request == null) return null;

            return new UsuarioRolDataModel
            {
                IdUsuario = idUsuario,
                IdRol = request.IdRol,

                // 💀 defaults
                EstadoUsuarioRol = "ACT",
                Activo = true
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static UsuarioRolDataModel ToDataModel(ActualizarUsuarioRolRequest request)
        {
            if (request == null) return null;

            var model = new UsuarioRolDataModel();

            if (!string.IsNullOrWhiteSpace(request.EstadoUsuarioRol))
                model.EstadoUsuarioRol = request.EstadoUsuarioRol;

            if (request.Activo.HasValue)
                model.Activo = request.Activo.Value;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static UsuarioRolResponse ToResponse(UsuarioRolDataModel model)
        {
            if (model == null) return null;

            return new UsuarioRolResponse
            {
                IdUsuarioRol = model.IdUsuarioRol,
                IdUsuario = model.IdUsuario,
                IdRol = model.IdRol,
                EstadoUsuarioRol = model.EstadoUsuarioRol,
                Activo = model.Activo
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<UsuarioRolResponse> ToResponseList(IEnumerable<UsuarioRolDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}