// ============================================================
// FacturaDataService.cs
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
    public class FacturaDataService : IFacturaDataService
    {
        private readonly IUnitOfWork _uow;

        // ABI=Abierta, APR=Aprobada, INA=Inactiva/Anulada
        private static readonly string[] EstadosValidos =
            { "ABI", "APR", "INA" };

        public FacturaDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<FacturaDataModel>> GetAllAsync()
        {
            var entities = await _uow.FacturaRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<FacturaDataModel>();

            return FacturaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<FacturaDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la factura debe ser mayor a 0.", nameof(id));

            var entity = await _uow.FacturaRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return FacturaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CLIENTE
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<FacturaDataModel>> GetByClienteAsync(int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.", nameof(idCliente));

            var entities = await _uow.FacturaRepository.GetByClienteAsync(idCliente);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<FacturaDataModel>();

            return FacturaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY RESERVA
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<FacturaDataModel>> GetByReservaAsync(int idReserva)
        {
            if (idReserva <= 0)
                throw new ArgumentException(
                    "El ID de la reserva debe ser mayor a 0.", nameof(idReserva));

            var entities = await _uow.FacturaRepository.GetByReservaAsync(idReserva);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<FacturaDataModel>();

            return FacturaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY METODO PAGO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<FacturaDataModel>> GetByMetodoPagoAsync(
            int idMetodo)
        {
            if (idMetodo <= 0)
                throw new ArgumentException(
                    "El ID del método de pago debe ser mayor a 0.",
                    nameof(idMetodo));

            var todos = await _uow.FacturaRepository.GetAllAsync();

            var filtrados = todos
                .Where(f => f.IdMetodo == idMetodo)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<FacturaDataModel>();

            return FacturaDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET BY NUMERO
        // ─────────────────────────────────────────────
        public async Task<FacturaDataModel> GetByNumeroAsync(string numeroFactura)
        {
            if (string.IsNullOrWhiteSpace(numeroFactura))
                throw new ArgumentException(
                    "El número de factura no puede estar vacío.",
                    nameof(numeroFactura));

            var entity = await _uow.FacturaRepository
                                   .GetByNumeroAsync(numeroFactura.Trim());

            if (entity == null)
                return null;

            return FacturaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<FacturaDataModel>> GetPagedAsync(
            FacturaFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.FacturaRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(f => f.IdCliente == filtro.IdCliente.Value);

            if (filtro.IdReserva.HasValue)
                query = query.Where(f => f.IdReserva == filtro.IdReserva.Value);

            if (filtro.IdMetodo.HasValue)
                query = query.Where(f => f.IdMetodo == filtro.IdMetodo.Value);

            if (!string.IsNullOrWhiteSpace(filtro.NumeroFactura))
                query = query.Where(f =>
                    f.NumeroFactura.Contains(filtro.NumeroFactura.Trim()));

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(f =>
                    f.Estado.ToUpper() == filtro.Estado.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.OrigenCanalFactura))
                query = query.Where(f =>
                    f.OrigenCanalFactura != null &&
                    f.OrigenCanalFactura.ToUpper() ==
                    filtro.OrigenCanalFactura.ToUpper());

            if (filtro.TotalMin.HasValue)
                query = query.Where(f => f.Total >= filtro.TotalMin.Value);

            if (filtro.TotalMax.HasValue)
                query = query.Where(f => f.Total <= filtro.TotalMax.Value);

            if (filtro.FechaInicio.HasValue)
                query = query.Where(f =>
                    f.FechaEmision >= filtro.FechaInicio.Value);

            if (filtro.FechaFin.HasValue)
                query = query.Where(f =>
                    f.FechaEmision <= filtro.FechaFin.Value);

            query = query.OrderByDescending(f => f.FechaEmision);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(FacturaDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<FacturaDataModel>
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
        public async Task<FacturaDataModel> CreateAsync(FacturaDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.NumeroFactura))
                throw new ArgumentException(
                    "El número de factura es obligatorio.");

            if (model.IdCliente <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio.");

            if (model.IdReserva <= 0)
                throw new ArgumentException("El ID de la reserva es obligatorio.");

            if (model.IdMetodo <= 0)
                throw new ArgumentException(
                    "El ID del método de pago es obligatorio.");

            if (model.Subtotal < 0)
                throw new ArgumentException("El subtotal no puede ser negativo.");

            if (model.ValorIva < 0)
                throw new ArgumentException("El IVA no puede ser negativo.");

            if (model.CargoServicio < 0)
                throw new ArgumentException(
                    "El cargo de servicio no puede ser negativo.");

            if (model.Total < 0)
                throw new ArgumentException("El total no puede ser negativo.");

            // CHK_FACTURAS_COHERENTE → total >= subtotal
            if (model.Total < model.Subtotal)
                throw new ArgumentException(
                    "El total no puede ser menor al subtotal.");

            // Verificar número de factura único
            var existente = await _uow.FacturaRepository
                                      .GetByNumeroAsync(model.NumeroFactura.Trim());

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe una factura con el número '{model.NumeroFactura}'.");

            // Verificar que cliente exista
            var cliente = await _uow.ClienteRepository
                                    .GetByIdAsync(model.IdCliente);

            if (cliente == null)
                throw new InvalidOperationException(
                    $"No existe un cliente con ID '{model.IdCliente}'.");

            // Verificar que reserva exista
            var reserva = await _uow.ReservaRepository
                                    .GetByIdAsync(model.IdReserva);

            if (reserva == null)
                throw new InvalidOperationException(
                    $"No existe una reserva con ID '{model.IdReserva}'.");

            // Verificar que método de pago exista
            var metodo = await _uow.MetodoPagoRepository
                                   .GetByIdAsync(model.IdMetodo);

            if (metodo == null)
                throw new InvalidOperationException(
                    $"No existe un método de pago con ID '{model.IdMetodo}'.");

            var entity = FacturaDataMapper.ToEntity(model);

            entity.GuidFactura = Guid.NewGuid();
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.FechaEmision = DateTime.UtcNow;

            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                ? "ABI"
                : model.Estado.ToUpper();

            entity.ServicioOrigen = string.IsNullOrWhiteSpace(model.ServicioOrigen)
                ? "VUELOS"
                : model.ServicioOrigen.ToUpper();

            await _uow.FacturaRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return FacturaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(FacturaDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdFactura <= 0)
                throw new ArgumentException(
                    "El ID de la factura debe ser mayor a 0.");

            var entity = await _uow.FacturaRepository.GetByIdAsync(model.IdFactura);

            if (entity == null)
                return false;

            // No modificar factura anulada
            if (entity.Estado == "INA")
                throw new InvalidOperationException(
                    "No se puede modificar una factura anulada.");

            if (!string.IsNullOrWhiteSpace(model.Estado) &&
                !EstadosValidos.Contains(model.Estado.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosValidos)}");

            if (model.Total < model.Subtotal)
                throw new ArgumentException(
                    "El total no puede ser menor al subtotal.");

            // UpdateEntity NO toca: IdCliente, IdReserva, IdMetodo, GuidFactura
            FacturaDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.FacturaRepository.Update(entity);
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
                    "El ID de la factura debe ser mayor a 0.", nameof(id));

            var entity = await _uow.FacturaRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            // No eliminar factura aprobada
            if (entity.Estado == "APR")
                throw new InvalidOperationException(
                    "No se puede eliminar una factura ya aprobada.");

            _uow.FacturaRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";
            entity.MotivoInhabilitacion = "Eliminación lógica del registro.";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}