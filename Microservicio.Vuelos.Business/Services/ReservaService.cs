// ============================================================
// Services/ReservaService.cs
// ============================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Reserva;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaDataService _reservaDataService;
        private readonly IBoletoDataService _boletoDataService;
        private readonly IFacturaDataService _facturaDataService;
        private readonly IAsientoDataService _asientoDataService;
        private readonly IVueloDataService _vueloDataService;

        public ReservaService(
            IReservaDataService reservaDataService,
            IBoletoDataService boletoDataService,
            IFacturaDataService facturaDataService,
            IAsientoDataService asientoDataService,
            IVueloDataService vueloDataService)
        {
            _reservaDataService = reservaDataService;
            _boletoDataService = boletoDataService;
            _facturaDataService = facturaDataService;
            _asientoDataService = asientoDataService;
            _vueloDataService = vueloDataService;
        }

        // ============================================================
        // CREAR RESERVA
        // ============================================================
        public async Task<ReservaResponse> CrearAsync(CrearReservaRequest request)
        {
            ReservaValidator.ValidarCrear(request);

            // 🔥 Validar fechas
            if (request.FechaFin <= request.FechaInicio)
                throw new BusinessException("FECHAS_INVALIDAS",
                    "La fecha fin debe ser mayor a la fecha inicio.");

            // 🔥 Validar vuelo
            var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);
            if (vuelo == null)
                throw new BusinessException("VUELO_NO_ENCONTRADO",
                    $"No existe el vuelo con ID '{request.IdVuelo}'.");

            if (vuelo.EstadoVuelo == "CANCELADO")
                throw new BusinessException("VUELO_CANCELADO",
                    "No se puede reservar en un vuelo cancelado.");

            if (vuelo.CapacidadDisponible <= 0)
                throw new BusinessException("VUELO_SIN_CAPACIDAD",
                    "El vuelo no tiene asientos disponibles.");

            // 🔥 Validar asiento
            var asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento);
            if (asiento == null)
                throw new BusinessException("ASIENTO_NO_ENCONTRADO",
                    $"No existe el asiento con ID '{request.IdAsiento}'.");

            if (!asiento.Disponible)
                throw new BusinessException("ASIENTO_NO_DISPONIBLE",
                    $"El asiento '{asiento.NumeroAsiento}' no está disponible.");

            // 🔥 Validar que el asiento pertenece al vuelo
            if (asiento.IdVuelo != request.IdVuelo)
                throw new BusinessException("ASIENTO_NO_CORRESPONDE",
                    "El asiento no pertenece al vuelo seleccionado.");

            // 🔥 Generar código
            var codigo = $"RES-{System.DateTime.UtcNow:yyyyMMdd}-{System.Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            var dataModel = ReservaBusinessMapper.ToDataModel(request);
            dataModel.CodigoReserva = codigo;

            // 🔥 Crear reserva
            var creada = await _reservaDataService.CreateAsync(dataModel);

            // 🔥 Bloquear asiento
            asiento.Disponible = false;
            await _asientoDataService.UpdateAsync(asiento);

            // 🔥 Reducir capacidad del vuelo
            vuelo.CapacidadDisponible -= 1;
            if (vuelo.CapacidadDisponible < 0)
                vuelo.CapacidadDisponible = 0;

            await _vueloDataService.UpdateAsync(vuelo);

            return ReservaBusinessMapper.ToResponse(creada);
        }

        // ============================================================
        // OBTENER POR ID
        // ============================================================
        public async Task<ReservaResponse> GetByIdAsync(int id)
        {
            var model = await _reservaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Reserva", id);

            return ReservaBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // DETALLE
        // ============================================================
        public async Task<ReservaDetalleResponse> GetDetalleAsync(int id)
        {
            var model = await _reservaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Reserva", id);

            var boletos = await _boletoDataService.GetByReservaAsync(id);
            var facturas = await _facturaDataService.GetByReservaAsync(id);

            return ReservaBusinessMapper.ToDetalleResponse(model, boletos, facturas);
        }

        // ============================================================
        // POR CLIENTE
        // ============================================================
        public async Task<IEnumerable<ReservaResponse>> GetByClienteAsync(int idCliente)
        {
            var reservas = await _reservaDataService.GetByClienteAsync(idCliente);
            return reservas.Select(ReservaBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRAR
        // ============================================================
        public async Task<IEnumerable<ReservaResponse>> FiltrarAsync(ReservaFiltroRequest request)
        {
            var filtro = new ReservaFiltroDataModel
            {
                IdCliente = request.IdCliente,
                IdPasajero = request.IdPasajero,
                IdVuelo = request.IdVuelo,
                CodigoReserva = request.CodigoReserva,
                EstadoReserva = request.EstadoReserva,
                OrigenCanalReserva = request.OrigenCanalReserva,
                TotalMin = request.TotalMin,
                TotalMax = request.TotalMax,
                FechaReservaInicio = request.FechaReservaInicio,
                FechaReservaFin = request.FechaReservaFin,
                FechaInicioViaje = request.FechaInicioViaje,
                FechaFinViaje = request.FechaFinViaje,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _reservaDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(ReservaBusinessMapper.ToResponse);
        }

        // ============================================================
        // ACTUALIZAR
        // ============================================================
        public async Task<ReservaResponse> ActualizarAsync(int id, ActualizarReservaRequest request)
        {
            ReservaValidator.ValidarActualizar(request);

            var model = await _reservaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Reserva", id);

            if (model.EstadoReserva == "CAN" || model.EstadoReserva == "FIN")
                throw new BusinessException("RESERVA_EN_ESTADO_FINAL",
                    "No se puede modificar una reserva cancelada o finalizada.");

            if (request.FechaInicio.HasValue)
                model.FechaInicio = request.FechaInicio.Value;
            if (request.FechaFin.HasValue)
                model.FechaFin = request.FechaFin.Value;
            if (request.SubtotalReserva.HasValue)
                model.SubtotalReserva = request.SubtotalReserva.Value;
            if (request.ValorIva.HasValue)
                model.ValorIva = request.ValorIva.Value;
            if (request.TotalReserva.HasValue)
                model.TotalReserva = request.TotalReserva.Value;
            if (!string.IsNullOrWhiteSpace(request.ContactoEmail))
                model.ContactoEmail = request.ContactoEmail.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(request.ContactoTelefono))
                model.ContactoTelefono = request.ContactoTelefono.Trim();
            if (!string.IsNullOrWhiteSpace(request.Observaciones))
                model.Observaciones = request.Observaciones.Trim();

            await _reservaDataService.UpdateAsync(model);

            return ReservaBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoReservaRequest request)
        {
            ReservaValidator.ValidarEstado(request);

            var model = await _reservaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Reserva", id);

            if (model.EstadoReserva == "CAN")
                throw new BusinessException("RESERVA_YA_CANCELADA",
                    "La reserva ya está cancelada.");

            if (model.EstadoReserva == "FIN")
                throw new BusinessException("RESERVA_FINALIZADA",
                    "No se puede cambiar el estado de una reserva finalizada.");

            if (request.EstadoReserva == "CAN")
                await _reservaDataService.CancelAsync(id, request.MotivoCancelacion);
            else
            {
                model.EstadoReserva = request.EstadoReserva.ToUpper();
                await _reservaDataService.UpdateAsync(model);
            }

            return true;
        }

        // ============================================================
        // CANCELAR
        // ============================================================
        public async Task<bool> CancelarAsync(int id, string motivo)
        {
            if (string.IsNullOrWhiteSpace(motivo))
                throw new ValidationException("motivo",
                    "El motivo de cancelación es obligatorio.");

            var model = await _reservaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Reserva", id);

            if (model.EstadoReserva == "CAN")
                throw new BusinessException("RESERVA_YA_CANCELADA",
                    "La reserva ya está cancelada.");

            await _reservaDataService.CancelAsync(id, motivo);

            return true;
        }

        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _reservaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Reserva", id);

            if (model.EstadoReserva == "CON" ||
                model.EstadoReserva == "FIN" ||
                model.EstadoReserva == "EMI")
                throw new BusinessException("RESERVA_NO_ELIMINABLE",
                    "No se puede eliminar una reserva confirmada, finalizada o emitida.");

            await _reservaDataService.DeleteAsync(id);

            return true;
        }
    }
}