using System;
using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.DTOs.Internal.AuditoriaLog;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Exceptions;

using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class AuditoriaLogService : IAuditoriaLogService
    {
        private readonly IAuditoriaLogDataService _dataService;

        public AuditoriaLogService(IAuditoriaLogDataService dataService)
        {
            _dataService = dataService;
        }

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        public async Task<AuditoriaLogResponse> GetByIdAsync(long id)
        {
            var data = await _dataService.GetByIdAsync(id);

            if (data == null)
                throw new BusinessException("AUDITORIA_NO_ENCONTRADA", "Registro de auditoría no encontrado");

            return AuditoriaLogBusinessMapper.ToResponse(data);
        }

        // ============================================================
        // 🔥 LISTAR TODOS
        // ============================================================
        public async Task<IEnumerable<AuditoriaLogResponse>> GetAllAsync()
        {
            var data = await _dataService.GetAllAsync();

            return AuditoriaLogBusinessMapper.ToResponseList(data);
        }

        // ============================================================
        // 🔥 FILTRAR (PAGINADO)
        // ============================================================
        public async Task<IEnumerable<AuditoriaLogResponse>> FiltrarAsync(AuditoriaLogFiltroRequest request)
        {
            if (request == null)
                throw new BusinessException("FILTRO_INVALIDO", "El filtro no puede ser nulo");

            var filtro = new AuditoriaLogFiltroDataModel
            {
                TablaAfectada = request.TablaAfectada,
                Operacion = request.Operacion,
                UsuarioEjecutor = request.UsuarioEjecutor,
                FechaInicio = request.FechaDesde,
                FechaFin = request.FechaHasta,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var result = await _dataService.GetPagedAsync(filtro);

            return AuditoriaLogBusinessMapper.ToResponseList(result.Data);
        }
    }
}