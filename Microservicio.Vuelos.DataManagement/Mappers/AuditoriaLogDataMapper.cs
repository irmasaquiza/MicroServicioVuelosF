using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class AuditoriaLogDataMapper
    {
        // 🔹 Entity → DataModel
        public static AuditoriaLogDataModel ToDataModel(AuditoriaLogEntity entity)
        {
            if (entity == null) return null;

            return new AuditoriaLogDataModel
            {
                IdAuditoria = entity.IdAuditoria,
                AuditoriaGuid = entity.AuditoriaGuid,

                TablaAfectada = entity.TablaAfectada,
                Operacion = entity.Operacion,
                IdRegistroAfectado = entity.IdRegistroAfectado,

                DatosAnteriores = entity.DatosAnteriores,
                DatosNuevos = entity.DatosNuevos,

                UsuarioEjecutor = entity.UsuarioEjecutor,
                IpOrigen = entity.IpOrigen,

                FechaEventoUtc = entity.FechaEventoUtc,

                Activo = entity.Activo
            };
        }

        // 🔹 DataModel → Entity
        public static AuditoriaLogEntity ToEntity(AuditoriaLogDataModel model)
        {
            if (model == null) return null;

            return new AuditoriaLogEntity
            {
                IdAuditoria = model.IdAuditoria,
                AuditoriaGuid = model.AuditoriaGuid,

                TablaAfectada = model.TablaAfectada,
                Operacion = model.Operacion,
                IdRegistroAfectado = model.IdRegistroAfectado,

                DatosAnteriores = model.DatosAnteriores,
                DatosNuevos = model.DatosNuevos,

                UsuarioEjecutor = model.UsuarioEjecutor,
                IpOrigen = model.IpOrigen,

                FechaEventoUtc = model.FechaEventoUtc,

                Activo = model.Activo
            };
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<AuditoriaLogDataModel> ToDataModelList(IEnumerable<AuditoriaLogEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<AuditoriaLogEntity> ToEntityList(IEnumerable<AuditoriaLogDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}