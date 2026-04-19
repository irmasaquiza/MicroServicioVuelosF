
// ============================================================
// Services/ClienteService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Cliente;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services
{
    public class ClienteService : IClienteService
    {
        private readonly IClienteDataService _clienteDataService;

        public ClienteService(IClienteDataService clienteDataService)
        {
            _clienteDataService = clienteDataService;
        }

        public async Task<ClienteResponse> CrearAsync(CrearClienteRequest request)
        {
            ClienteValidator.ValidarCrear(request);

            var existenteCorreo = await _clienteDataService.GetByCorreoAsync(request.Correo);
            if (existenteCorreo != null)
                throw new BusinessException("CORREO_DUPLICADO",
                    $"Ya existe un cliente con el correo '{request.Correo}'.");

            var existenteDoc = await _clienteDataService
                .GetByIdentificacionAsync(request.NumeroIdentificacion);
            if (existenteDoc != null)
                throw new BusinessException("IDENTIFICACION_DUPLICADA",
                    $"Ya existe un cliente con la identificación '{request.NumeroIdentificacion}'.");

            var dataModel = ClienteBusinessMapper.ToDataModel(request);
            var creado = await _clienteDataService.CreateAsync(dataModel);

            return ClienteBusinessMapper.ToResponse(creado);
        }

        public async Task<ClienteResponse> GetByIdAsync(int id)
        {
            var model = await _clienteDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Cliente", id);

            return ClienteBusinessMapper.ToResponse(model);
        }

        public async Task<ClienteResponse> GetByIdentificacionAsync(string numeroIdentificacion)
        {
            var model = await _clienteDataService.GetByIdentificacionAsync(numeroIdentificacion);
            if (model == null)
                throw new NotFoundException("Cliente", numeroIdentificacion);

            return ClienteBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<ClienteResponse>> GetAllAsync()
        {
            var todos = await _clienteDataService.GetAllAsync();
            return todos.Select(ClienteBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<ClienteResponse>> FiltrarAsync(ClienteFiltroRequest request)
        {
            var filtro = new Microservicio.Vuelos.DataManagement.Models.ClienteFiltroDataModel
            {
                TipoIdentificacion = request.TipoIdentificacion,
                NumeroIdentificacion = request.NumeroIdentificacion,
                Nombres = request.Nombres,
                Apellidos = request.Apellidos,
                Correo = request.Correo,
                IdCiudadResidencia = request.IdCiudadResidencia,
                IdPaisNacionalidad = request.IdPaisNacionalidad,
                Estado = request.Estado,
                FechaNacimientoInicio = request.FechaNacimientoInicio,
                FechaNacimientoFin = request.FechaNacimientoFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _clienteDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(ClienteBusinessMapper.ToResponse);
        }

        public async Task<ClienteResponse> ActualizarAsync(int id, ActualizarClienteRequest request)
        {
            ClienteValidator.ValidarActualizar(request);

            var model = await _clienteDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Cliente", id);

            if (model.Estado == "INA")
                throw new BusinessException("CLIENTE_INACTIVO",
                    "No se puede modificar un cliente inactivo.");

            if (!string.IsNullOrWhiteSpace(request.Nombres))
                model.Nombres = request.Nombres.Trim();
            if (!string.IsNullOrWhiteSpace(request.Apellidos))
                model.Apellidos = request.Apellidos.Trim();
            if (!string.IsNullOrWhiteSpace(request.RazonSocial))
                model.RazonSocial = request.RazonSocial.Trim();
            if (!string.IsNullOrWhiteSpace(request.Correo))
                model.Correo = request.Correo.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(request.Telefono))
                model.Telefono = request.Telefono.Trim();
            if (!string.IsNullOrWhiteSpace(request.Direccion))
                model.Direccion = request.Direccion.Trim();
            if (request.IdCiudadResidencia.HasValue)
                model.IdCiudadResidencia = request.IdCiudadResidencia.Value;
            if (request.IdPaisNacionalidad.HasValue)
                model.IdPaisNacionalidad = request.IdPaisNacionalidad.Value;
            if (request.FechaNacimiento.HasValue)
                model.FechaNacimiento = request.FechaNacimiento;
            if (!string.IsNullOrWhiteSpace(request.Nacionalidad))
                model.Nacionalidad = request.Nacionalidad.Trim();
            if (!string.IsNullOrWhiteSpace(request.Genero))
                model.Genero = request.Genero.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado.ToUpper();

            await _clienteDataService.UpdateAsync(model);

            return ClienteBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _clienteDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Cliente", id);

            await _clienteDataService.DeleteAsync(id);

            return true;
        }
    }
}

