/*using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class TipoMetodoPagoDataService : ITipoMetodoPagoDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] EstadosValidos =
            { "ACTIVO", "INACTIVO" };

        public TipoMetodoPagoDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────
        // GET ALL
        // ─────────────────────────────
        public async Task<IEnumerable<TipoMetodoPagoDataModel>> GetAllAsync()
        {
            var entities = await _uow.TipoMetodoPagoRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<TipoMetodoPagoDataModel>();

            return TipoMetodoPagoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────
        // GET BY ID
        // ─────────────────────────────
        public async Task<TipoMetodoPagoDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.", nameof(id));

            var entity = await _uow.TipoMetodoPagoRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return TipoMetodoPagoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────
        // GET BY NOMBRE
        // ─────────────────────────────
        public async Task<TipoMetodoPagoDataModel> GetByNombreAsync(string nombreTipo)
        {
            if (string.IsNullOrWhiteSpace(nombreTipo))
                throw new ArgumentException("Nombre requerido.", nameof(nombreTipo));

            var entity = await _uow.TipoMetodoPagoRepository
                                  .GetByNombreExactoAsync(nombreTipo.Trim());

            if (entity == null)
                return null;

            return TipoMetodoPagoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────
        // GET ACTIVOS
        // ─────────────────────────────
        public async Task<IEnumerable<TipoMetodoPagoDataModel>> GetActivosAsync()
        {
            var todos = await _uow.TipoMetodoPagoRepository.GetAllAsync();

            var activos = todos
                .Where(t => t.Estado == "ACTIVO" && !t.EsEliminado)
                .ToList();

            if (!activos.Any())
                return Enumerable.Empty<TipoMetodoPagoDataModel>();

            return TipoMetodoPagoDataMapper.ToDataModelList(activos);
        }

        // ─────────────────────────────
        // GET PAGED
        // ─────────────────────────────
        public async Task<DataPagedResult<TipoMetodoPagoDataModel>> GetPagedAsync(
            TipoMetodoPagoFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.TipoMetodoPagoRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.NombreTipo))
                query = query.Where(t =>
                    t.NombreTipo.ToUpper().Contains(filtro.NombreTipo.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(t =>
                    t.Estado.ToUpper() == filtro.Estado.ToUpper());

            query = query.OrderBy(t => t.NombreTipo);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(TipoMetodoPagoDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<TipoMetodoPagoDataModel>
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

        // ─────────────────────────────
        // CREATE
        // ─────────────────────────────
        public async Task<TipoMetodoPagoDataModel> CreateAsync(
            TipoMetodoPagoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.NombreTipo))
                throw new ArgumentException("Nombre obligatorio.");

            // Validar estado
            if (!string.IsNullOrWhiteSpace(model.Estado) &&
                !EstadosValidos.Contains(model.Estado.ToUpper()))
                throw new ArgumentException("Estado inválido.");

            // Validar unicidad
            var existente = await _uow.TipoMetodoPagoRepository
                .GetByNombreExactoAsync(model.NombreTipo.Trim());

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe un tipo con nombre '{model.NombreTipo}'.");

            var entity = TipoMetodoPagoDataMapper.ToEntity(model);

            // Auditoría
            entity.NombreTipo = model.NombreTipo.Trim().ToUpper();
            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                ? "ACTIVO"
                : model.Estado.ToUpper();

            entity.EsEliminado = false;

            await _uow.TipoMetodoPagoRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return TipoMetodoPagoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────
        // UPDATE
        // ─────────────────────────────
        public async Task<bool> UpdateAsync(TipoMetodoPagoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdTipoMetodo <= 0)
                throw new ArgumentException("ID inválido.");

            var entity = await _uow.TipoMetodoPagoRepository
                .GetByIdAsync(model.IdTipoMetodo);

            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.Estado) &&
                !EstadosValidos.Contains(model.Estado.ToUpper()))
                throw new ArgumentException("Estado inválido.");

            // Validar unicidad si cambia
            if (!string.IsNullOrWhiteSpace(model.NombreTipo) &&
                model.NombreTipo.Trim().ToUpper() != entity.NombreTipo.ToUpper())
            {
                var existente = await _uow.TipoMetodoPagoRepository
                    .GetByNombreExactoAsync(model.NombreTipo.Trim());

                if (existente != null &&
                    existente.IdTipoMetodo != model.IdTipoMetodo)
                    throw new InvalidOperationException(
                        "Ya existe otro tipo con ese nombre.");
            }

            // Update con mapper
            TipoMetodoPagoDataMapper.UpdateEntity(entity, model);

            _uow.TipoMetodoPagoRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────
        // DELETE
        // ─────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            var entity = await _uow.TipoMetodoPagoRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            // Validar si está en uso
            if (entity.MetodosPago != null && entity.MetodosPago.Any())
                throw new InvalidOperationException(
                    "No se puede eliminar un tipo en uso.");

            _uow.TipoMetodoPagoRepository.Delete(entity);

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}*/