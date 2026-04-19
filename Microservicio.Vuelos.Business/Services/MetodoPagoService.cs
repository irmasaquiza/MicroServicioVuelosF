
// ============================================================
// Services/MetodoPagoService.cs
// ============================================================
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.Business.Services
{
    public class MetodoPagoService : IMetodoPagoService
    {
        private readonly IMetodoPagoDataService _metodoPagoDataService;
        private readonly IClienteDataService _clienteDataService;
        private readonly IAuditoriaLogService _auditoria;

        public MetodoPagoService(
            IMetodoPagoDataService metodoPagoDataService,
            IClienteDataService clienteDataService,
            IAuditoriaLogService auditoria)
        {
            _metodoPagoDataService = metodoPagoDataService;
            _clienteDataService = clienteDataService;
            _auditoria = auditoria;
        }

        public async Task<MetodoPagoResponse> CrearAsync(CrearMetodoPagoRequest request)
        {
            MetodoPagoValidator.ValidarCrear(request);

            var cliente = await _clienteDataService.GetByIdAsync(request.IdCliente);
            if (cliente == null)
                throw new BusinessException("CLIENTE_NO_ENCONTRADO",
                    $"No existe un cliente con ID '{request.IdCliente}'.");

            var dataModel = MetodoPagoBusinessMapper.ToDataModel(request);
            var creado = await _metodoPagoDataService.CreateAsync(dataModel);

            return MetodoPagoBusinessMapper.ToResponse(creado);
        }

        public async Task<MetodoPagoResponse> GetByIdAsync(int id)
        {
            var model = await _metodoPagoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("MetodoPago", id);

            return MetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<MetodoPagoResponse>> GetByClienteAsync(int idCliente)
        {
            var cliente = await _clienteDataService.GetByIdAsync(idCliente);
            if (cliente == null)
                throw new NotFoundException("Cliente", idCliente);

            var metodos = await _metodoPagoDataService.GetByClienteAsync(idCliente);
            return metodos.Select(MetodoPagoBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<MetodoPagoResponse>> FiltrarAsync(
            MetodoPagoFiltroRequest request)
        {
            var filtro = new MetodoPagoFiltroDataModel
            {
                IdCliente = request.IdCliente,
                IdTipoMetodo = request.IdTipoMetodo,
                MarcaTarjeta = request.MarcaTarjeta,
                BancoEmisor = request.BancoEmisor,
                EsPrincipal = request.EsPrincipal,
                Estado = request.Estado,
                FechaExpiracionInicio = request.FechaExpiracionInicio,
                FechaExpiracionFin = request.FechaExpiracionFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _metodoPagoDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(MetodoPagoBusinessMapper.ToResponse);
        }

        public async Task<MetodoPagoResponse> ActualizarAsync(
            int id, ActualizarMetodoPagoRequest request)
        {
            MetodoPagoValidator.ValidarActualizar(request);

            var model = await _metodoPagoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("MetodoPago", id);

            if (model.Estado == "BLOQUEADO")
                throw new BusinessException("METODO_BLOQUEADO",
                    "No se puede modificar un método de pago bloqueado.");

            if (!string.IsNullOrWhiteSpace(request.ReferenciaVisible))
                model.ReferenciaVisible = request.ReferenciaVisible.Trim();
            if (request.FechaExpiracion.HasValue)
                model.FechaExpiracion = request.FechaExpiracion;
            if (!string.IsNullOrWhiteSpace(request.NombreTitular))
                model.NombreTitular = request.NombreTitular.Trim();
            if (!string.IsNullOrWhiteSpace(request.MarcaTarjeta))
                model.MarcaTarjeta = request.MarcaTarjeta.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.BancoEmisor))
                model.BancoEmisor = request.BancoEmisor.Trim();
            if (!string.IsNullOrWhiteSpace(request.PaisEmision))
                model.PaisEmision = request.PaisEmision.Trim();
            if (request.EsPrincipal.HasValue)
                model.EsPrincipal = request.EsPrincipal.Value;
            if (!string.IsNullOrWhiteSpace(request.Alias))
                model.Alias = request.Alias.Trim();
            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado.ToUpper();

            await _metodoPagoDataService.UpdateAsync(model);
            return MetodoPagoBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EstablecerPrincipalAsync(int idMetodo)
        {
            var model = await _metodoPagoDataService.GetByIdAsync(idMetodo);
            if (model == null)
                throw new NotFoundException("MetodoPago", idMetodo);

            // Quitar principal de todos los del mismo cliente
            var todos = await _metodoPagoDataService.GetByClienteAsync(model.IdCliente);
            foreach (var m in todos.Where(x => x.EsPrincipal))
            {
                m.EsPrincipal = false;
                await _metodoPagoDataService.UpdateAsync(m);
            }

            model.EsPrincipal = true;
            await _metodoPagoDataService.UpdateAsync(model);
            return true;
        }

        public async Task<bool> CambiarEstadoAsync(int idMetodo, string estado)
        {
            var estadosValidos = new[] { "ACTIVO", "EXPIRADO", "BLOQUEADO" };
            if (!estadosValidos.Contains(estado?.ToUpper()))
                throw new ValidationException("estado",
                    "El estado debe ser ACTIVO, EXPIRADO o BLOQUEADO.");

            var model = await _metodoPagoDataService.GetByIdAsync(idMetodo);
            if (model == null)
                throw new NotFoundException("MetodoPago", idMetodo);

            model.Estado = estado.ToUpper();
            await _metodoPagoDataService.UpdateAsync(model);
            return true;
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _metodoPagoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("MetodoPago", id);

            await _metodoPagoDataService.DeleteAsync(id);
            return true;
        }
    }
}
