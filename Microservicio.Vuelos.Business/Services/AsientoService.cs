

// ============================================================
// Services/AsientoService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Asiento;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class AsientoService : IAsientoService
    {
        private readonly IAsientoDataService _asientoDataService;
        private readonly IVueloDataService _vueloDataService;
        private readonly IAuditoriaLogService _auditoria;

        public AsientoService(
            IAsientoDataService asientoDataService,
            IVueloDataService vueloDataService,
            IAuditoriaLogService auditoria)
        {
            _asientoDataService = asientoDataService;
            _vueloDataService = vueloDataService;
            _auditoria = auditoria;
        }

        public async Task<AsientoResponse> CrearAsync(int idVuelo, CrearAsientoRequest request)
        {
            AsientoValidator.ValidarCrear(request);

            var vuelo = await _vueloDataService.GetByIdAsync(idVuelo);
            if (vuelo == null)
                throw new NotFoundException("Vuelo", idVuelo);

            var dataModel = AsientoBusinessMapper.ToDataModel(request);
            dataModel.IdVuelo = idVuelo;

            var creado = await _asientoDataService.CreateAsync(dataModel);
            return AsientoBusinessMapper.ToResponse(creado);
        }

        public async Task<AsientoResponse> GetByIdAsync(int id)
        {
            var model = await _asientoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Asiento", id);

            return AsientoBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<AsientoResponse>> GetByVueloAsync(int idVuelo)
        {
            var vuelo = await _vueloDataService.GetByIdAsync(idVuelo);
            if (vuelo == null)
                throw new NotFoundException("Vuelo", idVuelo);

            var asientos = await _asientoDataService.GetByVueloAsync(idVuelo);
            return asientos.Select(AsientoBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<AsientoResponse>> FiltrarAsync(AsientoFiltroRequest request)
        {
            var filtro = new AsientoFiltroDataModel
            {
                IdVuelo = request.IdVuelo,
                Clase = request.Clase,
                Disponible = request.Disponible,
                Posicion = request.Posicion,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _asientoDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(AsientoBusinessMapper.ToResponse);
        }

        public async Task<AsientoResponse> ActualizarAsync(int id, ActualizarAsientoRequest request)
        {
            AsientoValidator.ValidarActualizar(request);

            var model = await _asientoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Asiento", id);

            if (!string.IsNullOrWhiteSpace(request.NumeroAsiento))
                model.NumeroAsiento = request.NumeroAsiento.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.Clase))
                model.Clase = request.Clase.ToUpper();
            if (request.Disponible.HasValue)
                model.Disponible = request.Disponible.Value;
            if (request.PrecioExtra.HasValue)
                model.PrecioExtra = request.PrecioExtra.Value;
            if (!string.IsNullOrWhiteSpace(request.Posicion))
                model.Posicion = request.Posicion.ToUpper();

            await _asientoDataService.UpdateAsync(model);
            return AsientoBusinessMapper.ToResponse(model);
        }

        public async Task<bool> CambiarDisponibilidadAsync(int id, bool disponible)
        {
            var model = await _asientoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Asiento", id);

            model.Disponible = disponible;
            await _asientoDataService.UpdateAsync(model);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _asientoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Asiento", id);

            await _asientoDataService.DeleteAsync(id);
            return true;
        }
    }
}