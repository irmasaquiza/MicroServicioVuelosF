// ============================================================
// RolDataService.cs
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
    public class RolDataService : IRolDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] EstadosValidos = { "ACT", "INA" };

        public RolDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<RolDataModel>> GetAllAsync()
        {
            var entities = await _uow.RolRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<RolDataModel>();

            return RolDataMapper.ToDataModelList(entities);
        }

        public async Task<RolDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del rol debe ser mayor a 0.", nameof(id));

            var entity = await _uow.RolRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return RolDataMapper.ToDataModel(entity);
        }

        public async Task<RolDataModel> GetByNombreAsync(string nombreRol)
        {
            if (string.IsNullOrWhiteSpace(nombreRol))
                throw new ArgumentException(
                    "El nombre del rol no puede estar vacío.", nameof(nombreRol));

            var entity = await _uow.RolRepository.GetByNombreAsync(nombreRol.Trim());

            if (entity == null)
                return null;

            return RolDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<RolDataModel>> GetActivosAsync()
        {
            var todos = await _uow.RolRepository.GetAllAsync();

            var activos = todos
                .Where(r => r.Activo && r.EstadoRol == "ACT")
                .ToList();

            if (!activos.Any())
                return Enumerable.Empty<RolDataModel>();

            return RolDataMapper.ToDataModelList(activos);
        }

        public async Task<DataPagedResult<RolDataModel>> GetPagedAsync(
            RolFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.RolRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.NombreRol))
                query = query.Where(r =>
                    r.NombreRol.ToUpper().Contains(filtro.NombreRol.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.EstadoRol))
                query = query.Where(r =>
                    r.EstadoRol.ToUpper() == filtro.EstadoRol.ToUpper());

            if (filtro.Activo.HasValue)
                query = query.Where(r => r.Activo == filtro.Activo.Value);

            query = query.OrderBy(r => r.NombreRol);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(RolDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<RolDataModel>
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

        public async Task<RolDataModel> CreateAsync(RolDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.NombreRol))
                throw new ArgumentException("El nombre del rol es obligatorio.");

            // UQ_ROL_NOMBRE
            var existente = await _uow.RolRepository
                                      .GetByNombreAsync(model.NombreRol.Trim());

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe un rol con el nombre '{model.NombreRol}'.");

            var entity = RolDataMapper.ToEntity(model);

            entity.RolGuid = Guid.NewGuid();
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.Activo = true;

            entity.EstadoRol = string.IsNullOrWhiteSpace(model.EstadoRol)
                ? "ACT"
                : model.EstadoRol.ToUpper();

            await _uow.RolRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return RolDataMapper.ToDataModel(entity);
        }

        public async Task<bool> UpdateAsync(RolDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdRol <= 0)
                throw new ArgumentException(
                    "El ID del rol debe ser mayor a 0.");

            var entity = await _uow.RolRepository.GetByIdAsync(model.IdRol);

            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.EstadoRol) &&
                !EstadosValidos.Contains(model.EstadoRol.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosValidos)}");

            // Verificar unicidad nombre si cambió
            if (!string.IsNullOrWhiteSpace(model.NombreRol) &&
                model.NombreRol.Trim().ToUpper() != entity.NombreRol.ToUpper())
            {
                var conMismoNombre = await _uow.RolRepository
                                               .GetByNombreAsync(model.NombreRol.Trim());

                if (conMismoNombre != null && conMismoNombre.IdRol != model.IdRol)
                    throw new InvalidOperationException(
                        $"Ya existe otro rol con el nombre '{model.NombreRol}'.");
            }

            // UpdateEntity NO toca: IdRol, RolGuid
            RolDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.RolRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del rol debe ser mayor a 0.", nameof(id));

            var entity = await _uow.RolRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.RolRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}