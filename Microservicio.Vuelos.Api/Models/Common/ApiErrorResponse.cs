using System.Collections.Generic;

namespace Microservicio.Vuelos.Api.Models.Common
{
    public class ApiErrorResponse
    {
        public bool Success { get; set; } = false;

        public string Message { get; set; } = string.Empty;

        public List<string>? Errors { get; set; }
    }
}