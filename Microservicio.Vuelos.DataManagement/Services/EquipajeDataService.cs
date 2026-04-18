// ============================================================
// EquipajeDataService.cs
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
    public class EquipajeDataService : IEquipajeDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] TiposValidos =
            { "MANO", "BODEGA" };

        private static readonly string[] EstadosEquipajeValidos =
        {
            "REGISTRADO", "EMBARCADO", "EN_TRANSITO",
            "ENTREGADO",  "CANCELADO", "PERDIDO", "DAÑADO"
        };

        public EquipajeDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<EquipajeDataModel>> GetAllAsync()
        {
            var entities = await _uow.EquipajeRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<EquipajeDataModel>();

            return EquipajeDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<EquipajeDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del equipaje debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.EquipajeRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return EquipajeDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY BOLETO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<EquipajeDataModel>> GetByBoletoAsync(int idBoleto)
        {
            if (idBoleto <= 0)
                throw new ArgumentException(
                    "El ID del boleto debe ser mayor a 0.",
                    nameof(idBoleto));

            var entities = await _uow.EquipajeRepository.GetByBoletoAsync(idBoleto);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<EquipajeDataModel>();

            return EquipajeDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY TIPO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<EquipajeDataModel>> GetByTipoAsync(string tipo)
        {
            if (string.IsNullOrWhiteSpace(tipo))
                throw new ArgumentException(
                    "El tipo de equipaje no puede estar vacío.",
                    nameof(tipo));

            if (!TiposValidos.Contains(tipo.ToUpper()))
                throw new ArgumentException(
                    $"Tipo inválido. Los válidos son: {string.Join(", ", TiposValidos)}");

            var todos = await _uow.EquipajeRepository.GetAllAsync();

            var filtrados = todos
                .Where(e => e.Tipo.ToUpper() == tipo.ToUpper())
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<EquipajeDataModel>();

            return EquipajeDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<EquipajeDataModel>> GetPagedAsync(
            EquipajeFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.EquipajeRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdBoleto.HasValue)
                query = query.Where(e => e.IdBoleto == filtro.IdBoleto.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Tipo))
                query = query.Where(e =>
                    e.Tipo.ToUpper() == filtro.Tipo.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.EstadoEquipaje))
                query = query.Where(e =>
                    e.EstadoEquipaje.ToUpper() == filtro.EstadoEquipaje.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.NumeroEtiqueta))
                query = query.Where(e =>
                    e.NumeroEtiqueta != null &&
                    e.NumeroEtiqueta.Contains(filtro.NumeroEtiqueta.Trim()));

            if (filtro.PesoMin.HasValue)
                query = query.Where(e => e.PesoKg >= filtro.PesoMin.Value);

            if (filtro.PesoMax.HasValue)
                query = query.Where(e => e.PesoKg <= filtro.PesoMax.Value);

            if (filtro.PrecioMin.HasValue)
                query = query.Where(e => e.PrecioExtra >= filtro.PrecioMin.Value);

            if (filtro.PrecioMax.HasValue)
                query = query.Where(e => e.PrecioExtra <= filtro.PrecioMax.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(e =>
                    e.Estado.ToUpper() == filtro.Estado.ToUpper());

            query = query.OrderBy(e => e.Tipo).ThenBy(e => e.PesoKg);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(EquipajeDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<EquipajeDataModel>
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
        public async Task<EquipajeDataModel> CreateAsync(EquipajeDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdBoleto <= 0)
                throw new ArgumentException("El ID del boleto es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Tipo))
                throw new ArgumentException("El tipo de equipaje es obligatorio.");

            if (!TiposValidos.Contains(model.Tipo.ToUpper()))
                throw new ArgumentException(
                    $"Tipo inválido. Los válidos son: {string.Join(", ", TiposValidos)}");

            if (model.PesoKg <= 0)
                throw new ArgumentException(
                    "El peso del equipaje debe ser mayor a 0.");

            // CHK_Equipaje_ManoMax → equipaje de mano máximo 10kg
            if (model.Tipo.ToUpper() == "MANO" && model.PesoKg > 10)
                throw new ArgumentException(
                    "El equipaje de mano no puede superar los 10 kg.");

            if (model.PrecioExtra < 0)
                throw new ArgumentException(
                    "El precio extra no puede ser negativo.");

            // Verificar que el boleto exista
            var boleto = await _uow.BoletoRepository.GetByIdAsync(model.IdBoleto);

            if (boleto == null)
                throw new InvalidOperationException(
                    $"No existe un boleto con ID '{model.IdBoleto}'.");

            var entity = EquipajeDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.Estado = "ACTIVO";
            entity.Tipo = model.Tipo.ToUpper();

            entity.EstadoEquipaje = string.IsNullOrWhiteSpace(model.EstadoEquipaje)
                ? "REGISTRADO"
                : model.EstadoEquipaje.ToUpper();

            await _uow.EquipajeRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return EquipajeDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(EquipajeDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdEquipaje <= 0)
                throw new ArgumentException(
                    "El ID del equipaje debe ser mayor a 0.");

            var entity = await _uow.EquipajeRepository.GetByIdAsync(model.IdEquipaje);

            if (entity == null)
                return false;

            // No modificar equipaje entregado o perdido
            if (entity.EstadoEquipaje == "ENTREGADO")
                throw new InvalidOperationException(
                    "No se puede modificar un equipaje ya entregado.");

            if (entity.EstadoEquipaje == "PERDIDO")
                throw new InvalidOperationException(
                    "No se puede modificar un equipaje reportado como perdido.");

            if (!string.IsNullOrWhiteSpace(model.Tipo) &&
                !TiposValidos.Contains(model.Tipo.ToUpper()))
                throw new ArgumentException(
                    $"Tipo inválido. Los válidos son: {string.Join(", ", TiposValidos)}");

            if (!string.IsNullOrWhiteSpace(model.EstadoEquipaje) &&
                !EstadosEquipajeValidos.Contains(model.EstadoEquipaje.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosEquipajeValidos)}");

            // Validar mano máx 10kg si el tipo no cambió o sigue siendo MANO
            var tipoFinal = string.IsNullOrWhiteSpace(model.Tipo)
                ? entity.Tipo
                : model.Tipo.ToUpper();

            if (tipoFinal == "MANO" && model.PesoKg > 10)
                throw new ArgumentException(
                    "El equipaje de mano no puede superar los 10 kg.");

            EquipajeDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.EquipajeRepository.Update(entity);
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
                    "El ID del equipaje debe ser mayor a 0.", nameof(id));

            var entity = await _uow.EquipajeRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            if (entity.EstadoEquipaje == "EMBARCADO" ||
                entity.EstadoEquipaje == "EN_TRANSITO")
                throw new InvalidOperationException(
                    "No se puede eliminar un equipaje que está en tránsito.");

            _uow.EquipajeRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}