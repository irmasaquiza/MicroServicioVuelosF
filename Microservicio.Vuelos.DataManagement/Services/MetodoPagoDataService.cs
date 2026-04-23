/*// ============================================================
// MetodoPagoDataService.cs
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
    public class MetodoPagoDataService : IMetodoPagoDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] EstadosValidos =
            { "ACTIVO", "EXPIRADO", "BLOQUEADO" };

        public MetodoPagoDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<MetodoPagoDataModel>> GetAllAsync()
        {
            var entities = await _uow.MetodoPagoRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<MetodoPagoDataModel>();

            return MetodoPagoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<MetodoPagoDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del método de pago debe ser mayor a 0.", nameof(id));

            var entity = await _uow.MetodoPagoRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return MetodoPagoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CLIENTE
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<MetodoPagoDataModel>> GetByClienteAsync(
            int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.", nameof(idCliente));

            var entities = await _uow.MetodoPagoRepository
                                     .GetByClienteAsync(idCliente);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<MetodoPagoDataModel>();

            return MetodoPagoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY TIPO METODO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<MetodoPagoDataModel>> GetByTipoMetodoAsync(
            int idTipoMetodo)
        {
            if (idTipoMetodo <= 0)
                throw new ArgumentException(
                    "El ID del tipo de método debe ser mayor a 0.",
                    nameof(idTipoMetodo));

            var entities = await _uow.MetodoPagoRepository
                                     .GetByTipoAsync(idTipoMetodo);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<MetodoPagoDataModel>();

            return MetodoPagoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET PRINCIPALES
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<MetodoPagoDataModel>> GetPrincipalesAsync(
            int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.", nameof(idCliente));

            var principal = await _uow.MetodoPagoRepository
                                      .GetPrincipalByClienteAsync(idCliente);

            if (principal == null)
                return Enumerable.Empty<MetodoPagoDataModel>();

            return new List<MetodoPagoDataModel>
            {
                MetodoPagoDataMapper.ToDataModel(principal)
            };
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<MetodoPagoDataModel>> GetPagedAsync(
            MetodoPagoFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.MetodoPagoRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(m => m.IdCliente == filtro.IdCliente.Value);

            if (filtro.IdTipoMetodo.HasValue)
                query = query.Where(m =>
                    m.IdTipoMetodo == filtro.IdTipoMetodo.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Ultimos4))
                query = query.Where(m =>
                    m.Ultimos4 != null &&
                    m.Ultimos4 == filtro.Ultimos4.Trim());

            if (!string.IsNullOrWhiteSpace(filtro.MarcaTarjeta))
                query = query.Where(m =>
                    m.MarcaTarjeta != null &&
                    m.MarcaTarjeta.ToUpper() == filtro.MarcaTarjeta.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.BancoEmisor))
                query = query.Where(m =>
                    m.BancoEmisor != null &&
                    m.BancoEmisor.ToUpper().Contains(filtro.BancoEmisor.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.PaisEmision))
                query = query.Where(m =>
                    m.PaisEmision != null &&
                    m.PaisEmision.ToUpper() == filtro.PaisEmision.ToUpper());

            if (filtro.EsPrincipal.HasValue)
                query = query.Where(m => m.EsPrincipal == filtro.EsPrincipal.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(m =>
                    m.Estado.ToUpper() == filtro.Estado.ToUpper());

            if (filtro.FechaExpiracionInicio.HasValue)
                query = query.Where(m =>
                    m.FechaExpiracion.HasValue &&
                    m.FechaExpiracion.Value >= filtro.FechaExpiracionInicio.Value);

            if (filtro.FechaExpiracionFin.HasValue)
                query = query.Where(m =>
                    m.FechaExpiracion.HasValue &&
                    m.FechaExpiracion.Value <= filtro.FechaExpiracionFin.Value);

            if (filtro.FechaUltimoUsoInicio.HasValue)
                query = query.Where(m =>
                    m.FechaUltimoUso.HasValue &&
                    m.FechaUltimoUso.Value >= filtro.FechaUltimoUsoInicio.Value);

            if (filtro.FechaUltimoUsoFin.HasValue)
                query = query.Where(m =>
                    m.FechaUltimoUso.HasValue &&
                    m.FechaUltimoUso.Value <= filtro.FechaUltimoUsoFin.Value);

            query = query.OrderByDescending(m => m.EsPrincipal)
                         .ThenBy(m => m.IdCliente);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(MetodoPagoDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<MetodoPagoDataModel>
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
        public async Task<MetodoPagoDataModel> CreateAsync(MetodoPagoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdCliente <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio.");

            if (model.IdTipoMetodo <= 0)
                throw new ArgumentException(
                    "El ID del tipo de método es obligatorio.");

            // Verificar que cliente exista
            var cliente = await _uow.ClienteRepository
                                    .GetByIdAsync(model.IdCliente);

            if (cliente == null)
                throw new InvalidOperationException(
                    $"No existe un cliente con ID '{model.IdCliente}'.");

            // Verificar que tipo método exista
            var tipo = await _uow.TipoMetodoPagoRepository
                                 .GetByIdAsync(model.IdTipoMetodo);

            if (tipo == null)
                throw new InvalidOperationException(
                    $"No existe un tipo de método de pago con ID " +
                    $"'{model.IdTipoMetodo}'.");

            var entity = MetodoPagoDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;

            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                ? "ACTIVO"
                : model.Estado.ToUpper();

            await _uow.MetodoPagoRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return MetodoPagoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(MetodoPagoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdMetodo <= 0)
                throw new ArgumentException(
                    "El ID del método de pago debe ser mayor a 0.");

            var entity = await _uow.MetodoPagoRepository.GetByIdAsync(model.IdMetodo);

            if (entity == null)
                return false;

            if (entity.Estado == "BLOQUEADO")
                throw new InvalidOperationException(
                    "No se puede modificar un método de pago bloqueado.");

            if (!string.IsNullOrWhiteSpace(model.Estado) &&
                !EstadosValidos.Contains(model.Estado.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosValidos)}");

            // UpdateEntity NO toca: IdCliente, IdTipoMetodo, TokenPasarela
            MetodoPagoDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.MetodoPagoRepository.Update(entity);
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
                    "El ID del método de pago debe ser mayor a 0.", nameof(id));

            var entity = await _uow.MetodoPagoRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.MetodoPagoRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}*/