using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Cliente;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class ClienteBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static ClienteDataModel ToDataModel(CrearClienteRequest request)
        {
            if (request == null) return null;

            return new ClienteDataModel
            {
                TipoIdentificacion = request.TipoIdentificacion,
                NumeroIdentificacion = request.NumeroIdentificacion,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                IdCiudadResidencia = request.IdCiudadResidencia,
                IdPaisNacionalidad = request.IdPaisNacionalidad,
                FechaNacimiento = request.FechaNacimiento,
                Nacionalidad = request.Nacionalidad,
                Genero = request.Genero,

                // 💀 defaults
                Estado = "ACT"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static ClienteDataModel ToDataModel(ActualizarClienteRequest request)
        {
            if (request == null) return null;

            var model = new ClienteDataModel
            {
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                RazonSocial = request.RazonSocial,
                Correo = request.Correo,
                Telefono = request.Telefono,
                Direccion = request.Direccion,
                Nacionalidad = request.Nacionalidad,
                Genero = request.Genero
            };

            // 🔥 nullable fields
            if (request.IdCiudadResidencia.HasValue)
                model.IdCiudadResidencia = request.IdCiudadResidencia.Value;

            if (request.IdPaisNacionalidad.HasValue)
                model.IdPaisNacionalidad = request.IdPaisNacionalidad.Value;

            if (request.FechaNacimiento.HasValue)
                model.FechaNacimiento = request.FechaNacimiento.Value;

            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static ClienteResponse ToResponse(ClienteDataModel model)
        {
            if (model == null) return null;

            return new ClienteResponse
            {
                IdCliente = model.IdCliente,
                ClienteGuid = model.ClienteGuid,
                TipoIdentificacion = model.TipoIdentificacion,
                NumeroIdentificacion = model.NumeroIdentificacion,
                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                RazonSocial = model.RazonSocial,
                Correo = model.Correo,
                Telefono = model.Telefono,
                Direccion = model.Direccion,
                IdCiudadResidencia = model.IdCiudadResidencia,
                IdPaisNacionalidad = model.IdPaisNacionalidad,
                FechaNacimiento = model.FechaNacimiento,
                Nacionalidad = model.Nacionalidad,
                Genero = model.Genero,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<ClienteResponse> ToResponseList(IEnumerable<ClienteDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}