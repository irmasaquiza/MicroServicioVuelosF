// ============================================================
// Services/FacturaService.cs
// ============================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Factura;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microservicio.Vuelos.Business.DTOs.Booking.Factura;

namespace Microservicio.Vuelos.Business.Services
{
    public class FacturaService : IFacturaService
    {
        private readonly IFacturaDataService _facturaDataService;
        private readonly IUsuarioAppDataService _usuarioDataService;

        public FacturaService(
            IFacturaDataService facturaDataService,
            IUsuarioAppDataService usuarioDataService
        )
        {
            _facturaDataService = facturaDataService;
            _usuarioDataService = usuarioDataService;
        }

        // ============================================================
        // CREAR
        // ============================================================
        public async Task<FacturaResponse> CrearAsync(CrearFacturaRequest request)
        {
            FacturaValidator.ValidarCrear(request);

            var numero = $"FAC-{System.DateTime.UtcNow:yyyyMMdd}-{System.Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            var dataModel = FacturaBusinessMapper.ToDataModel(request);
            dataModel.NumeroFactura = numero;

            var creada = await _facturaDataService.CreateAsync(dataModel);

            return FacturaBusinessMapper.ToResponse(creada);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<FacturaResponse> GetByIdAsync(int id)
        {
            var model = await _facturaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Factura", id);

            return FacturaBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // POR RESERVA
        // ============================================================
        public async Task<IEnumerable<FacturaResponse>> GetByReservaAsync(int idReserva)
        {
            var facturas = await _facturaDataService.GetByReservaAsync(idReserva);
            return facturas.Select(FacturaBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRAR
        // ============================================================
        public async Task<IEnumerable<FacturaResponse>> FiltrarAsync(FacturaFiltroRequest request)
        {
            var filtro = new FacturaFiltroDataModel
            {
                IdCliente = request.IdCliente,
                IdReserva = request.IdReserva,
             //   IdMetodo = request.IdMetodo,
                NumeroFactura = request.NumeroFactura,
                Estado = request.Estado,
                OrigenCanalFactura = request.OrigenCanalFactura,
                TotalMin = request.TotalMin,
                TotalMax = request.TotalMax,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _facturaDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(FacturaBusinessMapper.ToResponse);
        }

        // ============================================================
        // ACTUALIZAR
        // ============================================================
        public async Task<FacturaResponse> ActualizarAsync(int id, ActualizarFacturaRequest request)
        {
            FacturaValidator.ValidarActualizar(request);

            var model = await _facturaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Factura", id);

            if (model.Estado == "INA")
                throw new BusinessException("FACTURA_ANULADA",
                    "No se puede modificar una factura anulada.");

            if (request.Subtotal.HasValue)
                model.Subtotal = request.Subtotal.Value;
            if (request.ValorIva.HasValue)
                model.ValorIva = request.ValorIva.Value;
            if (request.CargoServicio.HasValue)
                model.CargoServicio = request.CargoServicio.Value;
            if (request.Total.HasValue)
                model.Total = request.Total.Value;
            if (!string.IsNullOrWhiteSpace(request.ObservacionesFactura))
                model.ObservacionesFactura = request.ObservacionesFactura.Trim();
            if (!string.IsNullOrWhiteSpace(request.OrigenCanalFactura))
                model.OrigenCanalFactura = request.OrigenCanalFactura.Trim();
            if (!string.IsNullOrWhiteSpace(request.Estado))
                model.Estado = request.Estado.ToUpper();

            await _facturaDataService.UpdateAsync(model);

            return FacturaBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var estadosValidos = new[] { "ABI", "APR", "INA" };

            if (!estadosValidos.Contains(estado?.ToUpper()))
                throw new ValidationException("estado",
                    "El estado debe ser ABI, APR o INA.");

            var model = await _facturaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Factura", id);

            if (model.Estado == "INA")
                throw new BusinessException("FACTURA_ANULADA",
                    "No se puede cambiar el estado de una factura anulada.");

            model.Estado = estado.ToUpper();
            await _facturaDataService.UpdateAsync(model);

            return true;
        }

        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _facturaDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Factura", id);

            if (model.Estado == "APR")
                throw new BusinessException("FACTURA_APROBADA",
                    "No se puede eliminar una factura aprobada.");

            await _facturaDataService.DeleteAsync(id);

            return true;
        }

        public async Task AprobarAsync(int idFactura, int idUsuario)
        {
            // 🔥 1. obtener factura
            var factura = await _facturaDataService.GetByIdAsync(idFactura);

            if (factura == null)
                throw new BusinessException("FACTURA_NO_ENCONTRADA");

            if (factura.Estado == "APR")
                throw new BusinessException("FACTURA_YA_APROBADA");

            // 🔥 2. obtener usuario
            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario);

            if (usuario == null)
                throw new BusinessException("USUARIO_NO_ENCONTRADO");

            // 🔥 3. VALIDACIÓN CLAVE
            if (usuario.IdCliente.HasValue)
            {
                // 👤 CLIENTE → solo su factura
                if (factura.IdCliente != usuario.IdCliente.Value)
                    throw new BusinessException("NO_AUTORIZADO");
            }
            // 👨‍💼 ADMIN → puede aprobar cualquiera

            // 🔥 4. CAMBIAR ESTADO (PAGO)
            factura.Estado = "APR";

            await _facturaDataService.UpdateAsync(factura);
        }

        // boooking 

        public async Task<FacturaBookingResponse> GetByReservaBookingAsync(int idReserva)
        {
            var facturas = await _facturaDataService.GetByReservaAsync(idReserva);

            var factura = facturas.FirstOrDefault();

            if (factura == null)
                throw new BusinessException("FACTURA_NO_ENCONTRADA");

            return new FacturaBookingResponse
            {
                IdFactura = factura.IdFactura,
                IdReserva = factura.IdReserva,
                NumeroFactura = factura.NumeroFactura,
                FechaEmision = factura.FechaEmision,
                Subtotal = factura.Subtotal,
                ValorIva = factura.ValorIva,
                Total = factura.Total,
                Estado = factura.Estado
            };
        }
        public async Task<bool> PagarBookingAsync(int idFactura)
        {
            var factura = await _facturaDataService.GetByIdAsync(idFactura);

            if (factura == null)
                throw new BusinessException("FACTURA_NO_ENCONTRADA");

            if (factura.Estado == "APR")
                throw new BusinessException("FACTURA_YA_PAGADA");

            // 🔥 simular pago exitoso
            factura.Estado = "APR";

            await _facturaDataService.UpdateAsync(factura);

            return true;
        }



    }
}