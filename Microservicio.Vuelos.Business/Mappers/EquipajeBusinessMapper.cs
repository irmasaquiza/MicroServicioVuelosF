using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Mappers
{
    public static class EquipajeBusinessMapper
    {
        // ============================================================
        // 🔄 Crear → DataModel
        // ============================================================
        public static EquipajeDataModel ToDataModel(CrearEquipajeRequest request)
        {
            if (request == null) return null;

            return new EquipajeDataModel
            {
                IdBoleto = request.IdBoleto,
                Tipo = request.Tipo,
                PesoKg = request.PesoKg,
                DescripcionEquipaje = request.DescripcionEquipaje,
                PrecioExtra = request.PrecioExtra,
                DimensionesCm = request.DimensionesCm,

                // 💀 defaults
                EstadoEquipaje = "REGISTRADO"
                // NumeroEtiqueta lo genera BD o service
            };
        }

        // ============================================================
        // 🔄 Actualizar → DataModel (PATCH solo estado)
        // ============================================================
        public static EquipajeDataModel ToDataModel(ActualizarEquipajeRequest request)
        {
            if (request == null) return null;

            var model = new EquipajeDataModel();

            if (!string.IsNullOrWhiteSpace(request.EstadoEquipaje))
                model.EstadoEquipaje = request.EstadoEquipaje;

            return model;
        }

        // ============================================================
        // 🔄 DataModel → Response
        // ============================================================
        public static EquipajeResponse ToResponse(EquipajeDataModel model)
        {
            if (model == null) return null;

            return new EquipajeResponse
            {
                IdEquipaje = model.IdEquipaje,
                IdBoleto = model.IdBoleto,
                Tipo = model.Tipo,
                PesoKg = model.PesoKg,
                DescripcionEquipaje = model.DescripcionEquipaje,
                PrecioExtra = model.PrecioExtra,
                DimensionesCm = model.DimensionesCm,
                NumeroEtiqueta = model.NumeroEtiqueta,
                EstadoEquipaje = model.EstadoEquipaje
            };
        }

        // ============================================================
        // 🔄 Lista → ResponseList
        // ============================================================
        public static IEnumerable<EquipajeResponse> ToResponseList(IEnumerable<EquipajeDataModel> models)
        {
            return models?.Select(ToResponse);
        }
    }
}