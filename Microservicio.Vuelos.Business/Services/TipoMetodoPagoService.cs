
// ============================================================
// Services/TipoMetodoPagoService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.TipoMetodoPago;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class TipoMetodoPagoService : ITipoMetodoPagoService
    {
        private readonly ITipoMetodoPagoDataService _tipoDataService;

        public TipoMetodoPagoService(ITipoMetodoPagoDataService tipoDataService)
        {
            _tipoDataService = tipoDataService;
        }

        public async Task<TipoMetodoPagoResponse> CrearAsync(
            CrearTipoMetodoPagoRequest request)
        {
            TipoMetodoPagoValidator.ValidarCrear(request);

            var existente = await _tipoDataService.GetByNombreAsync(request.NombreTipo);
            if (existente != null)
                throw new BusinessException("TIPO_METODO_DUPLICADO",
                    $"Ya existe un tipo de método con el nombre '{request.NombreTipo}'.");

            var dataModel = TipoMetodoPagoBusinessMapper.ToDataModel(request);
            var creado = await _tipoDataService.CreateAsync(dataModel);

            return TipoMetodoPagoBusinessMapper.ToResponse(creado);
        }

        public async Task<TipoMetodoPagoResponse> GetByIdAsync(int id)
        {
            var model = await _tipoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("TipoMetodoPago", id);

            return TipoMetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<TipoMetodoPagoResponse>> GetAllAsync()
        {
            var todos = await _tipoDataService.GetAllAsync();
            return todos.Select(TipoMetodoPagoBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<TipoMetodoPagoResponse>> FiltrarAsync(
            TipoMetodoPagoFiltroRequest request)
        {
            var filtro = new TipoMetodoPagoFiltroDataModel
            {
                NombreTipo = request.NombreTipo,
                Estado = request.Estado,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _tipoDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(TipoMetodoPagoBusinessMapper.ToResponse);
        }

        public async Task<TipoMetodoPagoResponse> ActualizarAsync(
            int id, ActualizarTipoMetodoPagoRequest request)
        {
            TipoMetodoPagoValidator.ValidarActualizar(request);

            var model = await _tipoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("TipoMetodoPago", id);

            if (!string.IsNullOrWhiteSpace(request.NombreTipo))
                model.NombreTipo = request.NombreTipo.ToUpper().Trim();
            if (!string.IsNullOrWhiteSpace(request.Descripcion))
                model.Descripcion = request.Descripcion.Trim();
            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado.ToUpper();

            await _tipoDataService.UpdateAsync(model);
            return TipoMetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var estadosValidos = new[] { "ACTIVO", "INACTIVO" };
            if (!estadosValidos.Contains(estado?.ToUpper()))
                throw new ValidationException("estado",
                    "El estado debe ser ACTIVO o INACTIVO.");

            var model = await _tipoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("TipoMetodoPago", id);

            model.Estado = estado.ToUpper();
            await _tipoDataService.UpdateAsync(model);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _tipoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("TipoMetodoPago", id);

            await _tipoDataService.DeleteAsync(id);
            return true;
        }
    }
}