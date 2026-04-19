// ============================================================
// Services/BoletoService.cs
// ============================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoDataService _boletoDataService;
        private readonly IReservaDataService _reservaDataService;
        private readonly IVueloDataService _vueloDataService;
        private readonly IAsientoDataService _asientoDataService;

        public BoletoService(
            IBoletoDataService boletoDataService,
            IReservaDataService reservaDataService,
            IVueloDataService vueloDataService,
            IAsientoDataService asientoDataService)
        {
            _boletoDataService = boletoDataService;
            _reservaDataService = reservaDataService;
            _vueloDataService = vueloDataService;
            _asientoDataService = asientoDataService;
        }

        // ============================================================
        // CREAR BOLETO
        // ============================================================
        public async Task<BoletoResponse> CrearAsync(CrearBoletoRequest request)
        {
            BoletoValidator.ValidarCrear(request);

            // 🔥 Validar reserva
            var reserva = await _reservaDataService.GetByIdAsync(request.IdReserva);
            if (reserva == null)
                throw new BusinessException("RESERVA_NO_ENCONTRADA",
                    $"No existe la reserva '{request.IdReserva}'.");

            // 🔥 Validar vuelo
            var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);
            if (vuelo == null)
                throw new BusinessException("VUELO_NO_ENCONTRADO",
                    $"No existe el vuelo '{request.IdVuelo}'.");

            // 🔥 Validar asiento
            var asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento);
            if (asiento == null)
                throw new BusinessException("ASIENTO_NO_ENCONTRADO",
                    $"No existe el asiento '{request.IdAsiento}'.");

            if (!asiento.Disponible)
                throw new BusinessException("ASIENTO_OCUPADO",
                    $"El asiento '{asiento.NumeroAsiento}' ya está ocupado.");

            if (asiento.IdVuelo != request.IdVuelo)
                throw new BusinessException("ASIENTO_INVALIDO",
                    "El asiento no pertenece al vuelo.");

            // 🔥 Generar código
            var codigo = $"BT-{System.DateTime.UtcNow:yyyyMMdd}-{System.Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            // 🔥 Calcular precio real
            decimal precioBase = vuelo.PrecioBase;
            decimal precioExtra = asiento.PrecioExtra;
            decimal impuestos = (precioBase + precioExtra) * 0.12m;
            decimal total = precioBase + precioExtra + impuestos;

            var dataModel = BoletoBusinessMapper.ToDataModel(request);
            dataModel.CodigoBoleto = codigo;
            dataModel.PrecioVueloBase = precioBase;
            dataModel.PrecioAsientoExtra = precioExtra;
            dataModel.ImpuestosBoleto = impuestos;
            dataModel.PrecioFinal = total;

            var creado = await _boletoDataService.CreateAsync(dataModel);

            // 🔥 Marcar asiento ocupado
            asiento.Disponible = false;
            await _asientoDataService.UpdateAsync(asiento);

            return BoletoBusinessMapper.ToResponse(creado);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<BoletoResponse> GetByIdAsync(int id)
        {
            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            return BoletoBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // POR RESERVA
        // ============================================================
        public async Task<IEnumerable<BoletoResponse>> GetByReservaAsync(int idReserva)
        {
            var boletos = await _boletoDataService.GetByReservaAsync(idReserva);
            return boletos.Select(BoletoBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRAR
        // ============================================================
        public async Task<IEnumerable<BoletoResponse>> FiltrarAsync(BoletoFiltroRequest request)
        {
            var filtro = new BoletoFiltroDataModel
            {
                IdReserva = request.IdReserva,
                IdVuelo = request.IdVuelo,
                IdAsiento = request.IdAsiento,
                IdFactura = request.IdFactura,
                CodigoBoleto = request.CodigoBoleto,
                Clase = request.Clase,
                EstadoBoleto = request.EstadoBoleto,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _boletoDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(BoletoBusinessMapper.ToResponse);
        }

        // ============================================================
        // ACTUALIZAR
        // ============================================================
        public async Task<BoletoResponse> ActualizarAsync(int id, ActualizarBoletoRequest request)
        {
            BoletoValidator.ValidarActualizar(request);

            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            if (model.EstadoBoleto == "USADO")
                throw new BusinessException("BOLETO_USADO",
                    "No se puede modificar un boleto usado.");

            if (model.EstadoBoleto == "CANCELADO")
                throw new BusinessException("BOLETO_CANCELADO",
                    "No se puede modificar un boleto cancelado.");

            if (!string.IsNullOrWhiteSpace(request.Clase))
                model.Clase = request.Clase.ToUpper();

            await _boletoDataService.UpdateAsync(model);

            return BoletoBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var estadosValidos = new[] { "ACTIVO", "USADO", "CANCELADO" };

            if (!estadosValidos.Contains(estado?.ToUpper()))
                throw new ValidationException("estado",
                    "El estado debe ser ACTIVO, USADO o CANCELADO.");

            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            if (model.EstadoBoleto == "USADO")
                throw new BusinessException("BOLETO_USADO",
                    "No se puede cambiar estado de un boleto usado.");

            model.EstadoBoleto = estado.ToUpper();
            await _boletoDataService.UpdateAsync(model);

            return true;
        }

        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            if (model.EstadoBoleto == "USADO")
                throw new BusinessException("BOLETO_USADO",
                    "No se puede eliminar un boleto usado.");

            await _boletoDataService.DeleteAsync(id);
            return true;
        }
    }
}