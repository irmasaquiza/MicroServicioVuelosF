using Microservicio.Vuelos.Business.DTOs.Internal.Ciudad;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

public class CiudadService : ICiudadService
{
    private readonly ICiudadDataService _ciudadDataService;
    private readonly IPaisDataService _paisDataService;

    public CiudadService(
        ICiudadDataService ciudadDataService,
        IPaisDataService paisDataService)
    {
        _ciudadDataService = ciudadDataService;
        _paisDataService = paisDataService;
    }

    public async Task<CiudadResponse> CrearAsync(CrearCiudadRequest request)
    {
        CiudadValidator.ValidarCrear(request);

        var pais = await _paisDataService.GetByIdAsync(request.IdPais);
        if (pais == null)
            throw new BusinessException(
                "PAIS_NO_ENCONTRADO",
                $"No existe un país con ID '{request.IdPais}'.");

        var dataModel = CiudadBusinessMapper.ToDataModel(request);
        var creada = await _ciudadDataService.CreateAsync(dataModel);

        return CiudadBusinessMapper.ToResponse(creada);
    }

    public async Task<CiudadResponse> GetByIdAsync(int id)
    {
        var model = await _ciudadDataService.GetByIdAsync(id);
        if (model == null)
            throw new NotFoundException("Ciudad", id);

        return CiudadBusinessMapper.ToResponse(model);
    }

    public async Task<IEnumerable<CiudadResponse>> GetAllAsync()
    {
        var todos = await _ciudadDataService.GetAllAsync();
        return todos.Select(CiudadBusinessMapper.ToResponse);
    }

    public async Task<IEnumerable<CiudadResponse>> GetByPaisAsync(int idPais)
    {
        var ciudades = await _ciudadDataService.GetByPaisAsync(idPais);
        return ciudades.Select(CiudadBusinessMapper.ToResponse);
    }

    public async Task<IEnumerable<CiudadResponse>> FiltrarAsync(CiudadFiltroRequest request)
    {
        var filtro = new CiudadFiltroDataModel
        {
            IdPais = request.IdPais,
            Nombre = request.Nombre,
            ZonaHoraria = request.ZonaHoraria,
            Estado = request.Estado,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var resultado = await _ciudadDataService.GetPagedAsync(filtro);
        return resultado.Data.Select(CiudadBusinessMapper.ToResponse);
    }

    public async Task<CiudadResponse> ActualizarAsync(int id, ActualizarCiudadRequest request)
    {
        CiudadValidator.ValidarActualizar(request);

        var model = await _ciudadDataService.GetByIdAsync(id);
        if (model == null)
            throw new NotFoundException("Ciudad", id);

        if (!string.IsNullOrWhiteSpace(request.Nombre))
            model.Nombre = request.Nombre.Trim();
        if (!string.IsNullOrWhiteSpace(request.ZonaHoraria))
            model.ZonaHoraria = request.ZonaHoraria.Trim();
        if (request.Latitud.HasValue)
            model.Latitud = request.Latitud;
        if (request.Longitud.HasValue)
            model.Longitud = request.Longitud;
        if (!string.IsNullOrWhiteSpace(request.Estado))
            model.Estado = request.Estado.ToUpper();

        await _ciudadDataService.UpdateAsync(model);

        return CiudadBusinessMapper.ToResponse(model);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var model = await _ciudadDataService.GetByIdAsync(id);
        if (model == null)
            throw new NotFoundException("Ciudad", id);

        await _ciudadDataService.DeleteAsync(id);

        return true;
    }
}