// ============================================================
// Services/EscalaService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Escala;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services
{
    public class EscalaService : IEscalaService
    {
        private readonly IEscalaDataService _escalaDataService;
        private readonly IVueloDataService _vueloDataService;
        private readonly IAuditoriaLogService _auditoria;

        public EscalaService(
            IEscalaDataService escalaDataService,
            IVueloDataService vueloDataService,
            IAuditoriaLogService auditoria)
        {
            _escalaDataService = escalaDataService;
            _vueloDataService = vueloDataService;
            _auditoria = auditoria;
        }

        public async Task<EscalaResponse> CrearAsync(int idVuelo, CrearEscalaRequest request)
        {
            EscalaValidator.ValidarCrear(request);

            var vuelo = await _vueloDataService.GetByIdAsync(idVuelo);
            if (vuelo == null)
                throw new NotFoundException("Vuelo", idVuelo);

            if (vuelo.EstadoVuelo == "CANCELADO")
                throw new BusinessException("VUELO_CANCELADO",
                    "No se pueden agregar escalas a un vuelo cancelado.");

            var dataModel = EscalaBusinessMapper.ToDataModel(request);
            dataModel.IdVuelo = idVuelo;

            var creada = await _escalaDataService.CreateAsync(dataModel);
            return EscalaBusinessMapper.ToResponse(creada);
        }

        public async Task<EscalaResponse> GetByIdAsync(int id)
        {
            var model = await _escalaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Escala", id);

            return EscalaBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<EscalaResponse>> GetByVueloAsync(int idVuelo)
        {
            var vuelo = await _vueloDataService.GetByIdAsync(idVuelo);
            if (vuelo == null)
                throw new NotFoundException("Vuelo", idVuelo);

            var escalas = await _escalaDataService.GetByVueloAsync(idVuelo);
            return escalas.Select(EscalaBusinessMapper.ToResponse);
        }

        public async Task<EscalaResponse> ActualizarAsync(int id, ActualizarEscalaRequest request)
        {
            EscalaValidator.ValidarActualizar(request);

            var model = await _escalaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Escala", id);

            if (request.Orden.HasValue)
                model.Orden = request.Orden.Value;
            if (request.FechaHoraLlegada.HasValue)
                model.FechaHoraLlegada = request.FechaHoraLlegada.Value;
            if (request.FechaHoraSalida.HasValue)
                model.FechaHoraSalida = request.FechaHoraSalida.Value;
            if (request.DuracionMin.HasValue)
                model.DuracionMin = request.DuracionMin.Value;
            if (!string.IsNullOrWhiteSpace(request.TipoEscala))
                model.TipoEscala = request.TipoEscala.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.Terminal))
                model.Terminal = request.Terminal;
            if (!string.IsNullOrWhiteSpace(request.Puerta))
                model.Puerta = request.Puerta;
            if (!string.IsNullOrWhiteSpace(request.Observaciones))
                model.Observaciones = request.Observaciones;

            await _escalaDataService.UpdateAsync(model);
            return EscalaBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _escalaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Escala", id);

            await _escalaDataService.DeleteAsync(id);
            return true;
        }
    }
}