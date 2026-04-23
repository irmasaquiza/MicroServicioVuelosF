/*using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    public static class MetodoPagoDataMapper
    {
        // 🔹 Entity → DataModel
        public static MetodoPagoDataModel ToDataModel(MetodoPagoEntity entity)
        {
            if (entity == null) return null;

            return new MetodoPagoDataModel
            {
                IdMetodo = entity.IdMetodo,
                IdCliente = entity.IdCliente,
                IdTipoMetodo = entity.IdTipoMetodo,

                // 🔥 FIX
                TokenPasarela = entity.TokenPasarela,

                Ultimos4 = entity.Ultimos4,
                ReferenciaVisible = entity.ReferenciaVisible,

                FechaExpiracion = entity.FechaExpiracion,

                NombreTitular = entity.NombreTitular,
                MarcaTarjeta = entity.MarcaTarjeta,
                BancoEmisor = entity.BancoEmisor,
                PaisEmision = entity.PaisEmision,

                EsPrincipal = entity.EsPrincipal,
                Alias = entity.Alias,

                FechaUltimoUso = entity.FechaUltimoUso,

                Estado = entity.Estado
            };
        }

        // 🔹 DataModel → Entity
        public static MetodoPagoEntity ToEntity(MetodoPagoDataModel model)
        {
            if (model == null) return null;

            return new MetodoPagoEntity
            {
                IdMetodo = model.IdMetodo,
                IdCliente = model.IdCliente,
                IdTipoMetodo = model.IdTipoMetodo,

                // 🔥 FIX CRÍTICO
                TokenPasarela = model.TokenPasarela,

                Ultimos4 = model.Ultimos4,
                ReferenciaVisible = model.ReferenciaVisible,

                FechaExpiracion = model.FechaExpiracion,

                NombreTitular = model.NombreTitular,
                MarcaTarjeta = model.MarcaTarjeta,
                BancoEmisor = model.BancoEmisor,
                PaisEmision = model.PaisEmision,

                EsPrincipal = model.EsPrincipal,
                Alias = model.Alias,

                FechaUltimoUso = model.FechaUltimoUso,

                Estado = model.Estado ?? "ACTIVO"
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(MetodoPagoEntity entity, MetodoPagoDataModel model)
        {
            entity.Ultimos4 = model.Ultimos4;
            entity.ReferenciaVisible = model.ReferenciaVisible;

            entity.FechaExpiracion = model.FechaExpiracion;

            entity.NombreTitular = model.NombreTitular;
            entity.MarcaTarjeta = model.MarcaTarjeta;
            entity.BancoEmisor = model.BancoEmisor;
            entity.PaisEmision = model.PaisEmision;

            entity.EsPrincipal = model.EsPrincipal;
            entity.Alias = model.Alias;

            entity.FechaUltimoUso = model.FechaUltimoUso;

            entity.Estado = model.Estado ?? entity.Estado;

            // 🔥 IMPORTANTE
            if (!string.IsNullOrWhiteSpace(model.TokenPasarela))
                entity.TokenPasarela = model.TokenPasarela;
        }

        public static IEnumerable<MetodoPagoDataModel> ToDataModelList(IEnumerable<MetodoPagoEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        public static IEnumerable<MetodoPagoEntity> ToEntityList(IEnumerable<MetodoPagoDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}*/