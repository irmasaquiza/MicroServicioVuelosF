
// ============================================================
// Services/UsuarioRolService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioRol;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class UsuarioRolService : IUsuarioRolService
    {
        private readonly IUsuarioRolDataService _usuarioRolDataService;
        private readonly IUsuarioAppDataService _usuarioDataService;
        private readonly IRolDataService _rolDataService;

        public UsuarioRolService(
            IUsuarioRolDataService usuarioRolDataService,
            IUsuarioAppDataService usuarioDataService,
            IRolDataService rolDataService)
        {
            _usuarioRolDataService = usuarioRolDataService;
            _usuarioDataService = usuarioDataService;
            _rolDataService = rolDataService;
        }

        public async Task<UsuarioRolResponse> CrearAsync(
            int idUsuario, CrearUsuarioRolRequest request)
        {
            UsuarioRolValidator.ValidarCrear(request);

            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario);
            if (usuario == null)
                throw new NotFoundException("UsuarioApp", idUsuario);

            var rol = await _rolDataService.GetByIdAsync(request.IdRol);
            if (rol == null)
                throw new NotFoundException("Rol", request.IdRol);

            if (!rol.Activo)
                throw new BusinessException("ROL_INACTIVO",
                    "No se puede asignar un rol inactivo.");

            var existentes = await _usuarioRolDataService.GetByUsuarioAsync(idUsuario);
            if (existentes.Any(ur => ur.IdRol == request.IdRol && ur.Activo))
                throw new BusinessException("ROL_YA_ASIGNADO",
                    $"El usuario ya tiene asignado el rol '{rol.NombreRol}'.");

            var dataModel = UsuarioRolBusinessMapper.ToDataModel(idUsuario, request);
            var creado = await _usuarioRolDataService.CreateAsync(dataModel);

            return UsuarioRolBusinessMapper.ToResponse(creado);
        }

        public async Task<UsuarioRolResponse> GetByIdAsync(int id)
        {
            var todos = await _usuarioRolDataService.GetAllAsync();
            var model = todos.FirstOrDefault(ur => ur.IdUsuarioRol == id);
            if (model == null)
                throw new NotFoundException("UsuarioRol", id);

            return UsuarioRolBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<UsuarioRolResponse>> GetByUsuarioAsync(int idUsuario)
        {
            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario);
            if (usuario == null)
                throw new NotFoundException("UsuarioApp", idUsuario);

            var lista = await _usuarioRolDataService.GetByUsuarioAsync(idUsuario);
            return lista.Select(UsuarioRolBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<UsuarioRolResponse>> GetByRolAsync(int idRol)
        {
            var lista = await _usuarioRolDataService.GetByRolAsync(idRol);
            return lista.Select(UsuarioRolBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<UsuarioRolResponse>> FiltrarAsync(
            UsuarioRolFiltroRequest request)
        {
            var filtro = new UsuarioRolFiltroDataModel
            {
                IdUsuario = request.IdUsuario,
                IdRol = request.IdRol,
                EstadoUsuarioRol = request.EstadoUsuarioRol,
                Activo = request.Activo,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _usuarioRolDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(UsuarioRolBusinessMapper.ToResponse);
        }

        public async Task<UsuarioRolResponse> ActualizarAsync(
            int id, ActualizarUsuarioRolRequest request)
        {
            UsuarioRolValidator.ValidarActualizar(request);

            var todos = await _usuarioRolDataService.GetAllAsync();
            var model = todos.FirstOrDefault(ur => ur.IdUsuarioRol == id);
            if (model == null)
                throw new NotFoundException("UsuarioRol", id);

            if (!string.IsNullOrWhiteSpace(request.EstadoUsuarioRol))
                model.EstadoUsuarioRol = request.EstadoUsuarioRol.ToUpper();
            if (request.Activo.HasValue)
                model.Activo = request.Activo.Value;

            await _usuarioRolDataService.UpdateAsync(model);
            return UsuarioRolBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var todos = await _usuarioRolDataService.GetAllAsync();
            var model = todos.FirstOrDefault(ur => ur.IdUsuarioRol == id);
            if (model == null)
                throw new NotFoundException("UsuarioRol", id);

            await _usuarioRolDataService.DeleteAsync(id);
            return true;
        }
    }
}
