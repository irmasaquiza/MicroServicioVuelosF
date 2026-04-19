// ============================================================
// Services/UsuarioAppService.cs
// ============================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.UsuarioApp;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioAppDataService _usuarioDataService;

        public UsuarioAppService(IUsuarioAppDataService usuarioDataService)
        {
            _usuarioDataService = usuarioDataService;
        }

        // ============================================================
        // CREAR
        // ============================================================
        public async Task<UsuarioAppResponse> CrearAsync(CrearUsuarioAppRequest request)
        {
            UsuarioAppValidator.ValidarCrear(request);

            var existenteUser = await _usuarioDataService.GetByUsernameAsync(request.Username);
            if (existenteUser != null)
                throw new BusinessException("USERNAME_DUPLICADO",
                    $"Ya existe un usuario con el username '{request.Username}'.");

            var existenteCorreo = await _usuarioDataService.GetByCorreoAsync(request.Correo);
            if (existenteCorreo != null)
                throw new BusinessException("CORREO_DUPLICADO",
                    $"Ya existe un usuario con el correo '{request.Correo}'.");

            var dataModel = UsuarioAppBusinessMapper.ToDataModel(request);

            var creado = await _usuarioDataService.CreateAsync(dataModel);

            return UsuarioAppBusinessMapper.ToResponse(creado);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<UsuarioAppResponse> GetByIdAsync(int id)
        {
            var model = await _usuarioDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("UsuarioApp", id);

            return UsuarioAppBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // GET BY USERNAME
        // ============================================================
        public async Task<UsuarioAppResponse> GetByUsernameAsync(string username)
        {
            var model = await _usuarioDataService.GetByUsernameAsync(username);
            if (model == null)
                throw new NotFoundException("UsuarioApp", username);

            return UsuarioAppBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // LISTAR
        // ============================================================
        public async Task<IEnumerable<UsuarioAppResponse>> GetAllAsync()
        {
            var todos = await _usuarioDataService.GetAllAsync();
            return todos.Select(UsuarioAppBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRAR
        // ============================================================
        public async Task<IEnumerable<UsuarioAppResponse>> FiltrarAsync(
            UsuarioAppFiltroRequest request)
        {
            var filtro = new UsuarioAppFiltroDataModel
            {
                IdCliente = request.IdCliente,
                Username = request.Username,
                Correo = request.Correo,
                EstadoUsuario = request.EstadoUsuario,
                Activo = request.Activo,
                FechaUltimoLoginInicio = request.UltimoLoginInicio,
                FechaUltimoLoginFin = request.UltimoLoginFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _usuarioDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(UsuarioAppBusinessMapper.ToResponse);
        }

        // ============================================================
        // ACTUALIZAR
        // ============================================================
        public async Task<UsuarioAppResponse> ActualizarAsync(
            int id, ActualizarUsuarioAppRequest request)
        {
            UsuarioAppValidator.ValidarActualizar(request);

            var model = await _usuarioDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("UsuarioApp", id);

            // 🔥 Validar duplicados en actualización
            if (!string.IsNullOrWhiteSpace(request.Username))
            {
                var existe = await _usuarioDataService.GetByUsernameAsync(request.Username);
                if (existe != null && existe.IdUsuario != id)
                    throw new BusinessException("USERNAME_DUPLICADO",
                        "El username ya está en uso.");

                model.Username = request.Username.Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.Correo))
            {
                var existe = await _usuarioDataService.GetByCorreoAsync(request.Correo);
                if (existe != null && existe.IdUsuario != id)
                    throw new BusinessException("CORREO_DUPLICADO",
                        "El correo ya está en uso.");

                model.Correo = request.Correo.ToLower().Trim();
            }

            if (!string.IsNullOrWhiteSpace(request.EstadoUsuario))
                model.EstadoUsuario = request.EstadoUsuario.ToUpper();

            if (request.Activo.HasValue)
                model.Activo = request.Activo.Value;

            await _usuarioDataService.UpdateAsync(model);

            return UsuarioAppBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var estadosValidos = new[] { "ACT", "INA" };

            if (!estadosValidos.Contains(estado?.ToUpper()))
                throw new ValidationException("estado",
                    "El estado debe ser ACT o INA.");

            var model = await _usuarioDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("UsuarioApp", id);

            model.EstadoUsuario = estado.ToUpper();
            model.Activo = estado.ToUpper() == "ACT";

            await _usuarioDataService.UpdateAsync(model);
            return true;
        }

        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _usuarioDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("UsuarioApp", id);

            await _usuarioDataService.DeleteAsync(id);
            return true;
        }
    }
}