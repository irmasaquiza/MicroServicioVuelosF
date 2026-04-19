using Microservicio.Vuelos.Business.DTOs.Internal.Pais;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

public class PaisService : IPaisService
{
    private readonly IPaisDataService _paisDataService;

    public PaisService(IPaisDataService paisDataService)
    {
        _paisDataService = paisDataService;
    }

    public async Task<PaisResponse> CrearAsync(CrearPaisRequest request)
    {
        PaisValidator.ValidarCrear(request);

        var existente = await _paisDataService.GetByIso2Async(request.CodigoIso2);
        if (existente != null)
            throw new BusinessException(
                "PAIS_DUPLICADO",
                $"Ya existe un país con el código ISO2 '{request.CodigoIso2}'.");

        var dataModel = PaisBusinessMapper.ToDataModel(request);
        var creado = await _paisDataService.CreateAsync(dataModel);

        return PaisBusinessMapper.ToResponse(creado);
    }

    public async Task<PaisResponse> GetByIdAsync(int id)
    {
        var model = await _paisDataService.GetByIdAsync(id);
        if (model == null)
            throw new NotFoundException("Pais", id);

        return PaisBusinessMapper.ToResponse(model);
    }

    public async Task<IEnumerable<PaisResponse>> GetAllAsync()
    {
        var todos = await _paisDataService.GetAllAsync();
        return todos.Select(PaisBusinessMapper.ToResponse);
    }

    public async Task<IEnumerable<PaisResponse>> FiltrarAsync(PaisFiltroRequest request)
    {
        var filtro = new PaisFiltroDataModel
        {
            Nombre = request.Nombre,
            CodigoIso2 = request.CodigoIso2,
            CodigoIso3 = request.CodigoIso3,
            Continente = request.Continente,
            Estado = request.Estado,
            Page = request.Page,
            PageSize = request.PageSize
        };

        var resultado = await _paisDataService.GetPagedAsync(filtro);
        return resultado.Data.Select(PaisBusinessMapper.ToResponse);
    }

    public async Task<PaisResponse> ActualizarAsync(int id, ActualizarPaisRequest request)
    {
        PaisValidator.ValidarActualizar(request);

        var model = await _paisDataService.GetByIdAsync(id);
        if (model == null)
            throw new NotFoundException("Pais", id);

        if (!string.IsNullOrWhiteSpace(request.CodigoIso2))
            model.CodigoIso2 = request.CodigoIso2.ToUpper();
        if (!string.IsNullOrWhiteSpace(request.CodigoIso3))
            model.CodigoIso3 = request.CodigoIso3.ToUpper();
        if (!string.IsNullOrWhiteSpace(request.Nombre))
            model.Nombre = request.Nombre.Trim();
        if (!string.IsNullOrWhiteSpace(request.Continente))
            model.Continente = request.Continente.Trim();
        if (!string.IsNullOrWhiteSpace(request.Estado))
            model.Estado = request.Estado.ToUpper();

        await _paisDataService.UpdateAsync(model);

        return PaisBusinessMapper.ToResponse(model);
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var model = await _paisDataService.GetByIdAsync(id);
        if (model == null)
            throw new NotFoundException("Pais", id);

        await _paisDataService.DeleteAsync(id);

        return true;
    }
}