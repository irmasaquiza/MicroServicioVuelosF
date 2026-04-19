using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Aeropuerto;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class AeropuertoService : IAeropuertoService
    {
        private readonly IAeropuertoDataService _aeropuertoDataService;
        private readonly IPaisDataService _paisDataService;

        public AeropuertoService(
            IAeropuertoDataService aeropuertoDataService,
            IPaisDataService paisDataService)
        {
            _aeropuertoDataService = aeropuertoDataService;
            _paisDataService = paisDataService;
        }

        public async Task<AeropuertoResponse> CrearAsync(CrearAeropuertoRequest request)
        {
            AeropuertoValidator.ValidarCrear(request);

            var pais = await _paisDataService.GetByIdAsync(request.IdPais);
            if (pais == null)
                throw new BusinessException(
                    "PAIS_NO_ENCONTRADO",
                    $"No existe un país con ID '{request.IdPais}'.");

            var todos = await _aeropuertoDataService.GetAllAsync();
            if (todos.Any(a => a.CodigoIata?.ToUpper() == request.CodigoIata.ToUpper()))
                throw new BusinessException(
                    "AEROPUERTO_DUPLICADO",
                    $"Ya existe un aeropuerto con el código IATA '{request.CodigoIata}'.");

            var dataModel = AeropuertoBusinessMapper.ToDataModel(request);
            var creado = await _aeropuertoDataService.CreateAsync(dataModel);

            return AeropuertoBusinessMapper.ToResponse(creado);
        }

        public async Task<AeropuertoResponse> GetByIdAsync(int id)
        {
            var model = await _aeropuertoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Aeropuerto", id);

            return AeropuertoBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<AeropuertoResponse>> GetAllAsync()
        {
            var todos = await _aeropuertoDataService.GetAllAsync();
            return todos.Select(AeropuertoBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<AeropuertoResponse>> FiltrarAsync(AeropuertoFiltroRequest request)
        {
            var filtro = new AeropuertoFiltroDataModel
            {
                CodigoIata = request.CodigoIata,
                CodigoIcao = request.CodigoIcao,
                Nombre = request.Nombre,
                IdCiudad = request.IdCiudad,
                IdPais = request.IdPais,
                Estado = request.Estado,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _aeropuertoDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(AeropuertoBusinessMapper.ToResponse);
        }

        public async Task<AeropuertoResponse> ActualizarAsync(int id, ActualizarAeropuertoRequest request)
        {
            AeropuertoValidator.ValidarActualizar(request);

            var model = await _aeropuertoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Aeropuerto", id);

            if (!string.IsNullOrWhiteSpace(request.CodigoIata))
                model.CodigoIata = request.CodigoIata.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.CodigoIcao))
                model.CodigoIcao = request.CodigoIcao.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.Nombre))
                model.Nombre = request.Nombre.Trim();
            if (request.IdCiudad.HasValue)
                model.IdCiudad = request.IdCiudad;
            if (!string.IsNullOrWhiteSpace(request.ZonaHoraria))
                model.ZonaHoraria = request.ZonaHoraria.Trim();
            if (request.Latitud.HasValue)
                model.Latitud = request.Latitud;
            if (request.Longitud.HasValue)
                model.Longitud = request.Longitud;
            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado.ToUpper();

            await _aeropuertoDataService.UpdateAsync(model);

            return AeropuertoBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _aeropuertoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Aeropuerto", id);

            await _aeropuertoDataService.DeleteAsync(id);

            return true;
        }
    }
}