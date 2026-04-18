// ============================================================
// ReservaDataService.cs
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
    public class ReservaDataService : IReservaDataService
    {
        private readonly IUnitOfWork _uow;

        // PEN, CON, CAN, EXP, FIN, EMI
        private static readonly string[] EstadosValidos =
            { "PEN", "CON", "CAN", "EXP", "FIN", "EMI" };

        public ReservaDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ReservaDataModel>> GetAllAsync()
        {
            var entities = await _uow.ReservaRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ReservaDataModel>();

            return ReservaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<ReservaDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la reserva debe ser mayor a 0.", nameof(id));

            var entity = await _uow.ReservaRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return ReservaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CODIGO
        // ─────────────────────────────────────────────
        public async Task<ReservaDataModel> GetByCodigoAsync(string codigoReserva)
        {
            if (string.IsNullOrWhiteSpace(codigoReserva))
                throw new ArgumentException(
                    "El código de reserva no puede estar vacío.",
                    nameof(codigoReserva));

            var entity = await _uow.ReservaRepository
                                   .GetByCodigoAsync(codigoReserva.Trim());

            if (entity == null)
                return null;

            return ReservaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CLIENTE
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ReservaDataModel>> GetByClienteAsync(
            int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.", nameof(idCliente));

            var entities = await _uow.ReservaRepository.GetByClienteAsync(idCliente);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ReservaDataModel>();

            return ReservaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY PASAJERO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ReservaDataModel>> GetByPasajeroAsync(
            int idPasajero)
        {
            if (idPasajero <= 0)
                throw new ArgumentException(
                    "El ID del pasajero debe ser mayor a 0.", nameof(idPasajero));

            var entities = await _uow.ReservaRepository
                                     .GetByPasajeroAsync(idPasajero);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ReservaDataModel>();

            return ReservaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY VUELO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ReservaDataModel>> GetByVueloAsync(int idVuelo)
        {
            if (idVuelo <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.", nameof(idVuelo));

            var todos = await _uow.ReservaRepository.GetAllAsync();

            var filtrados = todos
                .Where(r => r.IdVuelo == idVuelo)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<ReservaDataModel>();

            return ReservaDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET BY ESTADO
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ReservaDataModel>> GetByEstadoAsync(
            string estadoReserva)
        {
            if (string.IsNullOrWhiteSpace(estadoReserva))
                throw new ArgumentException(
                    "El estado de la reserva no puede estar vacío.",
                    nameof(estadoReserva));

            if (!EstadosValidos.Contains(estadoReserva.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosValidos)}");

            var entities = await _uow.ReservaRepository
                                     .GetByEstadoAsync(estadoReserva.ToUpper());

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ReservaDataModel>();

            return ReservaDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<ReservaDataModel>> GetPagedAsync(
            ReservaFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.ReservaRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(r => r.IdCliente == filtro.IdCliente.Value);

            if (filtro.IdPasajero.HasValue)
                query = query.Where(r => r.IdPasajero == filtro.IdPasajero.Value);

            if (filtro.IdVuelo.HasValue)
                query = query.Where(r => r.IdVuelo == filtro.IdVuelo.Value);

            if (!string.IsNullOrWhiteSpace(filtro.CodigoReserva))
                query = query.Where(r =>
                    r.CodigoReserva.Contains(filtro.CodigoReserva.Trim()));

            if (!string.IsNullOrWhiteSpace(filtro.EstadoReserva))
                query = query.Where(r =>
                    r.EstadoReserva.ToUpper() == filtro.EstadoReserva.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.OrigenCanalReserva))
                query = query.Where(r =>
                    r.OrigenCanalReserva != null &&
                    r.OrigenCanalReserva.ToUpper() ==
                    filtro.OrigenCanalReserva.ToUpper());

            if (filtro.TotalMin.HasValue)
                query = query.Where(r => r.TotalReserva >= filtro.TotalMin.Value);

            if (filtro.TotalMax.HasValue)
                query = query.Where(r => r.TotalReserva <= filtro.TotalMax.Value);

            if (filtro.FechaReservaInicio.HasValue)
                query = query.Where(r =>
                    r.FechaReservaUtc >= filtro.FechaReservaInicio.Value);

            if (filtro.FechaReservaFin.HasValue)
                query = query.Where(r =>
                    r.FechaReservaUtc <= filtro.FechaReservaFin.Value);

            if (filtro.FechaInicioViaje.HasValue)
                query = query.Where(r =>
                    r.FechaInicio >= filtro.FechaInicioViaje.Value);

            if (filtro.FechaFinViaje.HasValue)
                query = query.Where(r =>
                    r.FechaFin <= filtro.FechaFinViaje.Value);

            query = query.OrderByDescending(r => r.FechaReservaUtc);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(ReservaDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<ReservaDataModel>
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
        public async Task<ReservaDataModel> CreateAsync(ReservaDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.CodigoReserva))
                throw new ArgumentException(
                    "El código de reserva es obligatorio.");

            if (model.IdCliente <= 0)
                throw new ArgumentException("El ID del cliente es obligatorio.");

            if (model.IdPasajero <= 0)
                throw new ArgumentException("El ID del pasajero es obligatorio.");

            if (model.IdVuelo <= 0)
                throw new ArgumentException("El ID del vuelo es obligatorio.");

            if (model.IdAsiento <= 0)
                throw new ArgumentException("El ID del asiento es obligatorio.");

            // CHK_RESERVAS_FECHAS → fecha_fin > fecha_inicio
            if (model.FechaFin <= model.FechaInicio)
                throw new ArgumentException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            if (model.SubtotalReserva < 0 ||
                model.ValorIva < 0 ||
                model.TotalReserva < 0)
                throw new ArgumentException(
                    "Los valores económicos no pueden ser negativos.");

            // Verificar código único
            var existente = await _uow.ReservaRepository
                                      .GetByCodigoAsync(model.CodigoReserva.Trim());

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe una reserva con el código '{model.CodigoReserva}'.");

            // Verificar que cliente exista
            var cliente = await _uow.ClienteRepository
                                    .GetByIdAsync(model.IdCliente);

            if (cliente == null)
                throw new InvalidOperationException(
                    $"No existe un cliente con ID '{model.IdCliente}'.");

            // Verificar que pasajero exista
            var pasajero = await _uow.PasajeroRepository
                                     .GetByIdAsync(model.IdPasajero);

            if (pasajero == null)
                throw new InvalidOperationException(
                    $"No existe un pasajero con ID '{model.IdPasajero}'.");

            // Verificar que vuelo exista y esté programado
            var vuelo = await _uow.VueloRepository.GetByIdAsync(model.IdVuelo);

            if (vuelo == null)
                throw new InvalidOperationException(
                    $"No existe un vuelo con ID '{model.IdVuelo}'.");

            if (vuelo.EstadoVuelo == "CANCELADO")
                throw new InvalidOperationException(
                    "No se puede reservar en un vuelo cancelado.");

            // Verificar que asiento exista y esté disponible
            var asiento = await _uow.AsientoRepository.GetByIdAsync(model.IdAsiento);

            if (asiento == null)
                throw new InvalidOperationException(
                    $"No existe un asiento con ID '{model.IdAsiento}'.");

            if (!asiento.Disponible)
                throw new InvalidOperationException(
                    $"El asiento '{asiento.NumeroAsiento}' no está disponible.");

            var entity = ReservaDataMapper.ToEntity(model);

            entity.GuidReserva = Guid.NewGuid();
            entity.FechaReservaUtc = DateTime.UtcNow;
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;

            entity.EstadoReserva = string.IsNullOrWhiteSpace(model.EstadoReserva)
                ? "PEN"
                : model.EstadoReserva.ToUpper();

            entity.ServicioOrigen = string.IsNullOrWhiteSpace(model.ServicioOrigen)
                ? "VUELOS"
                : model.ServicioOrigen.ToUpper();

            entity.OrigenCanalReserva =
                string.IsNullOrWhiteSpace(model.OrigenCanalReserva)
                    ? "WEB"
                    : model.OrigenCanalReserva.ToUpper();

            // Marcar asiento como no disponible
            asiento.Disponible = false;
            _uow.AsientoRepository.Update(asiento);

            await _uow.ReservaRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ReservaDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(ReservaDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdReserva <= 0)
                throw new ArgumentException(
                    "El ID de la reserva debe ser mayor a 0.");

            var entity = await _uow.ReservaRepository.GetByIdAsync(model.IdReserva);

            if (entity == null)
                return false;

            // No modificar reservas en estado terminal
            if (entity.EstadoReserva == "CAN")
                throw new InvalidOperationException(
                    "No se puede modificar una reserva cancelada.");

            if (entity.EstadoReserva == "FIN")
                throw new InvalidOperationException(
                    "No se puede modificar una reserva finalizada.");

            if (!string.IsNullOrWhiteSpace(model.EstadoReserva) &&
                !EstadosValidos.Contains(model.EstadoReserva.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosValidos)}");

            if (model.FechaFin <= model.FechaInicio)
                throw new ArgumentException(
                    "La fecha de fin debe ser posterior a la fecha de inicio.");

            // UpdateEntity NO toca:
            // IdCliente, IdPasajero, IdVuelo, IdAsiento, GuidReserva
            ReservaDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.ReservaRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // CANCEL — método especial de negocio
        // ─────────────────────────────────────────────
        public async Task<bool> CancelAsync(int id, string motivo)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID de la reserva debe ser mayor a 0.", nameof(id));

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException(
                    "El motivo de cancelación es obligatorio.", nameof(motivo));

            var entity = await _uow.ReservaRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            if (entity.EstadoReserva == "CAN")
                throw new InvalidOperationException(
                    "La reserva ya se encuentra cancelada.");

            if (entity.EstadoReserva == "FIN")
                throw new InvalidOperationException(
                    "No se puede cancelar una reserva finalizada.");

            // Cambiar estado a cancelado
            entity.EstadoReserva = "CAN";
            entity.MotivoCancelacion = motivo.Trim();
            entity.FechaCancelacionUtc = DateTime.UtcNow;

            // Auditoría
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            // Liberar el asiento
            var asiento = await _uow.AsientoRepository.GetByIdAsync(entity.IdAsiento);

            if (asiento != null)
            {
                asiento.Disponible = true;
                _uow.AsientoRepository.Update(asiento);
            }

            _uow.ReservaRepository.Update(entity);
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
                    "El ID de la reserva debe ser mayor a 0.", nameof(id));

            var entity = await _uow.ReservaRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            if (entity.EstadoReserva == "CON" ||
                entity.EstadoReserva == "FIN" ||
                entity.EstadoReserva == "EMI")
                throw new InvalidOperationException(
                    "No se puede eliminar una reserva confirmada, " +
                    "finalizada o emitida.");

            _uow.ReservaRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";
            entity.MotivoInhabilitacion = "Eliminación lógica del registro.";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}