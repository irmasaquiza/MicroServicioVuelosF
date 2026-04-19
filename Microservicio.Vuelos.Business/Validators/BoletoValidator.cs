using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class BoletoValidator
    {
        private static readonly string[] CLASES_VALIDAS =
        {
            "ECONOMICA",
            "EJECUTIVA",
            "PRIMERA"
        };

        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ACTIVO",
            "USADO",
            "CANCELADO"
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearBoletoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // IDs
            if (request.IdReserva <= 0)
                errors.Add("La reserva es obligatoria");

            if (request.IdVuelo <= 0)
                errors.Add("El vuelo es obligatorio");

            if (request.IdAsiento <= 0)
                errors.Add("El asiento es obligatorio");

            if (request.IdFactura <= 0)
                errors.Add("La factura es obligatoria");

            // Clase
            if (string.IsNullOrWhiteSpace(request.Clase))
                errors.Add("La clase es obligatoria");
            else if (!CLASES_VALIDAS.Contains(request.Clase))
                errors.Add("La clase debe ser ECONOMICA, EJECUTIVA o PRIMERA");

            // Precios
            if (request.PrecioVueloBase < 0)
                errors.Add("El precio base no puede ser negativo");

            if (request.PrecioAsientoExtra < 0)
                errors.Add("El precio extra del asiento no puede ser negativo");

            if (request.ImpuestosBoleto < 0)
                errors.Add("Los impuestos no pueden ser negativos");

            if (request.CargoEquipaje < 0)
                errors.Add("El cargo de equipaje no puede ser negativo");

            if (request.PrecioFinal < 0)
                errors.Add("El precio final no puede ser negativo");

            // Coherencia de precio
            var sumaMinima = request.PrecioVueloBase +
                             request.PrecioAsientoExtra +
                             request.ImpuestosBoleto +
                             request.CargoEquipaje;

            if (request.PrecioFinal < sumaMinima)
                errors.Add("El precio final no puede ser menor a la suma de sus componentes");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarBoletoRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Clase
            if (!string.IsNullOrWhiteSpace(request.Clase) &&
                !CLASES_VALIDAS.Contains(request.Clase))
                errors.Add("La clase debe ser ECONOMICA, EJECUTIVA o PRIMERA");

            // Precios
            if (request.PrecioVueloBase.HasValue && request.PrecioVueloBase < 0)
                errors.Add("El precio base no puede ser negativo");

            if (request.PrecioAsientoExtra.HasValue && request.PrecioAsientoExtra < 0)
                errors.Add("El precio extra no puede ser negativo");

            if (request.ImpuestosBoleto.HasValue && request.ImpuestosBoleto < 0)
                errors.Add("Los impuestos no pueden ser negativos");

            if (request.CargoEquipaje.HasValue && request.CargoEquipaje < 0)
                errors.Add("El cargo de equipaje no puede ser negativo");

            if (request.PrecioFinal.HasValue && request.PrecioFinal < 0)
                errors.Add("El precio final no puede ser negativo");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.EstadoBoleto) &&
                !ESTADOS_VALIDOS.Contains(request.EstadoBoleto))
                errors.Add("El estado debe ser ACTIVO, USADO o CANCELADO");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}