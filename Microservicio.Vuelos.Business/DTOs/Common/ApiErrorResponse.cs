// ============================================================
// ApiErrorResponse.cs
// ============================================================
using System.Collections.Generic;

namespace Microservicio.Vuelos.Business.DTOs.Common
{
    /// <summary>
    /// Respuesta estándar de error para la API.
    /// Compatible con todas las excepciones de negocio.
    /// HTTP 400 / 401 / 403 / 404 / 409 / 422 / 500
    /// </summary>
    public class ApiErrorResponse
    {
        // ─────────────────────────────────────────────
        // Indica si la operación fue exitosa
        // Siempre false en esta clase
        // ─────────────────────────────────────────────
        public bool Success { get; set; } = false;

        // ─────────────────────────────────────────────
        // Código del error
        // Ej: VALIDACION_ERROR, RECURSO_NO_ENCONTRADO
        // ─────────────────────────────────────────────
        public string Codigo { get; set; }

        // ─────────────────────────────────────────────
        // Mensaje principal del error
        // ─────────────────────────────────────────────
        public string Mensaje { get; set; }

        // ─────────────────────────────────────────────
        // Errores por campo (principalmente para validaciones)
        // Ej: { "correo": ["Formato inválido"],
        //       "nombres": ["El campo es requerido"] }
        // ─────────────────────────────────────────────
        public Dictionary<string, string[]> Errores { get; set; }

        // ─────────────────────────────────────────────
        // Constructor vacío
        // ─────────────────────────────────────────────
        public ApiErrorResponse()
        {
            Success = false;
            Errores = new Dictionary<string, string[]>();
        }

        // ─────────────────────────────────────────────
        // Constructor básico — sin errores de campo
        // Para NotFoundException, BusinessException,
        // UnauthorizedBusinessException
        // ─────────────────────────────────────────────
        public ApiErrorResponse(string codigo, string mensaje)
        {
            Success = false;
            Codigo = codigo;
            Mensaje = mensaje;
            Errores = new Dictionary<string, string[]>();
        }

        // ─────────────────────────────────────────────
        // Constructor con errores por campo
        // Para ValidationException
        // ─────────────────────────────────────────────
        public ApiErrorResponse(string codigo, string mensaje,
            Dictionary<string, string[]> errores)
        {
            Success = false;
            Codigo = codigo;
            Mensaje = mensaje;
            Errores = errores ?? new Dictionary<string, string[]>();
        }

        // ─────────────────────────────────────────────
        // Factory — desde ValidationException
        // ─────────────────────────────────────────────
        public static ApiErrorResponse FromValidation(
            Exceptions.ValidationException ex)
        {
            return new ApiErrorResponse(ex.Codigo, ex.Message, ex.Errores);
        }

        // ─────────────────────────────────────────────
        // Factory — desde NotFoundException (404)
        // ─────────────────────────────────────────────
        public static ApiErrorResponse FromNotFound(
            Exceptions.NotFoundException ex)
        {
            return new ApiErrorResponse(ex.Codigo, ex.Message);
        }

        // ─────────────────────────────────────────────
        // Factory — desde BusinessException (422)
        // ─────────────────────────────────────────────
        public static ApiErrorResponse FromBusiness(
            Exceptions.BusinessException ex)
        {
            return new ApiErrorResponse(ex.CodigoError, ex.Message);
        }

        // ─────────────────────────────────────────────
        // Factory — desde UnauthorizedBusinessException
        // (401 o 403)
        // ─────────────────────────────────────────────
        public static ApiErrorResponse FromUnauthorized(
            Exceptions.UnauthorizedBusinessException ex)
        {
            return new ApiErrorResponse(ex.Codigo, ex.Message);
        }

        // ─────────────────────────────────────────────
        // Factory — error simple sin exception
        // Para casos rápidos en el controller
        // ─────────────────────────────────────────────
        public static ApiErrorResponse Fail(string codigo, string mensaje)
        {
            return new ApiErrorResponse(codigo, mensaje);
        }

        // ─────────────────────────────────────────────
        // Factory — error interno del servidor (500)
        // Nunca expone detalles técnicos
        // ─────────────────────────────────────────────
        public static ApiErrorResponse ErrorInterno()
        {
            return new ApiErrorResponse(
                "ERROR_INTERNO",
                "Ocurrió un error interno. Por favor intente más tarde.");
        }
    }
}