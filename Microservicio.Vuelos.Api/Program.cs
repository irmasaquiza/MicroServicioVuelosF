using Microservicio.Vuelos.Api.Extensions;
using Microservicio.Vuelos.Api.Middleware;

var builder = WebApplication.CreateBuilder(args);

// ============================================================
// 🔥 SERVICES
// ============================================================

// Controllers
builder.Services.AddControllers();

// 🔥 EXTENSIONS
builder.Services.AddProjectServices(builder.Configuration); builder.Services.AddSwaggerExtension(); // Swagger + JWT
builder.Services.AddApiVersioningExtension(); // Versionado
builder.Services.AddAuthenticationExtension(builder.Configuration); // JWT
builder.Services.AddCorsExtension(builder.Configuration); // CORS

// ============================================================
// 🔥 BUILD APP
// ============================================================

var app = builder.Build();

// ============================================================
// 🔥 MIDDLEWARE PIPELINE
// ============================================================

// Swagger (solo en desarrollo recomendado)
if (app.Environment.IsDevelopment())
{
    app.UseSwaggerExtension();
}

// Middleware global de errores
app.UseMiddleware<ExceptionHandlingMiddleware>();

app.UseHttpsRedirection();

// CORS
app.UseCorsExtension();

// Auth
app.UseAuthentication();
app.UseAuthorization();

// Controllers
app.MapControllers();



// ============================================================
// 🔥 RUN
// ============================================================

app.Run();