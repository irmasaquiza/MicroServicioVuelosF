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
using Microservicio.Vuelos.Business.DTOs.Booking.Vuelo;

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

        // ============================================================
        // CREATE
        // ============================================================
        public async Task<VueloResponse> CrearAsync(CrearVueloRequest request)
        {
            VueloValidator.ValidarCrear(request);


            var todos = await _vueloDataService.GetAllAsync();

            var existente = todos.FirstOrDefault(v =>
                v.CodigoVuelo == request.NumeroVuelo);

            if (existente != null)
                throw new BusinessException(
                    "VUELO_DUPLICADO",
                    $"Ya existe un vuelo con el número '{request.NumeroVuelo}'.");

            var dataModel = VueloBusinessMapper.ToDataModel(request);
            var creado = await _vueloDataService.CreateAsync(dataModel);
            await GenerarAsientos(creado.IdVuelo, request);
            return VueloBusinessMapper.ToResponse(creado);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<VueloResponse> GetByIdAsync(int id)
        {
            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            return VueloBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // DETALLE
        // ============================================================
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

        // ============================================================
        // GET ALL
        // ============================================================
        public async Task<IEnumerable<VueloResponse>> GetAllAsync()
        {
            var todos = await _vueloDataService.GetAllAsync();
            return todos.Select(VueloBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRO
        // ============================================================
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
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _vueloDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(VueloBusinessMapper.ToResponse);
        }

        // ============================================================
        // UPDATE
        // ============================================================
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

        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoVueloRequest request)
        {
            VueloValidator.ValidarEstado(request);

            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            if (model.EstadoVuelo == "CANCELADO")
                throw new BusinessException(
                    "VUELO_YA_CANCELADO",
                    "El vuelo ya se encuentra cancelado.");

            if (model.EstadoVuelo == "ATERRIZADO" && request.EstadoVuelo == "CANCELADO")
                throw new BusinessException(
                    "VUELO_ATERRIZADO",
                    "No se puede cancelar un vuelo que ya aterrizó.");

            // 🔥 AQUÍ ESTÁ EL FIX
            if (request.EstadoVuelo == "CANCELADO")
            {
                model.EstadoVuelo = "CANCELADO";
                await _vueloDataService.UpdateAsync(model);
            }
            else
            {
                model.EstadoVuelo = request.EstadoVuelo.ToUpper();
                await _vueloDataService.UpdateAsync(model);
            }

            return true;
        }



        // ============================================================
        // DELETE
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _vueloDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Vuelo", id);

            if (model.EstadoVuelo == "EN_VUELO")
                throw new BusinessException(
                    "VUELO_EN_CURSO",
                    "No se puede eliminar un vuelo en curso.");

            await _vueloDataService.DeleteAsync(id);

            return true;
        }

        private async Task GenerarAsientos(int idVuelo, CrearVueloRequest request)
        {
            var asientos = new List<AsientoDataModel>();

            int total = request.CapacidadTotal;

            // 🔥 distribución automática
            int primera = (int)(total * 0.15);     // 15%
            int ejecutiva = (int)(total * 0.35);   // 35%
            int economica = total - primera - ejecutiva;

            // 🔹 PRIMERA
            for (int i = 1; i <= primera; i++)
            {
                asientos.Add(new AsientoDataModel
                {
                    IdVuelo = idVuelo,
                    NumeroAsiento = $"A{i}",
                    Clase = "PRIMERA",
                    PrecioExtra = 100,
                    Disponible = true
                });
            }

            // 🔹 EJECUTIVA
            for (int i = 1; i <= ejecutiva; i++)
            {
                asientos.Add(new AsientoDataModel
                {
                    IdVuelo = idVuelo,
                    NumeroAsiento = $"B{i}",
                    Clase = "EJECUTIVA",
                    PrecioExtra = 50,
                    Disponible = true
                });
            }

            // 🔹 ECONOMICA
            for (int i = 1; i <= economica; i++)
            {
                asientos.Add(new AsientoDataModel
                {
                    IdVuelo = idVuelo,
                    NumeroAsiento = $"C{i}",
                    Clase = "ECONOMICA",
                    PrecioExtra = 0,
                    Disponible = true
                });
            }

            await _asientoDataService.CreateRangeAsync(asientos);
        }

        // boooking

        public async Task<IEnumerable<VueloBookingResponse>> BuscarBookingAsync(VueloBookingFiltroRequest request)
        {
            // 🔥 convertir fecha a rango
            var fechaInicio = request.FechaSalida.Date;
            var fechaFin = request.FechaSalida.Date.AddDays(1).AddTicks(-1);

            // 🔥 armar filtro internal
            var filtroInternal = new VueloFiltroRequest
            {
                IdAeropuertoOrigen = request.IdAeropuertoOrigen,
                IdAeropuertoDestino = request.IdAeropuertoDestino,
                EstadoVuelo = request.EstadoVuelo,
                FechaSalidaInicio = fechaInicio,
                FechaSalidaFin = fechaFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            // 🔥 reutilizar lógica existente
            var vuelos = await FiltrarAsync(filtroInternal);

            // 🔥 mapear a Booking
            return vuelos.Select(v => new VueloBookingResponse
            {
                IdVuelo = v.IdVuelo,
                NumeroVuelo = v.NumeroVuelo,
                IdAeropuertoOrigen = v.IdAeropuertoOrigen,
                IdAeropuertoDestino = v.IdAeropuertoDestino,
                FechaHoraSalida = v.FechaHoraSalida,
                FechaHoraLlegada = v.FechaHoraLlegada,
                DuracionMin = v.DuracionMin,
                PrecioBase = v.PrecioBase,
                CapacidadTotal = v.CapacidadTotal,
                EstadoVuelo = v.EstadoVuelo
            });
        }



    }
}