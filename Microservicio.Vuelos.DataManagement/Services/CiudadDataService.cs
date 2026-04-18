// ============================================================
// CiudadDataService.cs
// ============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class CiudadDataService : ICiudadDataService
    {
        private readonly IUnitOfWork _uow;

        public CiudadDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<CiudadDataModel>> GetAllAsync()
        {
            var entities = await _uow.CiudadRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<CiudadDataModel>();

            return CiudadDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<CiudadDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la ciudad debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.CiudadRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return CiudadDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY PAIS
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<CiudadDataModel>> GetByPaisAsync(int idPais)
        {
            if (idPais <= 0)
                throw new ArgumentException(
                    "El ID del país debe ser mayor a 0.",
                    nameof(idPais));

            var entities = await _uow.CiudadRepository.GetByPaisAsync(idPais);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<CiudadDataModel>();

            return CiudadDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET PAGED — con filtros
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<CiudadDataModel>> GetPagedAsync(
            CiudadFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(
                    nameof(filtro),
                    "El filtro no puede ser nulo.");

            // Asegurar paginación válida
            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            // Traer todos los no eliminados
            var todos = await _uow.CiudadRepository.GetAllAsync();

            // ── Aplicar filtros en memoria ──────────────────
            var query = todos.AsQueryable();

            if (filtro.IdPais.HasValue)
                query = query.Where(c => c.IdPais == filtro.IdPais.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                query = query.Where(c =>
                    c.Nombre.ToUpper().Contains(filtro.Nombre.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.CodigoPostal))
                query = query.Where(c =>
                    c.CodigoPostal != null &&
                    c.CodigoPostal.ToUpper()
                     .Contains(filtro.CodigoPostal.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.ZonaHoraria))
                query = query.Where(c =>
                    c.ZonaHoraria != null &&
                    c.ZonaHoraria.ToUpper()
                     .Contains(filtro.ZonaHoraria.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(c =>
                    c.Estado.ToUpper() == filtro.Estado.ToUpper());

            // ── Filtros geográficos opcionales ──────────────
            if (filtro.LatitudMin.HasValue)
                query = query.Where(c =>
                    c.Latitud.HasValue &&
                    c.Latitud.Value >= filtro.LatitudMin.Value);

            if (filtro.LatitudMax.HasValue)
                query = query.Where(c =>
                    c.Latitud.HasValue &&
                    c.Latitud.Value <= filtro.LatitudMax.Value);

            if (filtro.LongitudMin.HasValue)
                query = query.Where(c =>
                    c.Longitud.HasValue &&
                    c.Longitud.Value >= filtro.LongitudMin.Value);

            if (filtro.LongitudMax.HasValue)
                query = query.Where(c =>
                    c.Longitud.HasValue &&
                    c.Longitud.Value <= filtro.LongitudMax.Value);

            // Ordenar por nombre
            query = query.OrderBy(c => c.Nombre);

            // ── Paginación ──────────────────────────────────
            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(CiudadDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<CiudadDataModel>
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
        // CREATE
        // ─────────────────────────────────────────────
        public async Task<CiudadDataModel> CreateAsync(CiudadDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo de ciudad no puede ser nulo.");

            // ── Validaciones de negocio ─────────────────────
            if (string.IsNullOrWhiteSpace(model.Nombre))
                throw new ArgumentException(
                    "El nombre de la ciudad es obligatorio.");

            if (model.IdPais <= 0)
                throw new ArgumentException(
                    "El ID del país es obligatorio.");

            // Verificar que el país exista
            var pais = await _uow.PaisRepository.GetByIdAsync(model.IdPais);

            if (pais == null)
                throw new InvalidOperationException(
                    $"No existe un país con ID '{model.IdPais}'.");

            // Verificar unicidad Nombre + País (UQ_Ciudad_Nombre_Pais en BD)
            var existentes = await _uow.CiudadRepository
                                       .GetByNombreAsync(model.Nombre);

            var duplicada = existentes?.FirstOrDefault(c =>
                c.IdPais == model.IdPais &&
                c.Nombre.ToUpper() == model.Nombre.ToUpper());

            if (duplicada != null)
                throw new InvalidOperationException(
                    $"Ya existe la ciudad '{model.Nombre}' en el país con ID '{model.IdPais}'.");

            // Validar coordenadas si vienen
            ValidarCoordenadas(model.Latitud, model.Longitud);

            // ── Construir entidad ───────────────────────────
            var entity = CiudadDataMapper.ToEntity(model);

            // Campos de auditoría
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.Eliminado = false;
            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                                          ? "ACTIVO"
                                          : model.Estado.ToUpper();

            // Persistir
            await _uow.CiudadRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return CiudadDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(CiudadDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo de ciudad no puede ser nulo.");

            if (model.IdCiudad <= 0)
                throw new ArgumentException(
                    "El ID de la ciudad debe ser mayor a 0.");

            // Buscar entidad existente
            var entity = await _uow.CiudadRepository.GetByIdAsync(model.IdCiudad);

            if (entity == null)
                return false;

            // Verificar unicidad si cambió el nombre
            if (!string.IsNullOrWhiteSpace(model.Nombre) &&
                model.Nombre.ToUpper() != entity.Nombre.ToUpper())
            {
                var existentes = await _uow.CiudadRepository
                                           .GetByNombreAsync(model.Nombre);

                var duplicada = existentes?.FirstOrDefault(c =>
                    c.IdPais == entity.IdPais &&
                    c.IdCiudad != model.IdCiudad &&
                    c.Nombre.ToUpper() == model.Nombre.ToUpper());

                if (duplicada != null)
                    throw new InvalidOperationException(
                        $"Ya existe la ciudad '{model.Nombre}' en ese país.");
            }

            // Validar coordenadas si vienen
            ValidarCoordenadas(model.Latitud, model.Longitud);

            // Aplicar cambios — UpdateEntity NO toca IdPais
            CiudadDataMapper.UpdateEntity(entity, model);

            // Auditoría de modificación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.CiudadRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // DELETE — eliminación lógica
        // ─────────────────────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la ciudad debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.CiudadRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            // Soft delete via repositorio
            _uow.CiudadRepository.Delete(entity);

            // Auditoría de eliminación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // PRIVADO — Validar coordenadas
        // ─────────────────────────────────────────────
        private static void ValidarCoordenadas(decimal? latitud, decimal? longitud)
        {
            if (latitud.HasValue && (latitud.Value < -90 || latitud.Value > 90))
                throw new ArgumentException(
                    "La latitud debe estar entre -90 y 90.");

            if (longitud.HasValue && (longitud.Value < -180 || longitud.Value > 180))
                throw new ArgumentException(
                    "La longitud debe estar entre -180 y 180.");
        }
    }
}