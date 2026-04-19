
// ============================================================
// Services/RolService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Rol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class RolService : IRolService
    {
        private readonly IRolDataService _rolDataService;
        private readonly IAuditoriaLogService _auditoria;

        public RolService(
            IRolDataService rolDataService,
            IAuditoriaLogService auditoria)
        {
            _rolDataService = rolDataService;
            _auditoria = auditoria;
        }

        public async Task<RolResponse> CrearAsync(CrearRolRequest request)
        {
            RolValidator.ValidarCrear(request);

            var existente = await _rolDataService.GetByNombreAsync(request.NombreRol);
            if (existente != null)
                throw new BusinessException("ROL_DUPLICADO",
                    $"Ya existe un rol con el nombre '{request.NombreRol}'.");

            var dataModel = RolBusinessMapper.ToDataModel(request);
            var creado = await _rolDataService.CreateAsync(dataModel);

            return RolBusinessMapper.ToResponse(creado);
        }

        public async Task<RolResponse> GetByIdAsync(int id)
        {
            var model = await _rolDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Rol", id);

            return RolBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<RolResponse>> GetAllAsync()
        {
            var todos = await _rolDataService.GetAllAsync();
            return todos.Select(RolBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<RolResponse>> FiltrarAsync(RolFiltroRequest request)
        {
            var filtro = new RolFiltroDataModel
            {
                NombreRol = request.NombreRol,
                EstadoRol = request.EstadoRol,
                Activo = request.Activo,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _rolDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(RolBusinessMapper.ToResponse);
        }

        public async Task<RolResponse> ActualizarAsync(int id, ActualizarRolRequest request)
        {
            RolValidator.ValidarActualizar(request);

            var model = await _rolDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Rol", id);

            if (!string.IsNullOrWhiteSpace(request.NombreRol))
                model.NombreRol = request.NombreRol.ToUpper().Trim();
            if (!string.IsNullOrWhiteSpace(request.DescripcionRol))
                model.DescripcionRol = request.DescripcionRol.Trim();
            if (!string.IsNullOrWhiteSpace(request.EstadoRol))
                model.EstadoRol = request.EstadoRol.ToUpper();
            if (request.Activo.HasValue)
                model.Activo = request.Activo.Value;

            await _rolDataService.UpdateAsync(model);
            return RolBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _rolDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Rol", id);

            await _rolDataService.DeleteAsync(id);
            return true;
        }
    }
}

