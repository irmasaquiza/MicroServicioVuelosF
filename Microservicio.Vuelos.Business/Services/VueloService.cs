using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Vuelo;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class VueloService : IVueloService
    {
        private readonly IVueloDataService _vueloDataService;
        private readonly IEscalaDataService _escalaDataService;
        private readonly IAsientoDataService _asientoDataService;

        public VueloService(
            IVueloDataService vueloDataService,
            IEscalaDataService escalaDataService,
            IAsientoDataService asientoDataService)
        {
            _vueloDataService = vueloDataService;
            _escalaDataService = escalaDataService;
            _asientoDataService = asientoDataService;
        }

        public async Task<VueloResponse> CrearAsync(CrearVueloRequest request)
        {
            VueloValidator.ValidarCrear(request);

            var existente = await _vueloDataService.GetByCodigoAsync(request.NumeroVuelo);
            if (existente != null)
                throw new BusinessException(
                    "VUELO_DUPLICADO",
                    $"Ya existe un vuelo con el número '{request.NumeroVuelo}'.");

            var dataModel = VueloBusinessMapper.ToDataModel(request);
            var creado = await _vueloDataService.CreateAsync(dataModel);

            return VueloBusinessMapper.ToResponse(creado);
        }

        public async Task<VueloResponse> GetByIdAsync(int id)
        {
            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            return VueloBusinessMapper.ToResponse(model);
        }

        public async Task<VueloDetalleResponse> GetDetalleAsync(int id)
        {
            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            var escalas = await _escalaDataService.GetByVueloAsync(id);
            var asientos = await _asientoDataService.GetByVueloAsync(id);

            return VueloBusinessMapper.ToDetalleResponse(
                model,
                escalas,
                asientos
            );
        }

        public async Task<IEnumerable<VueloResponse>> GetAllAsync()
        {
            var todos = await _vueloDataService.GetAllAsync();
            return todos.Select(VueloBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<VueloResponse>> FiltrarAsync(VueloFiltroRequest request)
        {
            var filtro = new VueloFiltroDataModel
            {
                CodigoVuelo = request.NumeroVuelo,
                IdAeropuertoOrigen = request.IdAeropuertoOrigen,
                IdAeropuertoDestino = request.IdAeropuertoDestino,
                EstadoVuelo = request.EstadoVuelo,
                FechaSalidaInicio = request.FechaSalidaInicio,
                FechaSalidaFin = request.FechaSalidaFin,
                PrecioMin = request.PrecioMin,
                PrecioMax = request.PrecioMax,
                CapacidadDisponibleMin = request.CapacidadDisponibleMin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _vueloDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(VueloBusinessMapper.ToResponse);
        }

        public async Task<VueloResponse> ActualizarAsync(int id, ActualizarVueloRequest request)
        {
            VueloValidator.ValidarActualizar(request);

            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            if (model.EstadoVuelo == "CANCELADO")
                throw new BusinessException(
                    "VUELO_CANCELADO",
                    "No se puede modificar un vuelo cancelado.");

            if (!string.IsNullOrWhiteSpace(request.NumeroVuelo))
                model.CodigoVuelo = request.NumeroVuelo.ToUpper();
            if (request.FechaHoraSalida.HasValue)
                model.FechaHoraSalida = request.FechaHoraSalida.Value;
            if (request.FechaHoraLlegada.HasValue)
                model.FechaHoraLlegada = request.FechaHoraLlegada.Value;
            if (request.DuracionMin.HasValue)
                model.DuracionMin = request.DuracionMin.Value;
            if (request.PrecioBase.HasValue)
                model.PrecioBase = request.PrecioBase.Value;
            if (request.CapacidadTotal.HasValue)
                model.CapacidadTotal = request.CapacidadTotal.Value;

            await _vueloDataService.UpdateAsync(model);

            return VueloBusinessMapper.ToResponse(model);
        }

        public async Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoVueloRequest request)
        {
            VueloValidator.ValidarEstado(request);

            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            if (model.EstadoVuelo == "CANCELADO")
                throw new BusinessException("VUELO_YA_CANCELADO",
                    "El vuelo ya se encuentra cancelado.");

            if (model.EstadoVuelo == "ATERRIZADO" && request.EstadoVuelo == "CANCELADO")
                throw new BusinessException("VUELO_ATERRIZADO",
                    "No se puede cancelar un vuelo que ya aterrizó.");

            if (request.EstadoVuelo == "CANCELADO")
                await _vueloDataService.CancelAsync(id, request.Motivo);
            else
            {
                model.EstadoVuelo = request.EstadoVuelo.ToUpper();
                await _vueloDataService.UpdateAsync(model);
            }

            return true;
        }

        public async Task<bool> ActualizarDisponibilidadAsync(int id, int cantidad)
        {
            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            model.CapacidadDisponible += cantidad;
            if (model.CapacidadDisponible < 0)
                model.CapacidadDisponible = 0;

            await _vueloDataService.UpdateAsync(model);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            if (model.EstadoVuelo == "EN_VUELO")
                throw new BusinessException("VUELO_EN_CURSO",
                    "No se puede eliminar un vuelo que está en curso.");

            await _vueloDataService.DeleteAsync(id);

            return true;
        }
    }
}