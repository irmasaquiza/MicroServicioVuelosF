using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class VueloDataMapper
    {
        // 🔹 Entity → DataModel
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
                TipoVuelo = entity.TipoVuelo,

                CapacidadTotal = entity.CapacidadTotal,
                CapacidadDisponible = entity.CapacidadDisponible,

                PrecioBase = entity.PrecioBase,

                Aerolinea = entity.Aerolinea,

                NumeroGate = entity.NumeroGate,
                Terminal = entity.Terminal,

                Observaciones = entity.Observaciones,

                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
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
                TipoVuelo = model.TipoVuelo,

                CapacidadTotal = model.CapacidadTotal,
                CapacidadDisponible = model.CapacidadDisponible,

                PrecioBase = model.PrecioBase,

                Aerolinea = model.Aerolinea,

                NumeroGate = model.NumeroGate,
                Terminal = model.Terminal,

                Observaciones = model.Observaciones,

                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(VueloEntity entity, VueloDataModel model)
        {
            entity.CodigoVuelo = model.CodigoVuelo;

            entity.FechaHoraSalida = model.FechaHoraSalida;
            entity.FechaHoraLlegada = model.FechaHoraLlegada;
            entity.DuracionMin = model.DuracionMin;

            entity.EstadoVuelo = model.EstadoVuelo;
            entity.TipoVuelo = model.TipoVuelo;

            entity.CapacidadTotal = model.CapacidadTotal;
            entity.CapacidadDisponible = model.CapacidadDisponible;

            entity.PrecioBase = model.PrecioBase;

            entity.Aerolinea = model.Aerolinea;

            entity.NumeroGate = model.NumeroGate;
            entity.Terminal = model.Terminal;

            entity.Observaciones = model.Observaciones;

            entity.Estado = model.Estado;

            // ❗ NO tocar:
            // IdVuelo
            // IdAeropuertoOrigen
            // IdAeropuertoDestino
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<VueloDataModel> ToDataModelList(IEnumerable<VueloEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<VueloEntity> ToEntityList(IEnumerable<VueloDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}