using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class BoletoBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static BoletoDataModel ToDataModel(CrearBoletoRequest request)
        {
            if (request == null) return null;

            return new BoletoDataModel
            {
                IdReserva = request.IdReserva,
                IdVuelo = request.IdVuelo,
                IdAsiento = request.IdAsiento,
                IdFactura = request.IdFactura,

                Clase = request.Clase,
                PrecioVueloBase = request.PrecioVueloBase,
                PrecioAsientoExtra = request.PrecioAsientoExtra,
                ImpuestosBoleto = request.ImpuestosBoleto,
                CargoEquipaje = request.CargoEquipaje,
                PrecioFinal = request.PrecioFinal,

                // 💀 valores por defecto
                EstadoBoleto = "ACTIVO",
                FechaEmision = DateTime.UtcNow
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static BoletoDataModel ToDataModel(ActualizarBoletoRequest request)
        {
            if (request == null) return null;

            var model = new BoletoDataModel
            {
                Clase = request.Clase
            };

            // 🔥 nullable fields (PATCH correcto)
            if (request.PrecioVueloBase.HasValue)
                model.PrecioVueloBase = request.PrecioVueloBase.Value;

            if (request.PrecioAsientoExtra.HasValue)
                model.PrecioAsientoExtra = request.PrecioAsientoExtra.Value;

            if (request.ImpuestosBoleto.HasValue)
                model.ImpuestosBoleto = request.ImpuestosBoleto.Value;

            if (request.CargoEquipaje.HasValue)
                model.CargoEquipaje = request.CargoEquipaje.Value;

            if (request.PrecioFinal.HasValue)
                model.PrecioFinal = request.PrecioFinal.Value;

            if (!string.IsNullOrWhiteSpace(request.EstadoBoleto))
                model.EstadoBoleto = request.EstadoBoleto;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static BoletoResponse ToResponse(BoletoDataModel model)
        {
            if (model == null) return null;

            return new BoletoResponse
            {
                IdBoleto = model.IdBoleto,
                CodigoBoleto = model.CodigoBoleto,
                IdReserva = model.IdReserva,
                IdVuelo = model.IdVuelo,
                IdAsiento = model.IdAsiento,
                IdFactura = model.IdFactura,

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

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<BoletoResponse> ToResponseList(IEnumerable<BoletoDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}