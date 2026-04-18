using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers
{
    internal static class ClienteDataMapper
    {
        // 🔹 Entity → DataModel
        public static ClienteDataModel ToDataModel(ClienteEntity entity)
        {
            if (entity == null) return null;

            return new ClienteDataModel
            {
                IdCliente = entity.IdCliente,
                ClienteGuid = entity.ClienteGuid,

                // 🪪 Identificación
                TipoIdentificacion = entity.TipoIdentificacion,
                NumeroIdentificacion = entity.NumeroIdentificacion,

                // 👤 Datos
                Nombres = entity.Nombres,
                Apellidos = entity.Apellidos,
                RazonSocial = entity.RazonSocial,

                // 📞 Contacto
                Correo = entity.Correo,
                Telefono = entity.Telefono,
                Direccion = entity.Direccion,

                // 🌍 Ubicación
                IdCiudadResidencia = entity.IdCiudadResidencia,
                IdPaisNacionalidad = entity.IdPaisNacionalidad,

                // 📅 Datos adicionales
                FechaNacimiento = entity.FechaNacimiento,
                Nacionalidad = entity.Nacionalidad,
                Genero = entity.Genero,

                // 📊 Estado
                Estado = entity.Estado,

                // 🔗 Integración
                ServicioOrigen = entity.ServicioOrigen,

                // ⚠️ Opcionales
                FechaInhabilitacionUtc = entity.FechaInhabilitacionUtc,
                MotivoInhabilitacion = entity.MotivoInhabilitacion
            };
        }

        // 🔹 DataModel → Entity
        public static ClienteEntity ToEntity(ClienteDataModel model)
        {
            if (model == null) return null;

            return new ClienteEntity
            {
                IdCliente = model.IdCliente,
                ClienteGuid = model.ClienteGuid,

                TipoIdentificacion = model.TipoIdentificacion,
                NumeroIdentificacion = model.NumeroIdentificacion,

                Nombres = model.Nombres,
                Apellidos = model.Apellidos,
                RazonSocial = model.RazonSocial,

                Correo = model.Correo,
                Telefono = model.Telefono,
                Direccion = model.Direccion,

                IdCiudadResidencia = model.IdCiudadResidencia,
                IdPaisNacionalidad = model.IdPaisNacionalidad,

                FechaNacimiento = model.FechaNacimiento,
                Nacionalidad = model.Nacionalidad,
                Genero = model.Genero,

                Estado = model.Estado,

                ServicioOrigen = model.ServicioOrigen,

                FechaInhabilitacionUtc = model.FechaInhabilitacionUtc,
                MotivoInhabilitacion = model.MotivoInhabilitacion
            };
        }

        // 🔹 Update controlado
        public static void UpdateEntity(ClienteEntity entity, ClienteDataModel model)
        {
            entity.TipoIdentificacion = model.TipoIdentificacion;
            entity.NumeroIdentificacion = model.NumeroIdentificacion;

            entity.Nombres = model.Nombres;
            entity.Apellidos = model.Apellidos;
            entity.RazonSocial = model.RazonSocial;

            entity.Correo = model.Correo;
            entity.Telefono = model.Telefono;
            entity.Direccion = model.Direccion;

            entity.IdCiudadResidencia = model.IdCiudadResidencia;
            entity.IdPaisNacionalidad = model.IdPaisNacionalidad;

            entity.FechaNacimiento = model.FechaNacimiento;
            entity.Nacionalidad = model.Nacionalidad;
            entity.Genero = model.Genero;

            entity.Estado = model.Estado;

            entity.ServicioOrigen = model.ServicioOrigen;

            entity.FechaInhabilitacionUtc = model.FechaInhabilitacionUtc;
            entity.MotivoInhabilitacion = model.MotivoInhabilitacion;

            // ❗ NO tocar:
            // IdCliente
            // ClienteGuid
        }

        // 🔹 Listas
        public static IEnumerable<ClienteDataModel> ToDataModelList(IEnumerable<ClienteEntity> entities)
        {
            return entities?.Select(ToDataModel).ToList();
        }

        public static IEnumerable<ClienteEntity> ToEntityList(IEnumerable<ClienteDataModel> models)
        {
            return models?.Select(ToEntity).ToList();
        }
    }
}