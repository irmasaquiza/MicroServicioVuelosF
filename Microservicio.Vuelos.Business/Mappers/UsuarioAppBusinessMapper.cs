using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class UsuarioAppBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
public static UsuarioAppDataModel ToDataModel(CrearUsuarioAppRequest request)
{
    if (request == null) return null;

    return new UsuarioAppDataModel
    {
        IdCliente = request.IdCliente,
        Username = request.Username,
        Correo = request.Correo,

        // 🔥 AQUÍ ESTABA EL PROBLEMA
        PasswordHash = request.Password,

        EstadoUsuario = "ACT",
        Activo = true
    };
}


        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static UsuarioAppDataModel ToDataModel(ActualizarUsuarioAppRequest request)
        {
            if (request == null) return null;

            var model = new UsuarioAppDataModel
            {
                Username = request.Username,
                Correo = request.Correo
            };

            if (!string.IsNullOrWhiteSpace(request.EstadoUsuario))
                model.EstadoUsuario = request.EstadoUsuario;

            if (request.Activo.HasValue)
                model.Activo = request.Activo.Value;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response (SIN PASSWORD)
        // ============================================================
        public static UsuarioAppResponse ToResponse(UsuarioAppDataModel model)
        {
            if (model == null) return null;

            return new UsuarioAppResponse
            {
                IdUsuario = model.IdUsuario,
                UsuarioGuid = model.UsuarioGuid,
                IdCliente = model.IdCliente,
                Username = model.Username,
                Correo = model.Correo,
                FechaUltimoLogin = model.FechaUltimoLogin,
                EstadoUsuario = model.EstadoUsuario,
                Activo = model.Activo
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<UsuarioAppResponse> ToResponseList(IEnumerable<UsuarioAppDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}
