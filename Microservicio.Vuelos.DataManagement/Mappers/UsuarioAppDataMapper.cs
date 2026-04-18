using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class UsuarioAppDataMapper
    {
        // 🔹 Entity → DataModel
        public static UsuarioAppDataModel ToDataModel(UsuarioAppEntity entity)
        {
            if (entity == null) return null;

            return new UsuarioAppDataModel
            {
                IdUsuario = entity.IdUsuario,
                UsuarioGuid = entity.UsuarioGuid,

                IdCliente = entity.IdCliente,

                Username = entity.Username,
                Correo = entity.Correo,

                FechaUltimoLogin = entity.FechaUltimoLogin,

                EstadoUsuario = entity.EstadoUsuario,
                Activo = entity.Activo
            };
        }

        // 🔹 DataModel → Entity
        public static UsuarioAppEntity ToEntity(UsuarioAppDataModel model)
        {
            if (model == null) return null;

            return new UsuarioAppEntity
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

        // 🔹 Update controlado
        public static void UpdateEntity(UsuarioAppEntity entity, UsuarioAppDataModel model)
        {
            entity.IdCliente = model.IdCliente;

            entity.Username = model.Username;
            entity.Correo = model.Correo;

            entity.FechaUltimoLogin = model.FechaUltimoLogin;

            entity.EstadoUsuario = model.EstadoUsuario;
            entity.Activo = model.Activo;

            // ❗ NO tocar:
            // IdUsuario
            // UsuarioGuid
            // PasswordHash
            // PasswordSalt
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<UsuarioAppDataModel> ToDataModelList(IEnumerable<UsuarioAppEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<UsuarioAppEntity> ToEntityList(IEnumerable<UsuarioAppDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}