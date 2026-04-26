using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Equipaje;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.Business.Policies; // 🔥 IMPORTANTE
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.Business.Services
{
    public class EquipajeService : IEquipajeService
    {
        private readonly IEquipajeDataService _equipajeDataService;
        private readonly IBoletoDataService _boletoDataService;
        private readonly IFacturaDataService _facturaDataService; // 🔥 NUEVO
        private readonly IAuditoriaLogService _auditoria;

        public EquipajeService(
            IEquipajeDataService equipajeDataService,
            IBoletoDataService boletoDataService,
            IFacturaDataService facturaDataService, // 🔥 NUEVO
            IAuditoriaLogService auditoria)
        {
            _equipajeDataService = equipajeDataService;
            _boletoDataService = boletoDataService;
            _facturaDataService = facturaDataService; // 🔥 NUEVO
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

            // ============================================================
            // 🔥 VALIDAR FACTURA
            // ============================================================
            var factura = await _facturaDataService.GetByIdAsync(boleto.IdFactura);

            if (factura == null)
                throw new BusinessException("FACTURA_NO_ENCONTRADA",
                    "El boleto no tiene factura asociada.");

            if (factura.Estado == "APR")
                throw new BusinessException("FACTURA_APROBADA",
                    "No se puede agregar equipaje a una factura aprobada.");

            if (factura.Estado != "ABI")
                throw new BusinessException("FACTURA_NO_EDITABLE",
                    "Solo se puede agregar equipaje a una factura ABI.");

            var dataModel = EquipajeBusinessMapper.ToDataModel(request);

            // ============================================================
            // 🔥 CALCULAR PRECIO DESDE BACKEND
            // ============================================================
            dataModel.PrecioExtra = EquipajePricingPolicy.CalcularPrecio(
                request.Tipo,
                request.PesoKg
            );

            // ============================================================
            // 🔥 GENERAR ETIQUETA
            // ============================================================
            dataModel.NumeroEtiqueta = $"EQ-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            if (string.IsNullOrWhiteSpace(dataModel.EstadoEquipaje))
                dataModel.EstadoEquipaje = "REGISTRADO";

            var creado = await _equipajeDataService.CreateAsync(dataModel);

            // ============================================================
            // 🔥 RECALCULAR TOTAL DEL BOLETO
            // ============================================================
            await RecalcularCargoEquipaje(request.IdBoleto);

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
                    "No se puede modificar un equipaje ya entregado.");

            model.EstadoEquipaje = estado.ToUpper();
            await _equipajeDataService.UpdateAsync(model);

            // 🔥 recalcular
            await RecalcularCargoEquipaje(model.IdBoleto);

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
                    "No se puede eliminar un equipaje en tránsito.");

            await _equipajeDataService.DeleteAsync(id);

            // 🔥 recalcular
            await RecalcularCargoEquipaje(model.IdBoleto);

            return true;
        }

        // ============================================================
        // 🔥 RECALCULAR CARGO EQUIPAJE
        // ============================================================
        private async Task RecalcularCargoEquipaje(int idBoleto)
        {
            var totalEquipaje = await _equipajeDataService.SumPrecioByBoletoAsync(idBoleto);

            var boleto = await _boletoDataService.GetByIdAsync(idBoleto);
            if (boleto == null)
                throw new NotFoundException("Boleto", idBoleto);

            // 🔥 actualizar equipaje
            boleto.CargoEquipaje = totalEquipaje;

            // ============================================================
            // 🔥 RECALCULAR BOLETO COMPLETO
            // ============================================================

            decimal subtotal =
                boleto.PrecioVueloBase +
                (boleto.PrecioAsientoExtra ?? 0) +
                boleto.CargoEquipaje;

            decimal iva = subtotal * 0.15m;
            decimal total = subtotal + iva;

            boleto.ImpuestosBoleto = iva;
            boleto.PrecioFinal = total;

            await _boletoDataService.UpdateAsync(boleto);

            // ============================================================
            // 🔥 RECALCULAR FACTURA
            // ============================================================

            var factura = await _facturaDataService.GetByIdAsync(boleto.IdFactura);

            if (factura != null)
            {
                factura.Subtotal = subtotal;
                factura.ValorIva = iva;
                factura.Total = subtotal + iva + factura.CargoServicio;

                await _facturaDataService.UpdateAsync(factura);
            }
        }
    }
}