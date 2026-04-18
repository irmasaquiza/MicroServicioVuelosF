using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class UsuarioRolDataService : IUsuarioRolDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] EstadosValidos = { "ACT", "INA" };

        public UsuarioRolDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────
        // GET ALL
        // ─────────────────────────────
        public async Task<IEnumerable<UsuarioRolDataModel>> GetAllAsync()
        {
            var entities = await _uow.UsuarioRolRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<UsuarioRolDataModel>();

            return UsuarioRolDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────
        // GET BY ID
        // ─────────────────────────────
        public async Task<UsuarioRolDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID debe ser mayor a 0.", nameof(id));

            var entity = await _uow.UsuarioRolRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return UsuarioRolDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────
        // GET BY USUARIO
        // ─────────────────────────────
        public async Task<IEnumerable<UsuarioRolDataModel>> GetByUsuarioAsync(int idUsuario)
        {
            if (idUsuario <= 0)
                throw new ArgumentException("El ID del usuario debe ser mayor a 0.");

            var entities = await _uow.UsuarioRolRepository.GetByUsuarioAsync(idUsuario);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<UsuarioRolDataModel>();

            return UsuarioRolDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────
        // GET BY ROL
        // ─────────────────────────────
        public async Task<IEnumerable<UsuarioRolDataModel>> GetByRolAsync(int idRol)
        {
            if (idRol <= 0)
                throw new ArgumentException("El ID del rol debe ser mayor a 0.");

            var entities = await _uow.UsuarioRolRepository.GetByRolAsync(idRol);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<UsuarioRolDataModel>();

            return UsuarioRolDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────
        // GET ACTIVOS
        // ─────────────────────────────
        public async Task<IEnumerable<UsuarioRolDataModel>> GetActivosAsync()
        {
            var entities = await _uow.UsuarioRolRepository.GetAllAsync();

            var activos = entities
                .Where(ur => ur.Activo && ur.EstadoUsuarioRol == "ACT")
                .ToList();

            if (!activos.Any())
                return Enumerable.Empty<UsuarioRolDataModel>();

            return UsuarioRolDataMapper.ToDataModelList(activos);
        }

        // ─────────────────────────────
        // GET PAGED
        // ─────────────────────────────
        public async Task<DataPagedResult<UsuarioRolDataModel>> GetPagedAsync(
            UsuarioRolFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.UsuarioRolRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdUsuario.HasValue)
                query = query.Where(ur => ur.IdUsuario == filtro.IdUsuario.Value);

            if (filtro.IdRol.HasValue)
                query = query.Where(ur => ur.IdRol == filtro.IdRol.Value);

            if (!string.IsNullOrWhiteSpace(filtro.EstadoUsuarioRol))
                query = query.Where(ur =>
                    ur.EstadoUsuarioRol.ToUpper() ==
                    filtro.EstadoUsuarioRol.ToUpper());

            if (filtro.Activo.HasValue)
                query = query.Where(ur => ur.Activo == filtro.Activo.Value);

            if (filtro.FechaRegistroInicio.HasValue)
                query = query.Where(ur =>
                    ur.FechaRegistroUtc >= filtro.FechaRegistroInicio.Value);

            if (filtro.FechaRegistroFin.HasValue)
                query = query.Where(ur =>
                    ur.FechaRegistroUtc <= filtro.FechaRegistroFin.Value);

            query = query.OrderBy(ur => ur.IdUsuario).ThenBy(ur => ur.IdRol);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(UsuarioRolDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<UsuarioRolDataModel>
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
        public async Task<UsuarioRolDataModel> CreateAsync(UsuarioRolDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdUsuario <= 0)
                throw new ArgumentException("El ID del usuario es obligatorio.");

            if (model.IdRol <= 0)
                throw new ArgumentException("El ID del rol es obligatorio.");

            // Validar estado
            if (!string.IsNullOrWhiteSpace(model.EstadoUsuarioRol) &&
                !EstadosValidos.Contains(model.EstadoUsuarioRol.ToUpper()))
                throw new ArgumentException("Estado inválido.");

            // Validar FK
            var usuario = await _uow.UsuarioAppRepository.GetByIdAsync(model.IdUsuario);
            if (usuario == null)
                throw new InvalidOperationException("Usuario no existe.");

            var rol = await _uow.RolRepository.GetByIdAsync(model.IdRol);
            if (rol == null)
                throw new InvalidOperationException("Rol no existe.");

            // Validar duplicado
            var existente = await _uow.UsuarioRolRepository
                .GetByUsuarioAndRolAsync(model.IdUsuario, model.IdRol);

            if (existente != null)
                throw new InvalidOperationException("El usuario ya tiene ese rol.");

            var entity = UsuarioRolDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.Activo = true;
            entity.EsEliminado = false;

            entity.EstadoUsuarioRol = string.IsNullOrWhiteSpace(model.EstadoUsuarioRol)
                ? "ACT"
                : model.EstadoUsuarioRol.ToUpper();

            await _uow.UsuarioRolRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return UsuarioRolDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────
        // UPDATE
        // ─────────────────────────────
        public async Task<bool> UpdateAsync(UsuarioRolDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdUsuarioRol <= 0)
                throw new ArgumentException("ID inválido.");

            var entity = await _uow.UsuarioRolRepository.GetByIdAsync(model.IdUsuarioRol);

            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.EstadoUsuarioRol) &&
                !EstadosValidos.Contains(model.EstadoUsuarioRol.ToUpper()))
                throw new ArgumentException("Estado inválido.");

            UsuarioRolDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.UsuarioRolRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────
        // DELETE (SOFT DELETE)
        // ─────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido.");

            var entity = await _uow.UsuarioRolRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.UsuarioRolRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";
            entity.Activo = false;

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}