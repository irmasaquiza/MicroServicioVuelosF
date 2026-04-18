using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class AsientoDataService : IAsientoDataService
    {
        private readonly IUnitOfWork _uow;

        public AsientoDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AsientoDataModel>> GetAllAsync()
        {
            var entities = await _uow.AsientoRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AsientoDataModel>();

            return entities.Select(AsientoDataMapper.ToDataModel);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<AsientoDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del asiento debe ser mayor a 0.", nameof(id));

            var entity = await _uow.AsientoRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return AsientoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY VUELO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AsientoDataModel>> GetByVueloAsync(int idVuelo)
        {
            if (idVuelo <= 0)
                throw new ArgumentException("El ID del vuelo debe ser mayor a 0.", nameof(idVuelo));

            var entities = await _uow.AsientoRepository.GetAllAsync();

            var result = entities
                .Where(a => a.IdVuelo == idVuelo && !a.Eliminado)
                .Select(AsientoDataMapper.ToDataModel);

            return result;
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<AsientoDataModel>> GetPagedAsync(AsientoFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.AsientoRepository.GetAllAsync();

            var query = todos.AsQueryable();

            if (filtro.IdVuelo.HasValue)
                query = query.Where(a => a.IdVuelo == filtro.IdVuelo.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Clase))
                query = query.Where(a =>
                    a.Clase.ToUpper() == filtro.Clase.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(a =>
                    a.Estado.ToUpper() == filtro.Estado.ToUpper());

            if (filtro.Disponible.HasValue)
                query = query.Where(a => a.Disponible == filtro.Disponible.Value);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(AsientoDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<AsientoDataModel>
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
        public async Task<AsientoDataModel> CreateAsync(AsientoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdVuelo <= 0)
                throw new ArgumentException("El asiento debe pertenecer a un vuelo.");

            if (string.IsNullOrWhiteSpace(model.NumeroAsiento))
                throw new ArgumentException("El número de asiento es obligatorio.");

            var entity = AsientoDataMapper.ToEntity(model);

            // Auditoría
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.Eliminado = false;

            await _uow.AsientoRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return AsientoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(AsientoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            var entity = await _uow.AsientoRepository.GetByIdAsync(model.IdAsiento);

            if (entity == null)
                return false;

            AsientoDataMapper.UpdateEntity(entity, model);

            // Auditoría
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.AsientoRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // DELETE (LOGICO)
        // ─────────────────────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            var entity = await _uow.AsientoRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.AsientoRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}