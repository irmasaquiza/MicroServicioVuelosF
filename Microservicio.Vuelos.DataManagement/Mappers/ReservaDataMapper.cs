using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class ReservaDataMapper
    {
        // 🔹 Entity → DataModel
        public static ReservaDataModel ToDataModel(ReservaEntity entity)
        {
            if (entity == null) return null;

            return new ReservaDataModel
            {
                IdReserva = entity.IdReserva,
                GuidReserva = entity.GuidReserva,
                CodigoReserva = entity.CodigoReserva,

                IdCliente = entity.IdCliente,
                IdPasajero = entity.IdPasajero,
                IdVuelo = entity.IdVuelo,
                IdAsiento = entity.IdAsiento,

                FechaReservaUtc = entity.FechaReservaUtc,
                FechaInicio = entity.FechaInicio,
                FechaFin = entity.FechaFin,

                FechaConfirmacionUtc = entity.FechaConfirmacionUtc,
                FechaCancelacionUtc = entity.FechaCancelacionUtc,

                SubtotalReserva = entity.SubtotalReserva,
                ValorIva = entity.ValorIva,
                TotalReserva = entity.TotalReserva,

                EstadoReserva = entity.EstadoReserva,
                OrigenCanalReserva = entity.OrigenCanalReserva,
                MotivoCancelacion = entity.MotivoCancelacion,

                ContactoEmail = entity.ContactoEmail,
                ContactoTelefono = entity.ContactoTelefono,
                Observaciones = entity.Observaciones,

                ServicioOrigen = entity.ServicioOrigen,

                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion
            };
        }

        // 🔹 DataModel → Entity
        public static ReservaEntity ToEntity(ReservaDataModel model)
        {
            if (model == null) return null;

            return new ReservaEntity
            {
                IdReserva = model.IdReserva,
                GuidReserva = model.GuidReserva,
                CodigoReserva = model.CodigoReserva,

                IdCliente = model.IdCliente,
                IdPasajero = model.IdPasajero,
                IdVuelo = model.IdVuelo,
                IdAsiento = model.IdAsiento,

                FechaReservaUtc = model.FechaReservaUtc,
                FechaInicio = model.FechaInicio,
                FechaFin = model.FechaFin,

                FechaConfirmacionUtc = model.FechaConfirmacionUtc,
                FechaCancelacionUtc = model.FechaCancelacionUtc,

                SubtotalReserva = model.SubtotalReserva,
                ValorIva = model.ValorIva,
                TotalReserva = model.TotalReserva,

                EstadoReserva = model.EstadoReserva,
                OrigenCanalReserva = model.OrigenCanalReserva,
                MotivoCancelacion = model.MotivoCancelacion,

                ContactoEmail = model.ContactoEmail,
                ContactoTelefono = model.ContactoTelefono,
                Observaciones = model.Observaciones,

                ServicioOrigen = model.ServicioOrigen,

                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(ReservaEntity entity, ReservaDataModel model)
        {
            entity.CodigoReserva = model.CodigoReserva;

            entity.FechaInicio = model.FechaInicio;
            entity.FechaFin = model.FechaFin;

            entity.FechaConfirmacionUtc = model.FechaConfirmacionUtc;
            entity.FechaCancelacionUtc = model.FechaCancelacionUtc;

            entity.SubtotalReserva = model.SubtotalReserva;
            entity.ValorIva = model.ValorIva;
            entity.TotalReserva = model.TotalReserva;

            entity.EstadoReserva = model.EstadoReserva;
            entity.OrigenCanalReserva = model.OrigenCanalReserva;
            entity.MotivoCancelacion = model.MotivoCancelacion;

            entity.ContactoEmail = model.ContactoEmail;
            entity.ContactoTelefono = model.ContactoTelefono;
            entity.Observaciones = model.Observaciones;

            entity.ServicioOrigen = model.ServicioOrigen;

            entity.FechaInhabilitacionUtc = model.FechaInhabilitacionUtc;
            entity.MotivoInhabilitacion = model.MotivoInhabilitacion;

            // ❗ NO tocar:
            // IdCliente
            // IdPasajero
            // IdVuelo
            // IdAsiento
            // GuidReserva
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<ReservaDataModel> ToDataModelList(IEnumerable<ReservaEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<ReservaEntity> ToEntityList(IEnumerable<ReservaDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}