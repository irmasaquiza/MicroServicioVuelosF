using System;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class VueloDataMapper
    {
        // ============================================================
        // 🔹 Entity → DataModel
        // ============================================================
        public static VueloDataModel ToDataModel(VueloEntity entity)
        {
            if (entity == null) return null;

            return new VueloDataModel
            {
                IdVuelo = entity.IdVuelo,

                CodigoVuelo = entity.CodigoVuelo,

                IdAeropuertoOrigen = entity.IdAeropuertoOrigen,
                IdAeropuertoDestino = entity.IdAeropuertoDestino,

                FechaHoraSalida = entity.FechaHoraSalida,
                FechaHoraLlegada = entity.FechaHoraLlegada,

                DuracionMin = entity.DuracionMin,

                EstadoVuelo = entity.EstadoVuelo,

                CapacidadTotal = entity.CapacidadTotal,

                PrecioBase = entity.PrecioBase,

                Estado = entity.Estado
            };
        }

        // ============================================================
        // 🔹 DataModel → Entity
        // ============================================================
        public static VueloEntity ToEntity(VueloDataModel model)
        {
            if (model == null) return null;

            return new VueloEntity
            {
                IdVuelo = model.IdVuelo,

                CodigoVuelo = model.CodigoVuelo,

                IdAeropuertoOrigen = model.IdAeropuertoOrigen,
                IdAeropuertoDestino = model.IdAeropuertoDestino,

                FechaHoraSalida = model.FechaHoraSalida,
                FechaHoraLlegada = model.FechaHoraLlegada,

                DuracionMin = model.DuracionMin,

                EstadoVuelo = model.EstadoVuelo,

                CapacidadTotal = model.CapacidadTotal,

                PrecioBase = model.PrecioBase,

                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔹 Update controlado
        // ============================================================
        public static void UpdateEntity(VueloEntity entity, VueloDataModel model)
        {
            entity.CodigoVuelo = model.CodigoVuelo;

            entity.FechaHoraSalida = model.FechaHoraSalida;
            entity.FechaHoraLlegada = model.FechaHoraLlegada;
            entity.DuracionMin = model.DuracionMin;

            entity.EstadoVuelo = model.EstadoVuelo;

            entity.CapacidadTotal = model.CapacidadTotal;

            entity.PrecioBase = model.PrecioBase;

            entity.Estado = model.Estado;

            // ❗ NO tocar:
            // IdVuelo
            // IdAeropuertoOrigen
            // IdAeropuertoDestino
        }

        // ============================================================
        // 🔹 Lista Entity → DataModel
        // ============================================================
        public static IEnumerable<VueloDataModel> ToDataModelList(IEnumerable<VueloEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // ============================================================
        // 🔹 Lista DataModel → Entity
        // ============================================================
        public static IEnumerable<VueloEntity> ToEntityList(IEnumerable<VueloDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}