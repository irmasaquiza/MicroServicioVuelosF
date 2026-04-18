using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class RolDataMapper
    {
        // 🔹 Entity → DataModel
        public static RolDataModel ToDataModel(RolEntity entity)
        {
            if (entity == null) return null;

            return new RolDataModel
            {
                IdRol = entity.IdRol,
                RolGuid = entity.RolGuid,

                NombreRol = entity.NombreRol,
                DescripcionRol = entity.DescripcionRol,

                EstadoRol = entity.EstadoRol,
                Activo = entity.Activo
            };
        }

        // 🔹 DataModel → Entity
        public static RolEntity ToEntity(RolDataModel model)
        {
            if (model == null) return null;

            return new RolEntity
            {
                IdRol = model.IdRol,
                RolGuid = model.RolGuid,

                NombreRol = model.NombreRol,
                DescripcionRol = model.DescripcionRol,

                EstadoRol = model.EstadoRol,
                Activo = model.Activo
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(RolEntity entity, RolDataModel model)
        {
            entity.NombreRol = model.NombreRol;
            entity.DescripcionRol = model.DescripcionRol;

            entity.EstadoRol = model.EstadoRol;
            entity.Activo = model.Activo;

            // ❗ NO tocar:
            // IdRol
            // RolGuid
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<RolDataModel> ToDataModelList(IEnumerable<RolEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<RolEntity> ToEntityList(IEnumerable<RolDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}