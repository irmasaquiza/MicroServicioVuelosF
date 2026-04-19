using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Reserva;
using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;
using Microservicio.Vuelos.Business.DTOs.Internal.Factura;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class ReservaBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static ReservaDataModel ToDataModel(CrearReservaRequest request)
        {
            if (request == null) return null;

            return new ReservaDataModel
            {
                IdCliente = request.IdCliente,
                IdPasajero = request.IdPasajero,
                IdVuelo = request.IdVuelo,
                IdAsiento = request.IdAsiento,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                SubtotalReserva = request.SubtotalReserva,
                ValorIva = request.ValorIva,
                TotalReserva = request.TotalReserva,
                OrigenCanalReserva = request.OrigenCanalReserva,
                ContactoEmail = request.ContactoEmail,
                ContactoTelefono = request.ContactoTelefono,
                Observaciones = request.Observaciones,

                // 💀 defaults
                FechaReservaUtc = DateTime.UtcNow,
                EstadoReserva = "PEN" // Pendiente
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static ReservaDataModel ToDataModel(ActualizarReservaRequest request)
        {
            if (request == null) return null;

            var model = new ReservaDataModel
            {
                ContactoEmail = request.ContactoEmail,
                ContactoTelefono = request.ContactoTelefono,
                Observaciones = request.Observaciones
            };

            if (request.FechaInicio.HasValue)
                model.FechaInicio = request.FechaInicio.Value;

            if (request.FechaFin.HasValue)
                model.FechaFin = request.FechaFin.Value;

            if (request.SubtotalReserva.HasValue)
                model.SubtotalReserva = request.SubtotalReserva.Value;

            if (request.ValorIva.HasValue)
                model.ValorIva = request.ValorIva.Value;

            if (request.TotalReserva.HasValue)
                model.TotalReserva = request.TotalReserva.Value;

            return model;
        }

        // ============================================================
        // 🔄 Actualizar Estado → DataModel
        // ============================================================
        public static ReservaDataModel ToDataModel(ActualizarEstadoReservaRequest request)
        {
            if (request == null) return null;

            return new ReservaDataModel
            {
                EstadoReserva = request.EstadoReserva,
                MotivoCancelacion = request.MotivoCancelacion
            };
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static ReservaResponse ToResponse(ReservaDataModel model)
        {
            if (model == null) return null;

            return new ReservaResponse
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
                SubtotalReserva = model.SubtotalReserva,
                ValorIva = model.ValorIva,
                TotalReserva = model.TotalReserva,
                OrigenCanalReserva = model.OrigenCanalReserva,
                EstadoReserva = model.EstadoReserva,
                MotivoCancelacion = model.MotivoCancelacion,
                ContactoEmail = model.ContactoEmail,
                ContactoTelefono = model.ContactoTelefono,
                Observaciones = model.Observaciones
            };
        }

        // ============================================================
        // 🔄 DataModel → DetalleResponse
        // ============================================================
        public static ReservaDetalleResponse ToDetalleResponse(
            ReservaDataModel model,
            IEnumerable<BoletoDataModel> boletos,
            IEnumerable<FacturaDataModel> facturas)
        {
            if (model == null) return null;

            return new ReservaDetalleResponse
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
                SubtotalReserva = model.SubtotalReserva,
                ValorIva = model.ValorIva,
                TotalReserva = model.TotalReserva,
                OrigenCanalReserva = model.OrigenCanalReserva,
                EstadoReserva = model.EstadoReserva,
                MotivoCancelacion = model.MotivoCancelacion,
                ContactoEmail = model.ContactoEmail,
                ContactoTelefono = model.ContactoTelefono,
                Observaciones = model.Observaciones,

                // 🔥 nested mapping
                Boletos = boletos?.Select(BoletoBusinessMapper.ToResponse),
                Facturas = facturas?.Select(FacturaBusinessMapper.ToResponse)
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<ReservaResponse> ToResponseList(IEnumerable<ReservaDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}