using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class CiudadDataMapper
    {
        // 🔹 Entity → DataModel
        public static CiudadDataModel ToDataModel(CiudadEntity entity)
        {
            if (entity == null) return null;

            return new CiudadDataModel
            {
                IdCiudad = entity.IdCiudad,
                IdPais = entity.IdPais,

                Nombre = entity.Nombre,
                CodigoPostal = entity.CodigoPostal,
                ZonaHoraria = entity.ZonaHoraria,

                Latitud = entity.Latitud,
                Longitud = entity.Longitud,

                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static CiudadEntity ToEntity(CiudadDataModel model)
        {
            if (model == null) return null;

            return new CiudadEntity
            {
                IdCiudad = model.IdCiudad,
                IdPais = model.IdPais,

                Nombre = model.Nombre,
                CodigoPostal = model.CodigoPostal,
                ZonaHoraria = model.ZonaHoraria,

                Latitud = model.Latitud,
                Longitud = model.Longitud,

                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(CiudadEntity entity, CiudadDataModel model)
        {
            entity.Nombre = model.Nombre;
            entity.CodigoPostal = model.CodigoPostal;
            entity.ZonaHoraria = model.ZonaHoraria;

            entity.Latitud = model.Latitud;
            entity.Longitud = model.Longitud;

            entity.Estado = model.Estado;

            // ❗ NO tocar IdPais normalmente en update
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<CiudadDataModel> ToDataModelList(IEnumerable<CiudadEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<CiudadEntity> ToEntityList(IEnumerable<CiudadDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}