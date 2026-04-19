using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class EquipajeValidator
    {
        private static readonly string[] TIPOS_VALIDOS =
        {
            "MANO",
            "BODEGA"
        };

        private static readonly string[] ESTADOS_VALIDOS =
        {
            "REGISTRADO",
            "EMBARCADO",
            "EN_TRANSITO",
            "ENTREGADO",
            "CANCELADO",
            "PERDIDO",
            "DAÑADO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearEquipajeRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Boleto
            if (request.IdBoleto <= 0)
                errors.Add("El boleto es obligatorio");

            // Tipo
            if (string.IsNullOrWhiteSpace(request.Tipo))
                errors.Add("El tipo de equipaje es obligatorio");
            else if (!TIPOS_VALIDOS.Contains(request.Tipo))
                errors.Add("El tipo debe ser MANO o BODEGA");

            // Peso
            if (request.PesoKg <= 0)
                errors.Add("El peso debe ser mayor a 0");

            // Regla negocio: equipaje de mano ≤ 10kg
            if (request.Tipo == "MANO" && request.PesoKg > 10)
                errors.Add("El equipaje de mano no puede superar los 10 kg");

            // Precio
            if (request.PrecioExtra < 0)
                errors.Add("El precio extra no puede ser negativo");

            // Descripción
            if (!string.IsNullOrWhiteSpace(request.DescripcionEquipaje) &&
                request.DescripcionEquipaje.Length > 200)
                errors.Add("La descripción no puede superar 200 caracteres");

            // Dimensiones
            if (!string.IsNullOrWhiteSpace(request.DimensionesCm) &&
                request.DimensionesCm.Length > 50)
                errors.Add("Las dimensiones no pueden superar 50 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN (solo estado)
        // ============================================================
        public static void ValidarActualizar(ActualizarEquipajeRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            if (string.IsNullOrWhiteSpace(request.EstadoEquipaje))
                errors.Add("El estado del equipaje es obligatorio");
            else if (!ESTADOS_VALIDOS.Contains(request.EstadoEquipaje))
                errors.Add("Estado inválido para el equipaje");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}