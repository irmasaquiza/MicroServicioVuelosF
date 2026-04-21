// ============================================================
// UsuarioAppDataService.cs
// ============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;
using Microservicio.Vuelos.DataAccess.Entities; // 🔥 IMPORTANTE

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class UsuarioAppDataService : IUsuarioAppDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] EstadosValidos = { "ACT", "INA" };

        public UsuarioAppDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<UsuarioAppDataModel>> GetAllAsync()
        {
            var entities = await _uow.UsuarioAppRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<UsuarioAppDataModel>();

            return UsuarioAppDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<UsuarioAppDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del usuario debe ser mayor a 0.", nameof(id));

            var entity = await _uow.UsuarioAppRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return UsuarioAppDataMapper.ToDataModel(entity);
        }

        public async Task<UsuarioAppEntity?> GetByCredentialsAsync(string login)
        {
            if (string.IsNullOrWhiteSpace(login))
                throw new ArgumentException("Login requerido");

            var input = login.Trim().ToLower();

            var usuarios = await _uow.UsuarioAppRepository.GetAllAsync();

            return usuarios.FirstOrDefault(u =>
                (u.Username.ToLower() == input || u.Correo.ToLower() == input) &&
                !u.EsEliminado);
        }

        // ─────────────────────────────────────────────
        // GET BY USERNAME
        // ─────────────────────────────────────────────
        public async Task<UsuarioAppDataModel> GetByUsernameAsync(string username)
        {
            if (string.IsNullOrWhiteSpace(username))
                throw new ArgumentException(
                    "El username no puede estar vacío.", nameof(username));

            var entity = await _uow.UsuarioAppRepository
                                   .GetByUsernameAsync(username.Trim());

            if (entity == null)
                return null;

            return UsuarioAppDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CORREO
        // ─────────────────────────────────────────────
        public async Task<UsuarioAppDataModel> GetByCorreoAsync(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException(
                    "El correo no puede estar vacío.", nameof(correo));

            var entity = await _uow.UsuarioAppRepository
                                   .GetByCorreoAsync(correo.Trim().ToLower());

            if (entity == null)
                return null;

            return UsuarioAppDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CLIENTE
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<UsuarioAppDataModel>> GetByClienteAsync(
            int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.", nameof(idCliente));

            var todos = await _uow.UsuarioAppRepository.GetAllAsync();

            var filtrados = todos
                .Where(u => u.IdCliente == idCliente)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<UsuarioAppDataModel>();

            return UsuarioAppDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET ACTIVOS
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<UsuarioAppDataModel>> GetActivosAsync()
        {
            var todos = await _uow.UsuarioAppRepository.GetAllAsync();

            var activos = todos
                .Where(u => u.Activo && u.EstadoUsuario == "ACT")
                .ToList();

            if (!activos.Any())
                return Enumerable.Empty<UsuarioAppDataModel>();

            return UsuarioAppDataMapper.ToDataModelList(activos);
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<UsuarioAppDataModel>> GetPagedAsync(
            UsuarioAppFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.UsuarioAppRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(u => u.IdCliente == filtro.IdCliente.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Username))
                query = query.Where(u =>
                    u.Username.ToUpper().Contains(filtro.Username.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Correo))
                query = query.Where(u =>
                    u.Correo.ToUpper().Contains(filtro.Correo.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.EstadoUsuario))
                query = query.Where(u =>
                    u.EstadoUsuario.ToUpper() == filtro.EstadoUsuario.ToUpper());

            if (filtro.Activo.HasValue)
                query = query.Where(u => u.Activo == filtro.Activo.Value);

            if (filtro.FechaUltimoLoginInicio.HasValue)
                query = query.Where(u =>
                    u.FechaUltimoLogin.HasValue &&
                    u.FechaUltimoLogin.Value >=
                    filtro.FechaUltimoLoginInicio.Value);

            if (filtro.FechaUltimoLoginFin.HasValue)
                query = query.Where(u =>
                    u.FechaUltimoLogin.HasValue &&
                    u.FechaUltimoLogin.Value <=
                    filtro.FechaUltimoLoginFin.Value);

            query = query.OrderBy(u => u.Username);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(UsuarioAppDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<UsuarioAppDataModel>
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
        public async Task<UsuarioAppDataModel> CreateAsync(UsuarioAppDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.Username))
                throw new ArgumentException("El username es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Correo))
                throw new ArgumentException("El correo es obligatorio.");

            // UQ_USUARIO_APP_USERNAME
            var existenteUser = await _uow.UsuarioAppRepository
                                          .GetByUsernameAsync(model.Username.Trim());

            if (existenteUser != null)
                throw new InvalidOperationException(
                    $"Ya existe un usuario con el username '{model.Username}'.");

            // UQ_USUARIO_APP_CORREO
            var existenteCorreo = await _uow.UsuarioAppRepository
                                            .GetByCorreoAsync(
                                                model.Correo.Trim().ToLower());

            if (existenteCorreo != null)
                throw new InvalidOperationException(
                    $"Ya existe un usuario con el correo '{model.Correo}'.");

            // Verificar que cliente exista si viene
            if (model.IdCliente.HasValue && model.IdCliente.Value > 0)
            {
                var cliente = await _uow.ClienteRepository
                                        .GetByIdAsync(model.IdCliente.Value);

                if (cliente == null)
                    throw new InvalidOperationException(
                        $"No existe un cliente con ID '{model.IdCliente}'.");
            }

            var entity = UsuarioAppDataMapper.ToEntity(model);

            entity.UsuarioGuid = Guid.NewGuid();
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.Activo = true;
            entity.Correo = model.Correo.Trim().ToLower();
            entity.Username = model.Username.Trim();

            entity.EstadoUsuario = string.IsNullOrWhiteSpace(model.EstadoUsuario)
                ? "ACT"
                : model.EstadoUsuario.ToUpper();

            await _uow.UsuarioAppRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return UsuarioAppDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(UsuarioAppDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdUsuario <= 0)
                throw new ArgumentException(
                    "El ID del usuario debe ser mayor a 0.");

            var entity = await _uow.UsuarioAppRepository.GetByIdAsync(model.IdUsuario);

            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.EstadoUsuario) &&
                !EstadosValidos.Contains(model.EstadoUsuario.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosValidos)}");

            // Verificar unicidad username si cambió
            if (!string.IsNullOrWhiteSpace(model.Username) &&
                model.Username.Trim() != entity.Username)
            {
                var existente = await _uow.UsuarioAppRepository
                                          .GetByUsernameAsync(model.Username.Trim());

                if (existente != null && existente.IdUsuario != model.IdUsuario)
                    throw new InvalidOperationException(
                        $"Ya existe otro usuario con el username '{model.Username}'.");
            }

            // Verificar unicidad correo si cambió
            if (!string.IsNullOrWhiteSpace(model.Correo) &&
                model.Correo.Trim().ToLower() != entity.Correo.ToLower())
            {
                var existente = await _uow.UsuarioAppRepository
                                          .GetByCorreoAsync(
                                              model.Correo.Trim().ToLower());

                if (existente != null && existente.IdUsuario != model.IdUsuario)
                    throw new InvalidOperationException(
                        $"Ya existe otro usuario con el correo '{model.Correo}'.");
            }

            // UpdateEntity NO toca: IdUsuario, UsuarioGuid, PasswordHash, PasswordSalt
            UsuarioAppDataMapper.UpdateEntity(entity, model);

            if (!string.IsNullOrWhiteSpace(model.Correo))
                entity.Correo = model.Correo.Trim().ToLower();

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.UsuarioAppRepository.Update(entity);
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
                    "El ID del usuario debe ser mayor a 0.", nameof(id));

            var entity = await _uow.UsuarioAppRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.UsuarioAppRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";
            entity.Activo = false;

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}