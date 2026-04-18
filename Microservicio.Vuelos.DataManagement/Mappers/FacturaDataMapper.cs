using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class FacturaDataMapper
    {
        // 🔹 Entity → DataModel
        public static FacturaDataModel ToDataModel(FacturaEntity entity)
        {
            if (entity == null) return null;

            return new FacturaDataModel
            {
                IdFactura = entity.IdFactura,
                GuidFactura = entity.GuidFactura,

                IdCliente = entity.IdCliente,
                IdReserva = entity.IdReserva,
                IdMetodo = entity.IdMetodo,

                NumeroFactura = entity.NumeroFactura,
                FechaEmision = entity.FechaEmision,

                Subtotal = entity.Subtotal,
                ValorIva = entity.ValorIva,
                CargoServicio = entity.CargoServicio,
                Total = entity.Total,

                ObservacionesFactura = entity.ObservacionesFactura,
                OrigenCanalFactura = entity.OrigenCanalFactura,

                Estado = entity.Estado,

                ServicioOrigen = entity.ServicioOrigen,

                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion
            };
        }

        // 🔹 DataModel → Entity
        public static FacturaEntity ToEntity(FacturaDataModel model)
        {
            if (model == null) return null;

            return new FacturaEntity
            {
                IdFactura = model.IdFactura,
                GuidFactura = model.GuidFactura,

                IdCliente = model.IdCliente,
                IdReserva = model.IdReserva,
                IdMetodo = model.IdMetodo,

                NumeroFactura = model.NumeroFactura,
                FechaEmision = model.FechaEmision,

                Subtotal = model.Subtotal,
                ValorIva = model.ValorIva,
                CargoServicio = model.CargoServicio,
                Total = model.Total,

                ObservacionesFactura = model.ObservacionesFactura,
                OrigenCanalFactura = model.OrigenCanalFactura,

                Estado = model.Estado,

                ServicioOrigen = model.ServicioOrigen,

                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(FacturaEntity entity, FacturaDataModel model)
        {
            entity.NumeroFactura = model.NumeroFactura;
            entity.FechaEmision = model.FechaEmision;

            entity.Subtotal = model.Subtotal;
            entity.ValorIva = model.ValorIva;
            entity.CargoServicio = model.CargoServicio;
            entity.Total = model.Total;

            entity.ObservacionesFactura = model.ObservacionesFactura;
            entity.OrigenCanalFactura = model.OrigenCanalFactura;

            entity.Estado = model.Estado;

            entity.ServicioOrigen = model.ServicioOrigen;

            entity.FechaInhabilitacionUtc = model.FechaInhabilitacionUtc;
            entity.MotivoInhabilitacion = model.MotivoInhabilitacion;

            // ❗ NO tocar:
            // IdCliente
            // IdReserva
            // IdMetodo
            // GuidFactura
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<FacturaDataModel> ToDataModelList(IEnumerable<FacturaEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<FacturaEntity> ToEntityList(IEnumerable<FacturaDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}