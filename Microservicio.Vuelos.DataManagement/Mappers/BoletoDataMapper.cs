using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class BoletoDataMapper
    {
        // 🔹 Entity → DataModel
        public static BoletoDataModel ToDataModel(BoletoEntity entity)
        {
            if (entity == null) return null;

            return new BoletoDataModel
            {
                IdBoleto = entity.IdBoleto,

                IdReserva = entity.IdReserva,
                IdVuelo = entity.IdVuelo,
                IdAsiento = entity.IdAsiento,
                IdFactura = entity.IdFactura,

                CodigoBoleto = entity.CodigoBoleto,
                Clase = entity.Clase,

                PrecioVueloBase = entity.PrecioVueloBase,
                PrecioAsientoExtra = entity.PrecioAsientoExtra,
                ImpuestosBoleto = entity.ImpuestosBoleto,
                CargoEquipaje = entity.CargoEquipaje,
                PrecioFinal = entity.PrecioFinal,

                EstadoBoleto = entity.EstadoBoleto,
                FechaEmision = entity.FechaEmision
            };
        }

        // 🔹 DataModel → Entity
        public static BoletoEntity ToEntity(BoletoDataModel model)
        {
            if (model == null) return null;

            return new BoletoEntity
            {
                IdBoleto = model.IdBoleto,

                IdReserva = model.IdReserva,
                IdVuelo = model.IdVuelo,
                IdAsiento = model.IdAsiento,
                IdFactura = model.IdFactura,

                CodigoBoleto = model.CodigoBoleto,
                Clase = model.Clase,

                PrecioVueloBase = model.PrecioVueloBase,
                PrecioAsientoExtra = model.PrecioAsientoExtra,
                ImpuestosBoleto = model.ImpuestosBoleto,
                CargoEquipaje = model.CargoEquipaje,
                PrecioFinal = model.PrecioFinal,

                EstadoBoleto = model.EstadoBoleto,
                FechaEmision = model.FechaEmision
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(BoletoEntity entity, BoletoDataModel model)
        {
            entity.CodigoBoleto = model.CodigoBoleto;
            entity.Clase = model.Clase;

            entity.PrecioVueloBase = model.PrecioVueloBase;
            entity.PrecioAsientoExtra = model.PrecioAsientoExtra;
            entity.ImpuestosBoleto = model.ImpuestosBoleto;
            entity.CargoEquipaje = model.CargoEquipaje;
            entity.PrecioFinal = model.PrecioFinal;

            entity.EstadoBoleto = model.EstadoBoleto;
            entity.FechaEmision = model.FechaEmision;

            // ❗ NO tocar:
            // IdReserva
            // IdVuelo
            // IdAsiento
            // IdFactura
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<BoletoDataModel> ToDataModelList(IEnumerable<BoletoEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<BoletoEntity> ToEntityList(IEnumerable<BoletoDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}