using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Factura;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class FacturaBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static FacturaDataModel ToDataModel(CrearFacturaRequest request)
        {
            if (request == null) return null;

            return new FacturaDataModel
            {
                IdCliente = request.IdCliente,
                IdReserva = request.IdReserva,
                IdMetodo = request.IdMetodo,
                Subtotal = request.Subtotal,
                ValorIva = request.ValorIva,
                CargoServicio = request.CargoServicio,
                Total = request.Total,
                ObservacionesFactura = request.ObservacionesFactura,
                OrigenCanalFactura = request.OrigenCanalFactura,

                // 💀 valores por defecto
                Estado = "ABI", // Abierta
                FechaEmision = DateTime.UtcNow
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static FacturaDataModel ToDataModel(ActualizarFacturaRequest request)
        {
            if (request == null) return null;

            var model = new FacturaDataModel
            {
                ObservacionesFactura = request.ObservacionesFactura,
                OrigenCanalFactura = request.OrigenCanalFactura
            };

            // 🔥 nullable fields
            if (request.Subtotal.HasValue)
                model.Subtotal = request.Subtotal.Value;

            if (request.ValorIva.HasValue)
                model.ValorIva = request.ValorIva.Value;

            if (request.CargoServicio.HasValue)
                model.CargoServicio = request.CargoServicio.Value;

            if (request.Total.HasValue)
                model.Total = request.Total.Value;

            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static FacturaResponse ToResponse(FacturaDataModel model)
        {
            if (model == null) return null;

            return new FacturaResponse
            {
                IdFactura = model.IdFactura,
                GuidFactura = model.GuidFactura,
                NumeroFactura = model.NumeroFactura,
                IdCliente = model.IdCliente,
                IdReserva = model.IdReserva,
                IdMetodo = model.IdMetodo,
                FechaEmision = model.FechaEmision,
                Subtotal = model.Subtotal,
                ValorIva = model.ValorIva,
                CargoServicio = model.CargoServicio,
                Total = model.Total,
                Estado = model.Estado,
                ObservacionesFactura = model.ObservacionesFactura,
                OrigenCanalFactura = model.OrigenCanalFactura
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<FacturaResponse> ToResponseList(IEnumerable<FacturaDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}