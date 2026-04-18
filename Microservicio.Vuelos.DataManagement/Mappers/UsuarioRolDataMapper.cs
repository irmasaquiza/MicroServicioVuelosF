using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class UsuarioRolDataMapper
    {
        // 🔹 Entity → DataModel
        public static UsuarioRolDataModel ToDataModel(UsuarioRolEntity entity)
        {
            if (entity == null) return null;

            return new UsuarioRolDataModel
            {
                IdUsuarioRol = entity.IdUsuarioRol,

                IdUsuario = entity.IdUsuario,
                IdRol = entity.IdRol,

                EstadoUsuarioRol = entity.EstadoUsuarioRol,
                Activo = entity.Activo
            };
        }

        // 🔹 DataModel → Entity
        public static UsuarioRolEntity ToEntity(UsuarioRolDataModel model)
        {
            if (model == null) return null;

            return new UsuarioRolEntity
            {
                IdUsuarioRol = model.IdUsuarioRol,

                IdUsuario = model.IdUsuario,
                IdRol = model.IdRol,

                EstadoUsuarioRol = model.EstadoUsuarioRol,
                Activo = model.Activo
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(UsuarioRolEntity entity, UsuarioRolDataModel model)
        {
            entity.EstadoUsuarioRol = model.EstadoUsuarioRol;
            entity.Activo = model.Activo;

            // ❗ NO tocar:
            // IdUsuarioRol
            // IdUsuario
            // IdRol
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<UsuarioRolDataModel> ToDataModelList(IEnumerable<UsuarioRolEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<UsuarioRolEntity> ToEntityList(IEnumerable<UsuarioRolDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}