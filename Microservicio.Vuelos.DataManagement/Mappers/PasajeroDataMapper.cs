using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class PasajeroDataMapper
    {
        // 🔹 Entity → DataModel
        public static PasajeroDataModel ToDataModel(PasajeroEntity entity)
        {
            if (entity == null) return null;

            return new PasajeroDataModel
            {
                IdPasajero = entity.IdPasajero,
                IdCliente = entity.IdCliente,

                NombrePasajero = entity.NombrePasajero,
                ApellidoPasajero = entity.ApellidoPasajero,

                TipoDocumentoPasajero = entity.TipoDocumentoPasajero,
                NumeroDocumentoPasajero = entity.NumeroDocumentoPasajero,

                FechaNacimientoPasajero = entity.FechaNacimientoPasajero,
                NacionalidadPasajero = entity.NacionalidadPasajero,

                EmailContactoPasajero = entity.EmailContactoPasajero,
                TelefonoContactoPasajero = entity.TelefonoContactoPasajero,

                GeneroPasajero = entity.GeneroPasajero,

                RequiereAsistencia = entity.RequiereAsistencia,
                ObservacionesPasajero = entity.ObservacionesPasajero,

                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static PasajeroEntity ToEntity(PasajeroDataModel model)
        {
            if (model == null) return null;

            return new PasajeroEntity
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
                ObservacionesPasajero = model.ObservacionesPasajero,

                Estado = model.Estado
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(PasajeroEntity entity, PasajeroDataModel model)
        {
            entity.IdCliente = model.IdCliente;

            entity.NombrePasajero = model.NombrePasajero;
            entity.ApellidoPasajero = model.ApellidoPasajero;

            entity.TipoDocumentoPasajero = model.TipoDocumentoPasajero;
            entity.NumeroDocumentoPasajero = model.NumeroDocumentoPasajero;

            entity.FechaNacimientoPasajero = model.FechaNacimientoPasajero;
            entity.NacionalidadPasajero = model.NacionalidadPasajero;

            entity.EmailContactoPasajero = model.EmailContactoPasajero;
            entity.TelefonoContactoPasajero = model.TelefonoContactoPasajero;

            entity.GeneroPasajero = model.GeneroPasajero;

            entity.RequiereAsistencia = model.RequiereAsistencia;
            entity.ObservacionesPasajero = model.ObservacionesPasajero;

            entity.Estado = model.Estado;

            // ❗ NO tocar IdPasajero
        }

        // 🔹 Lista Entity → DataModel
        public static IEnumerable<PasajeroDataModel> ToDataModelList(IEnumerable<PasajeroEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        // 🔹 Lista DataModel → Entity
        public static IEnumerable<PasajeroEntity> ToEntityList(IEnumerable<PasajeroDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}