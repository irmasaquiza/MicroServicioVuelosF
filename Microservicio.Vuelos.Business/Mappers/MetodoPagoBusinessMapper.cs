using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class MetodoPagoBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static MetodoPagoDataModel ToDataModel(CrearMetodoPagoRequest request)
        {
            if (request == null) return null;

            return new MetodoPagoDataModel
            {
                IdCliente = request.IdCliente,
                IdTipoMetodo = request.IdTipoMetodo,

                // 💀 SE GUARDA, PERO NUNCA SE EXPONE
                TokenPasarela = request.TokenPasarela,

                Ultimos4 = request.Ultimos4,
                ReferenciaVisible = request.ReferenciaVisible,
                FechaExpiracion = request.FechaExpiracion,
                NombreTitular = request.NombreTitular,
                MarcaTarjeta = request.MarcaTarjeta,
                BancoEmisor = request.BancoEmisor,
                PaisEmision = request.PaisEmision,
                EsPrincipal = request.EsPrincipal,
                Alias = request.Alias,

                // 💀 defaults
                Estado = "ACTIVO"
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static MetodoPagoDataModel ToDataModel(ActualizarMetodoPagoRequest request)
        {
            if (request == null) return null;

            var model = new MetodoPagoDataModel
            {
                ReferenciaVisible = request.ReferenciaVisible,
                NombreTitular = request.NombreTitular,
                MarcaTarjeta = request.MarcaTarjeta,
                BancoEmisor = request.BancoEmisor,
                PaisEmision = request.PaisEmision,
                Alias = request.Alias
            };

            // 🔥 nullable fields
            if (request.FechaExpiracion.HasValue)
                model.FechaExpiracion = request.FechaExpiracion.Value;

            if (request.EsPrincipal.HasValue)
                model.EsPrincipal = request.EsPrincipal.Value;

            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response (SIN TOKEN)
        // ============================================================
        public static MetodoPagoResponse ToResponse(MetodoPagoDataModel model)
        {
            if (model == null) return null;

            return new MetodoPagoResponse
            {
                IdMetodo = model.IdMetodo,
                IdCliente = model.IdCliente,
                IdTipoMetodo = model.IdTipoMetodo,
                Ultimos4 = model.Ultimos4,
                ReferenciaVisible = model.ReferenciaVisible,
                FechaExpiracion = model.FechaExpiracion,
                NombreTitular = model.NombreTitular,
                MarcaTarjeta = model.MarcaTarjeta,
                BancoEmisor = model.BancoEmisor,
                PaisEmision = model.PaisEmision,
                EsPrincipal = model.EsPrincipal,
                Alias = model.Alias,
                Estado = model.Estado
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<MetodoPagoResponse> ToResponseList(IEnumerable<MetodoPagoDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}