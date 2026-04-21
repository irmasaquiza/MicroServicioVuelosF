using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class AsientoDataMapper
    {
        // 🔹 Entity → DataModel
        public static AsientoDataModel ToDataModel(AsientoEntity entity)
        {
            if (entity == null) return null;

            return new AsientoDataModel
            {
                IdAsiento = entity.IdAsiento,
                IdVuelo = entity.IdVuelo,
                NumeroAsiento = entity.NumeroAsiento,
                Clase = entity.Clase,
                Disponible = entity.Disponible,
                PrecioExtra = entity.PrecioExtra,
                Posicion = entity.Posicion,
                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static AsientoEntity ToEntity(AsientoDataModel model)
        {
            if (model == null) return null;

            return new AsientoEntity
            {
                IdAsiento = model.IdAsiento,
                IdVuelo = model.IdVuelo,
                NumeroAsiento = model.NumeroAsiento,
                Clase = model.Clase,
                Disponible = model.Disponible,
                PrecioExtra = model.PrecioExtra,
                Posicion = model.Posicion,

                // 🔥 FIX
                Estado = model.Estado ?? "ACTIVO",

                Eliminado = model.Eliminado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(AsientoEntity entity, AsientoDataModel model)
        {
            entity.NumeroAsiento = model.NumeroAsiento;
            entity.Clase = model.Clase;
            entity.Disponible = model.Disponible;
            entity.PrecioExtra = model.PrecioExtra;
            entity.Posicion = model.Posicion;

            // 🔥 PROTECCIÓN
            entity.Estado = model.Estado ?? entity.Estado;
        }

        public static IEnumerable<AsientoDataModel> ToDataModelList(IEnumerable<AsientoEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        public static IEnumerable<AsientoEntity> ToEntityList(IEnumerable<AsientoDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}