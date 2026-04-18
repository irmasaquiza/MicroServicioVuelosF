using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class EscalaDataMapper
    {
        // 🔹 Entity → DataModel
        public static EscalaDataModel ToDataModel(EscalaEntity entity)
        {
            if (entity == null) return null;

            return new EscalaDataModel
            {
                IdEscala = entity.IdEscala,
                IdVuelo = entity.IdVuelo,
                IdAeropuerto = entity.IdAeropuerto,

                Orden = entity.Orden,

                FechaHoraLlegada = entity.FechaHoraLlegada,
                FechaHoraSalida = entity.FechaHoraSalida,
                DuracionMin = entity.DuracionMin,

                TipoEscala = entity.TipoEscala,
                Terminal = entity.Terminal,
                Puerta = entity.Puerta,
                Observaciones = entity.Observaciones,

                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static EscalaEntity ToEntity(EscalaDataModel model)
        {
            if (model == null) return null;

            return new EscalaEntity
            {
                IdEscala = model.IdEscala,
                IdVuelo = model.IdVuelo,
                IdAeropuerto = model.IdAeropuerto,

                Orden = model.Orden,

                FechaHoraLlegada = model.FechaHoraLlegada,
                FechaHoraSalida = model.FechaHoraSalida,
                DuracionMin = model.DuracionMin,

                TipoEscala = model.TipoEscala,
                Terminal = model.Terminal,
                Puerta = model.Puerta,
                Observaciones = model.Observaciones,

                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(EscalaEntity entity, EscalaDataModel model)
        {
            entity.Orden = model.Orden;

            entity.FechaHoraLlegada = model.FechaHoraLlegada;
            entity.FechaHoraSalida = model.FechaHoraSalida;
            entity.DuracionMin = model.DuracionMin;

            entity.TipoEscala = model.TipoEscala;
            entity.Terminal = model.Terminal;
            entity.Puerta = model.Puerta;
            entity.Observaciones = model.Observaciones;

            entity.Estado = model.Estado;

            // ❗ NO tocar:
            // IdVuelo
            // IdAeropuerto
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<EscalaDataModel> ToDataModelList(IEnumerable<EscalaEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<EscalaEntity> ToEntityList(IEnumerable<EscalaDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}