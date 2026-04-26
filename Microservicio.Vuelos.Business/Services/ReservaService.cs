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
        // CREAR
        // ============================================================
        public async Task<ReservaResponse> CrearAsync(CrearReservaRequest request)
        {
            ReservaValidator.ValidarCrear(request);

            if (request.FechaFin <= request.FechaInicio)
                throw new BusinessException("FECHAS_INVALIDAS",
                    "La fecha fin debe ser mayor a la fecha inicio.");

            var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);
            if (vuelo == null)
                throw new BusinessException("VUELO_NO_ENCONTRADO");

            if (vuelo.EstadoVuelo == "CANCELADO")
                throw new BusinessException("VUELO_CANCELADO");

            var asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento);

            if (asiento == null)
                throw new BusinessException("ASIENTO_NO_ENCONTRADO");

            if (!asiento.Disponible)
                throw new BusinessException("ASIENTO_OCUPADO");

            if (asiento.IdVuelo != request.IdVuelo)
                throw new BusinessException("ASIENTO_NO_CORRESPONDE");

            var dataModel = ReservaBusinessMapper.ToDataModel(request);
            dataModel.CodigoReserva = $"RES-{System.DateTime.UtcNow:yyyyMMdd}-{System.Guid.NewGuid().ToString("N")[..6]}";

            var creada = await _reservaDataService.CreateAsync(dataModel);

            // 🔒 bloquear asiento
            asiento.Disponible = false;
            await _asientoDataService.UpdateAsync(asiento);

            return ReservaBusinessMapper.ToResponse(creada);
        }

        // ============================================================
        // GET BY ID
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
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            if (request.FechaInicio.HasValue)
                model.FechaInicio = request.FechaInicio.Value;

            if (request.FechaFin.HasValue)
                model.FechaFin = request.FechaFin.Value;

            if (request.TotalReserva.HasValue)
                model.TotalReserva = request.TotalReserva.Value;

            if (!string.IsNullOrWhiteSpace(request.ContactoEmail))
                model.ContactoEmail = request.ContactoEmail;

            await _reservaDataService.UpdateAsync(model);

            return ReservaBusinessMapper.ToResponse(model);
        }
        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoReservaRequest request)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            var nuevoEstado = request.EstadoReserva.ToUpper();

            // ============================================================
            // 🔥 CUANDO PASA A CON → CREAR FACTURA AUTOMÁTICA
            // ============================================================
            if (nuevoEstado == "CON")
            {
                var facturas = await _facturaDataService.GetByReservaAsync(id);

                // evitar duplicar factura
                if (facturas == null || !facturas.Any())
                {
                    var numeroFactura = $"FAC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

                    var factura = new FacturaDataModel
                    {
                        IdCliente = model.IdCliente, // 🔥 ESTA ES LA CLAVE
                        IdReserva = id,
                        NumeroFactura = numeroFactura,
                        FechaEmision = DateTime.UtcNow,

                        Subtotal = model.SubtotalReserva,
                        ValorIva = model.ValorIva,
                        CargoServicio = 0,
                        Total = model.TotalReserva,

                        Estado = "ABI",
                        ObservacionesFactura = "Generada automáticamente",
                        OrigenCanalFactura = "SISTEMA",
                        ServicioOrigen = "VUELOS"
                    };

                    await _facturaDataService.CreateAsync(factura);
                }
            }

            // ============================================================
            // 🔥 VALIDAR FIN SOLO SI EL VUELO ATERRIZÓ
            // ============================================================
            if (nuevoEstado == "FIN")
            {
                var vuelo = await _vueloDataService.GetByIdAsync(model.IdVuelo);

                if (vuelo == null)
                    throw new BusinessException("VUELO_NO_ENCONTRADO",
                        "No se encontró el vuelo asociado.");

                if (vuelo.EstadoVuelo != "ATERRIZADO")
                    throw new BusinessException("VUELO_NO_FINALIZADO",
                        "No se puede finalizar la reserva si el vuelo no ha aterrizado.");
            }

            // 🔥 aplicar cambio
            model.EstadoReserva = nuevoEstado;

            await _reservaDataService.UpdateAsync(model);

            return true;
        }

        // ============================================================
        // CANCELAR
        // ============================================================
        public async Task<bool> CancelarAsync(int id, string motivo)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

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

            await _reservaDataService.DeleteAsync(id);

            return true;
        }
    }
}