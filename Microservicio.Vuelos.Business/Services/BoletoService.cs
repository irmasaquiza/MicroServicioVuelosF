// ============================================================
// Services/BoletoService.cs
// ============================================================

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Boleto;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
 
namespace Microservicio.Vuelos.Business.Services
{
    public class BoletoService : IBoletoService
    {
        private readonly IBoletoDataService _boletoDataService;
        private readonly IReservaDataService _reservaDataService;
        private readonly IVueloDataService _vueloDataService;
        private readonly IAsientoDataService _asientoDataService;
        private readonly IFacturaDataService _facturaDataService; // 🔥 1. NUEVA DEPENDENCIA
        private readonly IUsuarioAppDataService _usuarioDataService;
        public BoletoService(
            IBoletoDataService boletoDataService,
            IReservaDataService reservaDataService,
            IVueloDataService vueloDataService,
            IAsientoDataService asientoDataService,
            IFacturaDataService facturaDataService,
            IUsuarioAppDataService usuarioDataService // 👈 CAMBIO

            ) // 🔥 1. NUEVO PARÁMETRO
        {
            _boletoDataService = boletoDataService;
            _reservaDataService = reservaDataService;
            _vueloDataService = vueloDataService;
            _asientoDataService = asientoDataService;
            _facturaDataService = facturaDataService; // 🔥 1. ASIGNACIÓN
            _usuarioDataService = usuarioDataService; // 👈 NUEVO

        }

        // ============================================================
        // CREAR BOLETO
        // ============================================================
        public async Task<BoletoResponse> CrearAsync(CrearBoletoRequest request)
        {
            BoletoValidator.ValidarCrear(request);

            // ============================================================
            // 🔥 Validar reserva
            // ============================================================
            var reserva = await _reservaDataService.GetByIdAsync(request.IdReserva);
            if (reserva == null)
                throw new BusinessException("RESERVA_NO_ENCONTRADA",
                    $"No existe la reserva '{request.IdReserva}'.");

            // ============================================================
            // 🔥 Validar vuelo
            // ============================================================
            var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);
            if (vuelo == null)
                throw new BusinessException("VUELO_NO_ENCONTRADO",
                    $"No existe el vuelo '{request.IdVuelo}'.");

            // ============================================================
            // 🔥 ASIENTO OPCIONAL
            // ============================================================
            AsientoDataModel? asiento = null;
            decimal precioExtra = 0;

            if (request.IdAsiento.HasValue)
            {
                asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento.Value);

                if (asiento == null)
                    throw new BusinessException("ASIENTO_NO_ENCONTRADO",
                        $"No existe el asiento '{request.IdAsiento}'.");

                if (!asiento.Disponible)
                    throw new BusinessException("ASIENTO_OCUPADO",
                        $"El asiento '{asiento.NumeroAsiento}' ya está ocupado.");

                if (asiento.IdVuelo != request.IdVuelo)
                    throw new BusinessException("ASIENTO_INVALIDO",
                        "El asiento no pertenece al vuelo.");

                // ✔ solo si hay asiento
                precioExtra = asiento.PrecioExtra;
            }

            // ============================================================
            // 🔥 VALIDAR FACTURA (SOLO ABI)
            // ============================================================
            var factura = await _facturaDataService.GetByIdAsync(request.IdFactura);

            if (factura == null)
                throw new BusinessException("FACTURA_NO_ENCONTRADA",
                    $"No existe la factura '{request.IdFactura}'.");

            if (factura.IdReserva != request.IdReserva)
                throw new BusinessException("FACTURA_RESERVA_INVALIDA",
                    "La factura no pertenece a la reserva.");

            // ⚠️ usa Estado (no EstadoFactura)
            if (factura.Estado != "ABI")
                throw new BusinessException("FACTURA_INVALIDA",
                    "Solo se puede emitir boleto con factura ABI.");

            // ============================================================
            // 🔥 Generar código
            // ============================================================
            var codigo = $"BT-{System.DateTime.UtcNow:yyyyMMdd}-{System.Guid.NewGuid().ToString("N")[..6].ToUpper()}";

            // ============================================================
            // 🔥 Calcular precios (BACKEND MANDA)
            // ============================================================
            // ============================================================
            // 🔥 NUEVA LÓGICA CORRECTA (SUBTOTAL + IVA 15% + EXTRAS)
            // ============================================================

            // 🔹 base real viene de la factura
            decimal subtotal = factura.Subtotal;

            // 🔹 extra asiento (si existe)
            decimal precioAsiento = asiento?.PrecioExtra ?? 0;

            // 🔹 equipaje inicia en 0
            decimal cargoEquipaje = 0;

            // 🔹 IVA 15%
            decimal subtotalCompleto = subtotal + precioAsiento + cargoEquipaje;
            decimal iva = subtotalCompleto * 0.15m;
            decimal total = subtotalCompleto + iva;

