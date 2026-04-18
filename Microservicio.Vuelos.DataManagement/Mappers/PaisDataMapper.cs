using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class PaisDataMapper
    {
        // 🔹 Entity → DataModel
        public static PaisDataModel ToDataModel(PaisEntity entity)
        {
            if (entity == null) return null;

            return new PaisDataModel
            {
                IdPais = entity.IdPais,

                CodigoIso2 = entity.CodigoIso2,
                CodigoIso3 = entity.CodigoIso3,

                Nombre = entity.Nombre,
                Continente = entity.Continente,

                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static PaisEntity ToEntity(PaisDataModel model)
        {
            if (model == null) return null;

            return new PaisEntity
            {
                IdPais = model.IdPais,

                CodigoIso2 = model.CodigoIso2,
                CodigoIso3 = model.CodigoIso3,

                Nombre = model.Nombre,
                Continente = model.Continente,

                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(PaisEntity entity, PaisDataModel model)
        {
            entity.CodigoIso2 = model.CodigoIso2;
            entity.CodigoIso3 = model.CodigoIso3;

            entity.Nombre = model.Nombre;
            entity.Continente = model.Continente;

            entity.Estado = model.Estado;
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<PaisDataModel> ToDataModelList(IEnumerable<PaisEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<PaisEntity> ToEntityList(IEnumerable<PaisDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}