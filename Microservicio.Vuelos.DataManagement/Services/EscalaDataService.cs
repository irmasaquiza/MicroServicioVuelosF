// ============================================================
// EscalaDataService.cs
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
    public class EscalaDataService : IEscalaDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] TiposEscalaValidos =
            { "TECNICA", "COMERCIAL" };

        public EscalaDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<EscalaDataModel>> GetAllAsync()
        {
            var entities = await _uow.EscalaRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<EscalaDataModel>();

            return EscalaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<EscalaDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la escala debe ser mayor a 0.", nameof(id));

            var entity = await _uow.EscalaRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return EscalaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY VUELO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<EscalaDataModel>> GetByVueloAsync(int idVuelo)
        {
            if (idVuelo <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.", nameof(idVuelo));

            var entities = await _uow.EscalaRepository.GetByVueloAsync(idVuelo);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<EscalaDataModel>();

            return EscalaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY AEROPUERTO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<EscalaDataModel>> GetByAeropuertoAsync(
            int idAeropuerto)
        {
            if (idAeropuerto <= 0)
                throw new ArgumentException(
                    "El ID del aeropuerto debe ser mayor a 0.",
                    nameof(idAeropuerto));

            var todos = await _uow.EscalaRepository.GetAllAsync();

            var filtrados = todos
                .Where(e => e.IdAeropuerto == idAeropuerto)
                .OrderBy(e => e.FechaHoraLlegada)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<EscalaDataModel>();

            return EscalaDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<EscalaDataModel>> GetPagedAsync(
            EscalaFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.EscalaRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdVuelo.HasValue)
                query = query.Where(e => e.IdVuelo == filtro.IdVuelo.Value);

            if (filtro.IdAeropuerto.HasValue)
                query = query.Where(e => e.IdAeropuerto == filtro.IdAeropuerto.Value);

            if (filtro.Orden.HasValue)
                query = query.Where(e => e.Orden == filtro.Orden.Value);

            if (!string.IsNullOrWhiteSpace(filtro.TipoEscala))
                query = query.Where(e =>
                    e.TipoEscala.ToUpper() == filtro.TipoEscala.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.Terminal))
                query = query.Where(e =>
                    e.Terminal != null &&
                    e.Terminal.ToUpper().Contains(filtro.Terminal.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Puerta))
                query = query.Where(e =>
                    e.Puerta != null &&
                    e.Puerta.ToUpper() == filtro.Puerta.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(e =>
                    e.Estado.ToUpper() == filtro.Estado.ToUpper());

            if (filtro.FechaInicio.HasValue)
                query = query.Where(e =>
                    e.FechaHoraLlegada >= filtro.FechaInicio.Value);

            if (filtro.FechaFin.HasValue)
                query = query.Where(e =>
                    e.FechaHoraSalida <= filtro.FechaFin.Value);

            if (filtro.DuracionMin.HasValue)
                query = query.Where(e =>
                    e.DuracionMin >= filtro.DuracionMin.Value);

            // Ordenar por vuelo y luego por orden de escala
            query = query.OrderBy(e => e.IdVuelo).ThenBy(e => e.Orden);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(EscalaDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<EscalaDataModel>
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
        public async Task<EscalaDataModel> CreateAsync(EscalaDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdVuelo <= 0)
                throw new ArgumentException("El ID del vuelo es obligatorio.");

            if (model.IdAeropuerto <= 0)
                throw new ArgumentException("El ID del aeropuerto es obligatorio.");

            if (model.Orden < 1)
                throw new ArgumentException(
                    "El orden de la escala debe ser mayor o igual a 1.");

            if (!string.IsNullOrWhiteSpace(model.TipoEscala) &&
                !TiposEscalaValidos.Contains(model.TipoEscala.ToUpper()))
                throw new ArgumentException(
                    $"Tipo de escala inválido. " +
                    $"Los válidos son: {string.Join(", ", TiposEscalaValidos)}");

            // CK_Escala_Fechas → salida > llegada
            if (model.FechaHoraSalida <= model.FechaHoraLlegada)
                throw new ArgumentException(
                    "La fecha/hora de salida debe ser posterior " +
                    "a la fecha/hora de llegada.");

            // CK_Escala_Duracion → duracion >= 0
            if (model.DuracionMin < 0)
                throw new ArgumentException(
                    "La duración no puede ser negativa.");

            // Verificar que el vuelo exista
            var vuelo = await _uow.VueloRepository.GetByIdAsync(model.IdVuelo);

            if (vuelo == null)
                throw new InvalidOperationException(
                    $"No existe un vuelo con ID '{model.IdVuelo}'.");

            // Verificar que el aeropuerto exista
            var aeropuerto = await _uow.AeropuertoRepository
                                       .GetByIdAsync(model.IdAeropuerto);

            if (aeropuerto == null)
                throw new InvalidOperationException(
                    $"No existe un aeropuerto con ID '{model.IdAeropuerto}'.");

            // Verificar unicidad Vuelo + Orden (UQ_Escala_Vuelo_Orden)
            var escalaExistente = await _uow.EscalaRepository
                                            .GetByVueloYOrdenAsync(
                                                model.IdVuelo, model.Orden);

            if (escalaExistente != null)
                throw new InvalidOperationException(
                    $"Ya existe una escala con orden '{model.Orden}' " +
                    $"para el vuelo '{model.IdVuelo}'.");

            var entity = EscalaDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.Eliminado = false;
            entity.Estado = "ACTIVO";

            entity.TipoEscala = string.IsNullOrWhiteSpace(model.TipoEscala)
                ? "COMERCIAL"
                : model.TipoEscala.ToUpper();

            await _uow.EscalaRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return EscalaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(EscalaDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdEscala <= 0)
                throw new ArgumentException(
                    "El ID de la escala debe ser mayor a 0.");

            var entity = await _uow.EscalaRepository.GetByIdAsync(model.IdEscala);

            if (entity == null)
                return false;

            if (model.FechaHoraSalida <= model.FechaHoraLlegada)
                throw new ArgumentException(
                    "La fecha/hora de salida debe ser posterior " +
                    "a la fecha/hora de llegada.");

            if (model.DuracionMin < 0)
                throw new ArgumentException(
                    "La duración no puede ser negativa.");

            if (!string.IsNullOrWhiteSpace(model.TipoEscala) &&
                !TiposEscalaValidos.Contains(model.TipoEscala.ToUpper()))
                throw new ArgumentException(
                    $"Tipo de escala inválido. " +
                    $"Los válidos son: {string.Join(", ", TiposEscalaValidos)}");

            // Verificar unicidad Orden si cambió
            if (model.Orden >= 1 && model.Orden != entity.Orden)
            {
                var existeOrden = await _uow.EscalaRepository
                                            .GetByVueloYOrdenAsync(
                                                entity.IdVuelo, model.Orden);

                if (existeOrden != null && existeOrden.IdEscala != model.IdEscala)
                    throw new InvalidOperationException(
                        $"Ya existe una escala con orden '{model.Orden}' " +
                        $"en este vuelo.");
            }

            // UpdateEntity NO toca IdVuelo ni IdAeropuerto
            EscalaDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.EscalaRepository.Update(entity);
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
                    "El ID de la escala debe ser mayor a 0.", nameof(id));

            var entity = await _uow.EscalaRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.EscalaRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}