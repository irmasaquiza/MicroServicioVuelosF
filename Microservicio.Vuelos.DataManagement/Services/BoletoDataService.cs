using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class BoletoDataService : IBoletoDataService
    {
        private readonly IUnitOfWork _uow;

        // Estados válidos según CHECK de la BD
        private static readonly string[] EstadosBoletoValidos =
            { "ACTIVO", "USADO", "CANCELADO" };

        // Clases válidas según CHECK de la BD
        private static readonly string[] ClasesValidas =
            { "ECONOMICA", "EJECUTIVA", "PRIMERA" };

        public BoletoDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<BoletoDataModel>> GetAllAsync()
        {
            var entities = await _uow.BoletoRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<BoletoDataModel>();

            return BoletoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<BoletoDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del boleto debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.BoletoRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return BoletoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY RESERVA
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<BoletoDataModel>> GetByReservaAsync(int idReserva)
        {
            if (idReserva <= 0)
                throw new ArgumentException(
                    "El ID de la reserva debe ser mayor a 0.",
                    nameof(idReserva));

            var entities = await _uow.BoletoRepository.GetByReservaAsync(idReserva);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<BoletoDataModel>();

            return BoletoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY VUELO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<BoletoDataModel>> GetByVueloAsync(int idVuelo)
        {
            if (idVuelo <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.",
                    nameof(idVuelo));

            var entities = await _uow.BoletoRepository.GetByVueloAsync(idVuelo);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<BoletoDataModel>();

            return BoletoDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY FACTURA
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<BoletoDataModel>> GetByFacturaAsync(int idFactura)
        {
            if (idFactura <= 0)
                throw new ArgumentException(
                    "El ID de la factura debe ser mayor a 0.",
                    nameof(idFactura));

            // El repositorio no tiene GetByFacturaAsync directamente
            // usamos GetAll y filtramos
            var todos = await _uow.BoletoRepository.GetAllAsync();

            var filtrados = todos
                .Where(b => b.IdFactura == idFactura)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<BoletoDataModel>();

            return BoletoDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET PAGED — con filtros
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<BoletoDataModel>> GetPagedAsync(
            BoletoFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(
                    nameof(filtro),
                    "El filtro no puede ser nulo.");

            // Asegurar paginación válida
            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            // Traer todos los no eliminados desde el repositorio
            var todos = await _uow.BoletoRepository.GetAllAsync();

            // ── Aplicar filtros en memoria ──────────────────
            var query = todos.AsQueryable();

            if (filtro.IdReserva.HasValue)
                query = query.Where(b => b.IdReserva == filtro.IdReserva.Value);

            if (filtro.IdVuelo.HasValue)
                query = query.Where(b => b.IdVuelo == filtro.IdVuelo.Value);

            if (filtro.IdAsiento.HasValue)
                query = query.Where(b => b.IdAsiento == filtro.IdAsiento.Value);

            if (filtro.IdFactura.HasValue)
                query = query.Where(b => b.IdFactura == filtro.IdFactura.Value);

            if (!string.IsNullOrWhiteSpace(filtro.CodigoBoleto))
                query = query.Where(b =>
                    b.CodigoBoleto.ToUpper()
                     .Contains(filtro.CodigoBoleto.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Clase))
                query = query.Where(b =>
                    b.Clase.ToUpper() == filtro.Clase.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.EstadoBoleto))
                query = query.Where(b =>
                    b.EstadoBoleto.ToUpper() == filtro.EstadoBoleto.ToUpper());

            // Filtro por rango de fechas de emisión
            if (filtro.FechaInicio.HasValue)
                query = query.Where(b =>
                    b.FechaEmision >= filtro.FechaInicio.Value);

            if (filtro.FechaFin.HasValue)
                query = query.Where(b =>
                    b.FechaEmision <= filtro.FechaFin.Value);

            // Ordenar por fecha de emisión descendente
            query = query.OrderByDescending(b => b.FechaEmision);

            // ── Paginación ──────────────────────────────────
            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(BoletoDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<BoletoDataModel>
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
        public async Task<BoletoDataModel> CreateAsync(BoletoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo del boleto no puede ser nulo.");

            // ── Validaciones de negocio ─────────────────────

            if (string.IsNullOrWhiteSpace(model.CodigoBoleto))
                throw new ArgumentException(
                    "El código del boleto es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Clase))
                throw new ArgumentException(
                    "La clase del boleto es obligatoria.");

            if (!ClasesValidas.Contains(model.Clase.ToUpper()))
                throw new ArgumentException(
                    $"Clase inválida. Las válidas son: {string.Join(", ", ClasesValidas)}");

            if (model.IdReserva <= 0)
                throw new ArgumentException(
                    "El ID de la reserva es obligatorio.");

            if (model.IdVuelo <= 0)
                throw new ArgumentException(
                    "El ID del vuelo es obligatorio.");

            if (model.IdAsiento <= 0)
                throw new ArgumentException(
                    "El ID del asiento es obligatorio.");

            if (model.IdFactura <= 0)
                throw new ArgumentException(
                    "El ID de la factura es obligatorio.");

            if (model.PrecioFinal < 0)
                throw new ArgumentException(
                    "El precio final no puede ser negativo.");

            if (model.CargoEquipaje < 0)
                throw new ArgumentException(
                    "El cargo de equipaje no puede ser negativo.");

            // Verificar que no exista ya un boleto con ese código
            var existente = await _uow.BoletoRepository
                                      .GetByCodigoAsync(model.CodigoBoleto);

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe un boleto con el código '{model.CodigoBoleto}'.");

            // ── Construir entidad ───────────────────────────
            var entity = BoletoDataMapper.ToEntity(model);

            // Campos de auditoría
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.Estado = "ACTIVO";

            // Estado inicial del boleto
            entity.EstadoBoleto = string.IsNullOrWhiteSpace(model.EstadoBoleto)
                ? "ACTIVO"
                : model.EstadoBoleto.ToUpper();

            // Fecha de emisión
            entity.FechaEmision = model.FechaEmision == default
                ? DateTime.UtcNow
                : model.FechaEmision;

            // Normalizar clase
            entity.Clase = model.Clase.ToUpper();

            // Persistir
            await _uow.BoletoRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return BoletoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(BoletoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo del boleto no puede ser nulo.");

            if (model.IdBoleto <= 0)
                throw new ArgumentException(
                    "El ID del boleto debe ser mayor a 0.");

            // Buscar entidad existente
            var entity = await _uow.BoletoRepository.GetByIdAsync(model.IdBoleto);

            if (entity == null)
                return false;

            // Validar clase si viene
            if (!string.IsNullOrWhiteSpace(model.Clase) &&
                !ClasesValidas.Contains(model.Clase.ToUpper()))
                throw new ArgumentException(
                    $"Clase inválida. Las válidas son: {string.Join(", ", ClasesValidas)}");

            // Validar estado boleto si viene
            if (!string.IsNullOrWhiteSpace(model.EstadoBoleto) &&
                !EstadosBoletoValidos.Contains(model.EstadoBoleto.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: {string.Join(", ", EstadosBoletoValidos)}");

            // No permitir modificar un boleto ya USADO o CANCELADO
            if (entity.EstadoBoleto == "USADO")
                throw new InvalidOperationException(
                    "No se puede modificar un boleto que ya fue USADO.");

            if (entity.EstadoBoleto == "CANCELADO")
                throw new InvalidOperationException(
                    "No se puede modificar un boleto que fue CANCELADO.");

            // Aplicar cambios mediante el mapper
            // UpdateEntity NO toca: IdReserva, IdVuelo, IdAsiento, IdFactura
            BoletoDataMapper.UpdateEntity(entity, model);

            // Auditoría de modificación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.BoletoRepository.Update(entity);
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
                    "El ID del boleto debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.BoletoRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            // No permitir eliminar un boleto ya USADO
            if (entity.EstadoBoleto == "USADO")
                throw new InvalidOperationException(
                    "No se puede eliminar un boleto que ya fue USADO.");

            // Soft delete via repositorio
            // El repo ya hace: entity.EsEliminado = true + Update
            _uow.BoletoRepository.Delete(entity);

            // Auditoría de eliminación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}