using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class AuditoriaLogBusinessMapper
    {
        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static AuditoriaLogResponse ToResponse(AuditoriaLogDataModel model)
        {
            if (model == null) return null;

            return new AuditoriaLogResponse
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
                FechaEventoUtc = model.FechaEventoUtc
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<AuditoriaLogResponse> ToResponseList(IEnumerable<AuditoriaLogDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}
