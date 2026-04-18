using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class AuditoriaLogDataService : IAuditoriaLogDataService
    {
        private readonly IUnitOfWork _uow;

        public AuditoriaLogDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AuditoriaLogDataModel>> GetAllAsync()
        {
            var entities = await _uow.AuditoriaLogRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AuditoriaLogDataModel>();

            return AuditoriaLogDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<AuditoriaLogDataModel> GetByIdAsync(long id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de auditoría debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.AuditoriaLogRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return AuditoriaLogDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY TABLA
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AuditoriaLogDataModel>> GetByTablaAsync(string tabla)
        {
            if (string.IsNullOrWhiteSpace(tabla))
                throw new ArgumentException(
                    "El nombre de la tabla no puede estar vacío.",
                    nameof(tabla));

            var entities = await _uow.AuditoriaLogRepository.GetByTablaAsync(tabla.Trim());

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AuditoriaLogDataModel>();

            return AuditoriaLogDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY OPERACION
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AuditoriaLogDataModel>> GetByOperacionAsync(string operacion)
        {
            if (string.IsNullOrWhiteSpace(operacion))
                throw new ArgumentException(
                    "La operación no puede estar vacía.",
                    nameof(operacion));

            // Validar que sea una operación válida según el CHECK de la BD
            var operacionesValidas = new[] { "INSERT", "UPDATE", "DELETE" };

            if (!operacionesValidas.Contains(operacion.ToUpper()))
                throw new ArgumentException(
                    $"Operación inválida. Las válidas son: {string.Join(", ", operacionesValidas)}",
                    nameof(operacion));

            var entities = await _uow.AuditoriaLogRepository
                                     .GetByOperacionAsync(operacion.ToUpper());

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AuditoriaLogDataModel>();

            return AuditoriaLogDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY USUARIO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AuditoriaLogDataModel>> GetByUsuarioAsync(string usuario)
        {
            if (string.IsNullOrWhiteSpace(usuario))
                throw new ArgumentException(
                    "El usuario no puede estar vacío.",
                    nameof(usuario));

            var entities = await _uow.AuditoriaLogRepository
                                     .GetByUsuarioAsync(usuario.Trim());

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AuditoriaLogDataModel>();

            return AuditoriaLogDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY FECHA
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AuditoriaLogDataModel>> GetByFechaAsync(
            DateTime fechaInicio,
            DateTime fechaFin)
        {
            if (fechaInicio == default)
                throw new ArgumentException(
                    "La fecha de inicio no es válida.",
                    nameof(fechaInicio));

            if (fechaFin == default)
                throw new ArgumentException(
                    "La fecha de fin no es válida.",
                    nameof(fechaFin));

            if (fechaInicio > fechaFin)
                throw new ArgumentException(
                    "La fecha de inicio no puede ser mayor a la fecha de fin.");

            // Normalizar a UTC
            var inicioUtc = fechaInicio.Kind == DateTimeKind.Utc
                ? fechaInicio
                : fechaInicio.ToUniversalTime();

            var finUtc = fechaFin.Kind == DateTimeKind.Utc
                ? fechaFin
                : fechaFin.ToUniversalTime();

            var entities = await _uow.AuditoriaLogRepository
                                     .GetByFechaAsync(inicioUtc, finUtc);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AuditoriaLogDataModel>();

            return AuditoriaLogDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET PAGED — con filtros
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<AuditoriaLogDataModel>> GetPagedAsync(
            AuditoriaLogFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(
                    nameof(filtro),
                    "El filtro no puede ser nulo.");

            // Asegurar paginación válida
            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            // Traer todos desde el repositorio
            var todos = await _uow.AuditoriaLogRepository.GetAllAsync();

            // ── Aplicar filtros en memoria ──────────────────
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.TablaAfectada))
                query = query.Where(a =>
                    a.TablaAfectada.ToUpper()
                     .Contains(filtro.TablaAfectada.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Operacion))
                query = query.Where(a =>
                    a.Operacion.ToUpper() == filtro.Operacion.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.IdRegistroAfectado))
                query = query.Where(a =>
                    a.IdRegistroAfectado == filtro.IdRegistroAfectado.Trim());

            if (!string.IsNullOrWhiteSpace(filtro.UsuarioEjecutor))
                query = query.Where(a =>
                    a.UsuarioEjecutor.ToUpper()
                     .Contains(filtro.UsuarioEjecutor.ToUpper()));

            if (filtro.Activo.HasValue)
                query = query.Where(a => a.Activo == filtro.Activo.Value);

            // Filtro por rango de fechas
            if (filtro.FechaInicio.HasValue)
            {
                var inicioUtc = filtro.FechaInicio.Value.Kind == DateTimeKind.Utc
                    ? filtro.FechaInicio.Value
                    : filtro.FechaInicio.Value.ToUniversalTime();

                query = query.Where(a => a.FechaEventoUtc >= inicioUtc);
            }

            if (filtro.FechaFin.HasValue)
            {
                var finUtc = filtro.FechaFin.Value.Kind == DateTimeKind.Utc
                    ? filtro.FechaFin.Value
                    : filtro.FechaFin.Value.ToUniversalTime();

                query = query.Where(a => a.FechaEventoUtc <= finUtc);
            }

            // Ordenar por fecha descendente (más reciente primero)
            query = query.OrderByDescending(a => a.FechaEventoUtc);

            // ── Paginación ──────────────────────────────────
            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(AuditoriaLogDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<AuditoriaLogDataModel>
            {
                Data = items,
                Meta = new MetaData
                {
                    Page = filtro.Page,
                    PageSize = filtro.PageSize,
                    Total = total,
                    TotalPages = totalPages
                }
            };
        }

        // ─────────────────────────────────────────────
        // CREATE — único método de escritura
        // ─────────────────────────────────────────────
        public async Task<AuditoriaLogDataModel> CreateAsync(AuditoriaLogDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo de auditoría no puede ser nulo.");

            // ── Validaciones obligatorias ───────────────────
            if (string.IsNullOrWhiteSpace(model.TablaAfectada))
                throw new ArgumentException(
                    "La tabla afectada es obligatoria.");

            if (string.IsNullOrWhiteSpace(model.Operacion))
                throw new ArgumentException(
                    "La operación es obligatoria.");

            var operacionesValidas = new[] { "INSERT", "UPDATE", "DELETE" };

            if (!operacionesValidas.Contains(model.Operacion.ToUpper()))
                throw new ArgumentException(
                    $"Operación inválida. Las válidas son: {string.Join(", ", operacionesValidas)}");

            // ── Construir la entidad ────────────────────────
            var entity = AuditoriaLogDataMapper.ToEntity(model);

            // Campos que siempre se generan aquí, no desde el modelo
            entity.AuditoriaGuid = Guid.NewGuid();
            entity.FechaEventoUtc = DateTime.UtcNow;
            entity.Operacion = model.Operacion.ToUpper();
            entity.TablaAfectada = model.TablaAfectada.Trim();
            entity.Activo = true;

            // Normalizar usuario ejecutor
            entity.UsuarioEjecutor = string.IsNullOrWhiteSpace(model.UsuarioEjecutor)
                ? "SYSTEM"
                : model.UsuarioEjecutor.Trim();

            // Persistir — sin SaveChanges en cascada, 
            // la auditoría debe ser atómica
            await _uow.AuditoriaLogRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return AuditoriaLogDataMapper.ToDataModel(entity);
        }
    }
}