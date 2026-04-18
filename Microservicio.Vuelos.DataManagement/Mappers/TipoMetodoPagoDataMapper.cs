using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class TipoMetodoPagoDataMapper
    {
        // 🔹 Entity → DataModel
        public static TipoMetodoPagoDataModel ToDataModel(TipoMetodoPagoEntity entity)
        {
            if (entity == null) return null;

            return new TipoMetodoPagoDataModel
            {
                IdTipoMetodo = entity.IdTipoMetodo,
                NombreTipo = entity.NombreTipo,
                Descripcion = entity.Descripcion,
                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static TipoMetodoPagoEntity ToEntity(TipoMetodoPagoDataModel model)
        {
            if (model == null) return null;

            return new TipoMetodoPagoEntity
            {
                IdTipoMetodo = model.IdTipoMetodo,
                NombreTipo = model.NombreTipo,
                Descripcion = model.Descripcion,
                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(TipoMetodoPagoEntity entity, TipoMetodoPagoDataModel model)
        {
            entity.NombreTipo = model.NombreTipo;
            entity.Descripcion = model.Descripcion;
            entity.Estado = model.Estado;

            // ❗ NO tocar IdTipoMetodo
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<TipoMetodoPagoDataModel> ToDataModelList(IEnumerable<TipoMetodoPagoEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<TipoMetodoPagoEntity> ToEntityList(IEnumerable<TipoMetodoPagoDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}