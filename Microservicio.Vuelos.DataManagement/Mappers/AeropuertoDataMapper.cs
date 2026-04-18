using System;
using System.Collections.Generic;
using System.Text;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Mappers;

public static class AeropuertoDataMapper
{
    // 🔹 Entity → DataModel
    public static AeropuertoDataModel ToDataModel(AeropuertoEntity entity)
    {
        return new AeropuertoDataModel
        {
            IdAeropuerto = entity.IdAeropuerto,

            CodigoIata = entity.CodigoIata,
            CodigoIcao = entity.CodigoIcao,
            Nombre = entity.Nombre,

            Latitud = entity.Latitud,
            Longitud = entity.Longitud,
            ZonaHoraria = entity.ZonaHoraria,

            IdCiudad = entity.IdCiudad,
            IdPais = entity.IdPais,

            Estado = entity.Estado
        };
    }

    // 🔹 DataModel → Entity
    public static AeropuertoEntity ToEntity(AeropuertoDataModel model)
    {
        return new AeropuertoEntity
        {
            IdAeropuerto = model.IdAeropuerto,

            CodigoIata = model.CodigoIata,
            CodigoIcao = model.CodigoIcao,
            Nombre = model.Nombre,

            Latitud = model.Latitud,
            Longitud = model.Longitud,
            ZonaHoraria = model.ZonaHoraria,

            IdCiudad = model.IdCiudad,
            IdPais = model.IdPais,

            Estado = model.Estado
        };
    }

    // 🔹 Update controlado
    public static void UpdateEntity(AeropuertoEntity entity, AeropuertoDataModel model)
    {
        entity.CodigoIata = model.CodigoIata;
        entity.CodigoIcao = model.CodigoIcao;
        entity.Nombre = model.Nombre;

        entity.Latitud = model.Latitud;
        entity.Longitud = model.Longitud;
        entity.ZonaHoraria = model.ZonaHoraria;

        entity.IdCiudad = model.IdCiudad;
        entity.IdPais = model.IdPais;

        entity.Estado = model.Estado;
    }
}