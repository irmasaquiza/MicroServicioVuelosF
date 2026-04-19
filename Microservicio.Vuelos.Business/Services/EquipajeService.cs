
// ============================================================
// Services/EquipajeService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services
{
    public class EquipajeService : IEquipajeService
    {
        private readonly IEquipajeDataService _equipajeDataService;
        private readonly IBoletoDataService _boletoDataService;
        private readonly IAuditoriaLogService _auditoria;

        public EquipajeService(
            IEquipajeDataService equipajeDataService,
            IBoletoDataService boletoDataService,
            IAuditoriaLogService auditoria)
        {
            _equipajeDataService = equipajeDataService;
            _boletoDataService = boletoDataService;
            _auditoria = auditoria;
        }

        public async Task<EquipajeResponse> CrearAsync(CrearEquipajeRequest request)
        {
            EquipajeValidator.ValidarCrear(request);

            var boleto = await _boletoDataService.GetByIdAsync(request.IdBoleto);
            if (boleto == null)
                throw new NotFoundException("Boleto", request.IdBoleto);

            if (boleto.EstadoBoleto == "CANCELADO")
                throw new BusinessException("BOLETO_CANCELADO",
                    "No se puede registrar equipaje en un boleto cancelado.");

            var dataModel = EquipajeBusinessMapper.ToDataModel(request);
            var creado = await _equipajeDataService.CreateAsync(dataModel);

            return EquipajeBusinessMapper.ToResponse(creado);
        }

        public async Task<EquipajeResponse> GetByIdAsync(int id)
        {
            var model = await _equipajeDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Equipaje", id);

            return EquipajeBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<EquipajeResponse>> GetByBoletoAsync(int idBoleto)
        {
            var boleto = await _boletoDataService.GetByIdAsync(idBoleto);
            if (boleto == null)
                throw new NotFoundException("Boleto", idBoleto);

            var equipajes = await _equipajeDataService.GetByBoletoAsync(idBoleto);
            return equipajes.Select(EquipajeBusinessMapper.ToResponse);
        }

        public async Task<bool> CambiarEstadoAsync(int idEquipaje, string estado)
        {
            EquipajeValidator.ValidarActualizar(
                new ActualizarEquipajeRequest { EstadoEquipaje = estado });

            var model = await _equipajeDataService.GetByIdAsync(idEquipaje);
            if (model == null)
                throw new NotFoundException("Equipaje", idEquipaje);

            if (model.EstadoEquipaje == "ENTREGADO")
                throw new BusinessException("EQUIPAJE_ENTREGADO",
                    "No se puede modificar el estado de un equipaje ya entregado.");

            model.EstadoEquipaje = estado.ToUpper();
            await _equipajeDataService.UpdateAsync(model);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _equipajeDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Equipaje", id);

            if (model.EstadoEquipaje == "EMBARCADO" ||
                model.EstadoEquipaje == "EN_TRANSITO")
                throw new BusinessException("EQUIPAJE_EN_TRANSITO",
                    "No se puede eliminar un equipaje que está en tránsito.");

            await _equipajeDataService.DeleteAsync(id);
            return true;
        }
    }
}
