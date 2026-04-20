using System.Net;
using System.Text.Json;

using Microservicio.Vuelos.Api.Models.Common;
using Microservicio.Vuelos.Business.Exceptions;

namespace Microservicio.Vuelos.Api.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (BusinessException ex)
            {
                await HandleBusinessException(context, ex);
            }
            catch (Exception ex)
            {
                await HandleGeneralException(context, ex);
            }
        }

        // ============================================================
        // 🔥 ERRORES DE NEGOCIO
        // ============================================================
        private static async Task HandleBusinessException(HttpContext context, BusinessException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;

            var response = new
            {
                success = false,
                tipo = "BUSINESS_ERROR",
                mensaje = ex.Message,
                codigo = ex.CodigoError,
                detalle = ex.InnerException?.Message,
                stackTrace = ex.StackTrace
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }

        // ============================================================
        // 🔥 ERRORES GENERALES (DEBUG FULL 🔥)
        // ============================================================
        private static async Task HandleGeneralException(HttpContext context, Exception ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;

            var response = new
            {
                success = false,
                tipo = "ERROR_INTERNO",
                mensaje = ex.Message,
                detalle = ex.InnerException?.Message,
                stackTrace = ex.StackTrace,
                origen = ex.Source
            };

            var json = JsonSerializer.Serialize(response, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            await context.Response.WriteAsync(json);
        }
    }
}