            // ============================================================
            // 🔥 Crear modelo
            // ============================================================
            var dataModel = BoletoBusinessMapper.ToDataModel(request);

            dataModel.CodigoBoleto = codigo;
            dataModel.PrecioVueloBase = subtotal;
            dataModel.PrecioAsientoExtra = precioAsiento;
            dataModel.ImpuestosBoleto = iva;
            dataModel.CargoEquipaje = cargoEquipaje;
            dataModel.PrecioFinal = total;

            var creado = await _boletoDataService.CreateAsync(dataModel);

            // ============================================================
            // 🔥 CAMBIAR RESERVA A EMI
            // ============================================================
            if (reserva.EstadoReserva == "CON")
            {
                reserva.EstadoReserva = "EMI";
                await _reservaDataService.UpdateAsync(reserva);
            }

            // ============================================================
            // 🔥 Marcar asiento ocupado SOLO SI EXISTE
            // ============================================================
            if (asiento != null)
            {
                asiento.Disponible = false;
                await _asientoDataService.UpdateAsync(asiento);
            }

            return BoletoBusinessMapper.ToResponse(creado);
        }
        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<BoletoResponse> GetByIdAsync(int id)
        {
            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            return BoletoBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // POR RESERVA
        // ============================================================
        public async Task<IEnumerable<BoletoResponse>> GetByReservaAsync(int idReserva)
        {
            var boletos = await _boletoDataService.GetByReservaAsync(idReserva);
            return boletos.Select(BoletoBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRAR
        // ============================================================
        public async Task<IEnumerable<BoletoResponse>> FiltrarAsync(BoletoFiltroRequest request)
        {
            var filtro = new BoletoFiltroDataModel
            {
                IdReserva = request.IdReserva,
                IdVuelo = request.IdVuelo,
                IdAsiento = request.IdAsiento,
                IdFactura = request.IdFactura,
                CodigoBoleto = request.CodigoBoleto,
                Clase = request.Clase,
                EstadoBoleto = request.EstadoBoleto,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _boletoDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(BoletoBusinessMapper.ToResponse);
        }

        // ============================================================
        // ACTUALIZAR
        // ============================================================
        public async Task<BoletoResponse> ActualizarAsync(int id, ActualizarBoletoRequest request)
        {
            BoletoValidator.ValidarActualizar(request);

            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            if (model.EstadoBoleto == "USADO")
                throw new BusinessException("BOLETO_USADO",
                    "No se puede modificar un boleto usado.");

            if (model.EstadoBoleto == "CANCELADO")
                throw new BusinessException("BOLETO_CANCELADO",
                    "No se puede modificar un boleto cancelado.");

            if (!string.IsNullOrWhiteSpace(request.Clase))
                model.Clase = request.Clase.ToUpper();

            await _boletoDataService.UpdateAsync(model);

            return BoletoBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, string estado)
        {
            var estadosValidos = new[] { "ACTIVO", "USADO", "CANCELADO" };

            if (!estadosValidos.Contains(estado?.ToUpper()))
                throw new ValidationException("estado",
                    "El estado debe ser ACTIVO, USADO o CANCELADO.");

            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            if (model.EstadoBoleto == "USADO")
                throw new BusinessException("BOLETO_USADO",
                    "No se puede cambiar estado de un boleto usado.");

            model.EstadoBoleto = estado.ToUpper();
            await _boletoDataService.UpdateAsync(model);

            return true;
        }


        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _boletoDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Boleto", id);

            if (model.EstadoBoleto == "USADO")
                throw new BusinessException("BOLETO_USADO",
                    "No se puede eliminar un boleto usado.");

            await _boletoDataService.DeleteAsync(id);
            return true;
        }


        // ============================================================
        // 🔥 BOLETOS DEL USUARIO (MIS BOLETOS)
        // ============================================================
        public async Task<IEnumerable<BoletoResponse>> GetByUsuarioAsync(int idUsuario)
        {
            // 🔥 1. obtener usuario
            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario);

            if (usuario == null)
                throw new BusinessException("USUARIO_NO_ENCONTRADO");

            if (!usuario.IdCliente.HasValue)
                throw new BusinessException("USUARIO_SIN_CLIENTE");

            var idCliente = usuario.IdCliente.Value;

            // 🔥 2. obtener reservas del cliente
            var reservas = await _reservaDataService.GetByClienteAsync(idCliente);

            var idsReservas = reservas.Select(r => r.IdReserva).ToList();

            // 🔥 3. obtener boletos
            var boletos = await _boletoDataService.GetAllAsync();

            // 🔥 4. filtrar por reservas del cliente
            var filtrados = boletos
                .Where(b => idsReservas.Contains(b.IdReserva))
                .ToList();

            // 🔥 5. mapear
            return filtrados.Select(BoletoBusinessMapper.ToResponse);
        }
    }
}