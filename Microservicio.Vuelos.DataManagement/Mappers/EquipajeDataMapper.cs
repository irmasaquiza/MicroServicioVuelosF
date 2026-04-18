using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class EquipajeDataMapper
    {
        // 🔹 Entity → DataModel
        public static EquipajeDataModel ToDataModel(EquipajeEntity entity)
        {
            if (entity == null) return null;

            return new EquipajeDataModel
            {
                IdEquipaje = entity.IdEquipaje,
                IdBoleto = entity.IdBoleto,

                Tipo = entity.Tipo,
                PesoKg = entity.PesoKg,
                DescripcionEquipaje = entity.DescripcionEquipaje,
                PrecioExtra = entity.PrecioExtra,
                DimensionesCm = entity.DimensionesCm,
                NumeroEtiqueta = entity.NumeroEtiqueta,

                EstadoEquipaje = entity.EstadoEquipaje,
                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static EquipajeEntity ToEntity(EquipajeDataModel model)
        {
            if (model == null) return null;

            return new EquipajeEntity
            {
                IdEquipaje = model.IdEquipaje,
                IdBoleto = model.IdBoleto,

                Tipo = model.Tipo,
                PesoKg = model.PesoKg,
                DescripcionEquipaje = model.DescripcionEquipaje,
                PrecioExtra = model.PrecioExtra,
                DimensionesCm = model.DimensionesCm,
                NumeroEtiqueta = model.NumeroEtiqueta,

                EstadoEquipaje = model.EstadoEquipaje,
                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(EquipajeEntity entity, EquipajeDataModel model)
        {
            entity.Tipo = model.Tipo;
            entity.PesoKg = model.PesoKg;
            entity.DescripcionEquipaje = model.DescripcionEquipaje;
            entity.PrecioExtra = model.PrecioExtra;
            entity.DimensionesCm = model.DimensionesCm;
            entity.NumeroEtiqueta = model.NumeroEtiqueta;

            entity.EstadoEquipaje = model.EstadoEquipaje;
            entity.Estado = model.Estado;

            // ❗ NO tocar IdBoleto en update
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<EquipajeDataModel> ToDataModelList(IEnumerable<EquipajeEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<EquipajeEntity> ToEntityList(IEnumerable<EquipajeDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}