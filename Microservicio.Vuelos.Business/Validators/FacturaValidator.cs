using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;

using Microservicio.Vuelos.Business.DTOs.Internal.Factura;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Business.Validators
{
    public static class FacturaValidator
    {
        private static readonly string[] ESTADOS_VALIDOS =
        {
            "ABI", // Abierta
            "APR", // Aprobada
            "INA"  // Inactiva
        };

        // ============================================================
        // 🔥 VALIDAR CREACIÓN
        // ============================================================
        public static void ValidarCrear(CrearFacturaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // IDs
            if (request.IdCliente <= 0)
                errors.Add("El cliente es obligatorio");

            if (request.IdReserva <= 0)
                errors.Add("La reserva es obligatoria");

         //   if (request.IdMetodo <= 0)
         //       errors.Add("El método de pago es obligatorio");

            // Valores
            if (request.Subtotal < 0)
                errors.Add("El subtotal no puede ser negativo");

            if (request.ValorIva < 0)
                errors.Add("El IVA no puede ser negativo");

            if (request.CargoServicio < 0)
                errors.Add("El cargo de servicio no puede ser negativo");

            if (request.Total < 0)
                errors.Add("El total no puede ser negativo");

            // 💀 Regla de negocio clave
            var totalEsperado = request.Subtotal + request.ValorIva + request.CargoServicio;

            if (request.Total != totalEsperado)
                errors.Add("El total debe ser igual a Subtotal + IVA + Cargo de Servicio");

            // Textos
            if (!string.IsNullOrWhiteSpace(request.ObservacionesFactura) &&
                request.ObservacionesFactura.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (!string.IsNullOrWhiteSpace(request.OrigenCanalFactura) &&
                request.OrigenCanalFactura.Length > 50)
                errors.Add("El canal no puede superar 50 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }

        // ============================================================
        // 🔥 VALIDAR ACTUALIZACIÓN
        // ============================================================
        public static void ValidarActualizar(ActualizarFacturaRequest request)
        {
            var errors = new List<string>();

            if (request == null)
                throw new ValidationException("La solicitud es requerida.");

            // Valores
            if (request.Subtotal.HasValue && request.Subtotal < 0)
                errors.Add("El subtotal no puede ser negativo");

            if (request.ValorIva.HasValue && request.ValorIva < 0)
                errors.Add("El IVA no puede ser negativo");

            if (request.CargoServicio.HasValue && request.CargoServicio < 0)
                errors.Add("El cargo de servicio no puede ser negativo");

            if (request.Total.HasValue && request.Total < 0)
                errors.Add("El total no puede ser negativo");

            // Estado
            if (!string.IsNullOrWhiteSpace(request.Estado) &&
                !ESTADOS_VALIDOS.Contains(request.Estado))
                errors.Add("El estado debe ser ABI, APR o INA");

            // Textos
            if (!string.IsNullOrWhiteSpace(request.ObservacionesFactura) &&
                request.ObservacionesFactura.Length > 300)
                errors.Add("Las observaciones no pueden superar 300 caracteres");

            if (!string.IsNullOrWhiteSpace(request.OrigenCanalFactura) &&
                request.OrigenCanalFactura.Length > 50)
                errors.Add("El canal no puede superar 50 caracteres");

            if (errors.Any())
                throw new ValidationException(errors);
        }
    }
}