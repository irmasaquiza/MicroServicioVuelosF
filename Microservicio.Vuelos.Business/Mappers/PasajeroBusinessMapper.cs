using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Pasajero;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class PasajeroBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static PasajeroDataModel ToDataModel(CrearPasajeroRequest request)
        {
            if (request == null) return null;

            return new PasajeroDataModel
            {
                IdCliente = request.IdCliente,
                NombrePasajero = request.NombrePasajero,
                ApellidoPasajero = request.ApellidoPasajero,
                TipoDocumentoPasajero = request.TipoDocumentoPasajero,
                NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
                FechaNacimientoPasajero = request.FechaNacimientoPasajero,
                NacionalidadPasajero = request.NacionalidadPasajero,
                EmailContactoPasajero = request.EmailContactoPasajero,
                TelefonoContactoPasajero = request.TelefonoContactoPasajero,
                GeneroPasajero = request.GeneroPasajero,
                RequiereAsistencia = request.RequiereAsistencia,
                ObservacionesPasajero = request.ObservacionesPasajero
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH)
        // ============================================================
        public static PasajeroDataModel ToDataModel(ActualizarPasajeroRequest request)
        {
            if (request == null) return null;

            var model = new PasajeroDataModel
            {
                NombrePasajero = request.NombrePasajero,
                ApellidoPasajero = request.ApellidoPasajero,
                TipoDocumentoPasajero = request.TipoDocumentoPasajero,
                NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
                NacionalidadPasajero = request.NacionalidadPasajero,
                EmailContactoPasajero = request.EmailContactoPasajero,
                TelefonoContactoPasajero = request.TelefonoContactoPasajero,
                GeneroPasajero = request.GeneroPasajero,
                ObservacionesPasajero = request.ObservacionesPasajero
            };

            // 🔥 nullable fields
            if (request.FechaNacimientoPasajero.HasValue)
                model.FechaNacimientoPasajero = request.FechaNacimientoPasajero.Value;

            if (request.RequiereAsistencia.HasValue)
                model.RequiereAsistencia = request.RequiereAsistencia.Value;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static PasajeroResponse ToResponse(PasajeroDataModel model)
        {
            if (model == null) return null;

            return new PasajeroResponse
            {
                IdPasajero = model.IdPasajero,
                IdCliente = model.IdCliente,
                NombrePasajero = model.NombrePasajero,
                ApellidoPasajero = model.ApellidoPasajero,
                TipoDocumentoPasajero = model.TipoDocumentoPasajero,
                NumeroDocumentoPasajero = model.NumeroDocumentoPasajero,
                FechaNacimientoPasajero = model.FechaNacimientoPasajero,
                NacionalidadPasajero = model.NacionalidadPasajero,
                EmailContactoPasajero = model.EmailContactoPasajero,
                TelefonoContactoPasajero = model.TelefonoContactoPasajero,
                GeneroPasajero = model.GeneroPasajero,
                RequiereAsistencia = model.RequiereAsistencia,
                ObservacionesPasajero = model.ObservacionesPasajero
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<PasajeroResponse> ToResponseList(IEnumerable<PasajeroDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}