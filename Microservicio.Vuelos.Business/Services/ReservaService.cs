using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Reserva;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using Microservicio.Vuelos.Business.DTOs.Booking.Reserva;

namespace Microservicio.Vuelos.Business.Services
{
    public class ReservaService : IReservaService
    {
        private readonly IReservaDataService _reservaDataService;
        private readonly IBoletoDataService _boletoDataService;
        private readonly IFacturaDataService _facturaDataService;
        private readonly IAsientoDataService _asientoDataService;
        private readonly IVueloDataService _vueloDataService;
        private readonly IUsuarioAppDataService _usuarioDataService;
        public ReservaService(
            IReservaDataService reservaDataService,
            IBoletoDataService boletoDataService,
            IFacturaDataService facturaDataService,
            IAsientoDataService asientoDataService,
            IVueloDataService vueloDataService,
            IUsuarioAppDataService usuarioDataService)
        {
            _reservaDataService = reservaDataService;
            _boletoDataService = boletoDataService;
            _facturaDataService = facturaDataService;
            _asientoDataService = asientoDataService;
            _vueloDataService = vueloDataService;
            _usuarioDataService = usuarioDataService;
        }

        // ============================================================
        // CREAR
        // ============================================================
        public async Task<ReservaResponse> CrearAsync(CrearReservaRequest request)
        {
            ReservaValidator.ValidarCrear(request);
            var vuelo = await _vueloDataService.GetByIdAsync(request.IdVuelo);

            if (vuelo == null)
                throw new BusinessException("El vuelo no existe");

            // 👇 recién aquí puedes usarlo
            request.FechaInicio = vuelo.FechaHoraSalida;
            request.FechaFin = vuelo.FechaHoraLlegada;



            if (vuelo.EstadoVuelo == "CANCELADO")
                throw new BusinessException("VUELO_CANCELADO");

            var asiento = await _asientoDataService.GetByIdAsync(request.IdAsiento);

            if (asiento == null)
                throw new BusinessException("ASIENTO_NO_ENCONTRADO");

            if (!asiento.Disponible)
                throw new BusinessException("ASIENTO_OCUPADO");

            if (asiento.IdVuelo != request.IdVuelo)
                throw new BusinessException("ASIENTO_NO_CORRESPONDE");
            /*
            var usuario = await _usuarioDataService.GetByIdAsync(request.IdUsuario);

            if (usuario == null)
                throw new BusinessException("USUARIO_NO_ENCONTRADO");

            // 🔥 DIFERENCIAR CLIENTE VS ADMIN

            
            if (usuario.IdCliente.HasValue)
            {
                // 👤 CLIENTE
                request.IdCliente = usuario.IdCliente.Value;
            }
            else
            {
                // 👨‍💼 ADMIN
                if (request.IdCliente <= 0)
                    throw new BusinessException("CLIENTE_REQUERIDO",
                        "El administrador debe enviar un cliente válido.");
            }

            */

            // 🔥 SI VIENE USUARIO (flujo normal)
            if (request.IdUsuario > 0)
            {
                var usuario = await _usuarioDataService.GetByIdAsync(request.IdUsuario);

                if (usuario == null)
                    throw new BusinessException("USUARIO_NO_ENCONTRADO");

                if (usuario.IdCliente.HasValue)
                {
                    request.IdCliente = usuario.IdCliente.Value;
                }
                else
                {
                    if (request.IdCliente <= 0)
                        throw new BusinessException("CLIENTE_REQUERIDO");
                }
            }
            else
            {
                // 🔥 flujo BOOKING (sin usuario)
                if (request.IdCliente <= 0)
                    throw new BusinessException("CLIENTE_REQUERIDO");
            }


            // 🔥 CALCULAR PRECIOS
            decimal precioBase = vuelo.PrecioBase;
            decimal extraAsiento = asiento.PrecioExtra;

            decimal subtotal = precioBase + extraAsiento;
            const decimal IVA = 0.12m;
            decimal iva = subtotal * IVA;
            decimal total = subtotal + iva;

            request.SubtotalReserva = subtotal;
            request.ValorIva = iva;
            request.TotalReserva = total;
                

            var dataModel = ReservaBusinessMapper.ToDataModel(request);
            dataModel.CodigoReserva = $"RES-{System.DateTime.UtcNow:yyyyMMdd}-{System.Guid.NewGuid().ToString("N")[..6]}";

            var creada = await _reservaDataService.CreateAsync(dataModel);

            // 🔒 bloquear asiento
            asiento.Disponible = false;
            await _asientoDataService.UpdateAsync(asiento);

            return ReservaBusinessMapper.ToResponse(creada);
        }

        public async Task<IEnumerable<ReservaResponse>> GetByUsuarioAsync(int idUsuario)
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

            // 🔥 3. mapear
            return reservas.Select(ReservaBusinessMapper.ToResponse);
        }


        public async Task ConfirmarAsync(int idReserva, int idUsuario)
        {
            var reserva = await _reservaDataService.GetByIdAsync(idReserva);

            if (reserva == null)
                throw new BusinessException("RESERVA_NO_ENCONTRADA");

            if (reserva.EstadoReserva != "PEN")
                throw new BusinessException("SOLO_RESERVAS_PENDIENTES");

            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario);

            if (usuario == null)
                throw new BusinessException("USUARIO_NO_ENCONTRADO");

            if (!usuario.IdCliente.HasValue)
                throw new BusinessException("USUARIO_SIN_CLIENTE");

            // 🔥 VALIDACIÓN CORRECTA
            if (reserva.IdCliente != usuario.IdCliente.Value)
                throw new BusinessException("NO_AUTORIZADO");

            await CambiarEstadoAsync(idReserva, new ActualizarEstadoReservaRequest
            {
                EstadoReserva = "CON"
            });
        }

        public async Task CancelarClienteAsync(int idReserva, int idUsuario)
        {
            var reserva = await _reservaDataService.GetByIdAsync(idReserva);

            if (reserva == null)
                throw new BusinessException("RESERVA_NO_ENCONTRADA");

            // 🔥 solo puede cancelar si está pendiente o confirmada
            if (reserva.EstadoReserva != "PEN" && reserva.EstadoReserva != "CON")
                throw new BusinessException("NO_SE_PUEDE_CANCELAR");

            // 🔥 obtener usuario
            var usuario = await _usuarioDataService.GetByIdAsync(idUsuario);

            if (usuario == null)
                throw new BusinessException("USUARIO_NO_ENCONTRADO");

            if (!usuario.IdCliente.HasValue)
                throw new BusinessException("USUARIO_SIN_CLIENTE");

            // 🔥 VALIDACIÓN CORRECTA (cliente dueño)
            if (reserva.IdCliente != usuario.IdCliente.Value)
                throw new BusinessException("NO_AUTORIZADO");

            // 🔥 liberar asiento    
            var asiento = await _asientoDataService.GetByIdAsync(reserva.IdAsiento);

            if (asiento != null)
            {
                asiento.Disponible = true;
                await _asientoDataService.UpdateAsync(asiento);
            }

            // 🔥 cambiar estado
            reserva.EstadoReserva = "CAN";

            await _reservaDataService.UpdateAsync(reserva);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<ReservaResponse> GetByIdAsync(int id)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            return ReservaBusinessMapper.ToResponse(model);
        }

        // ============================================================
        // DETALLE
        // ============================================================
        public async Task<ReservaDetalleResponse> GetDetalleAsync(int id)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            var boletos = await _boletoDataService.GetByReservaAsync(id);
            var facturas = await _facturaDataService.GetByReservaAsync(id);

            return ReservaBusinessMapper.ToDetalleResponse(model, boletos, facturas);
        }

        // ============================================================
        // POR CLIENTE
        // ============================================================
        public async Task<IEnumerable<ReservaResponse>> GetByClienteAsync(int idCliente)
        {
            var reservas = await _reservaDataService.GetByClienteAsync(idCliente);
            return reservas.Select(ReservaBusinessMapper.ToResponse);
        }

        // ============================================================
        // FILTRAR
        // ============================================================
        public async Task<IEnumerable<ReservaResponse>> FiltrarAsync(ReservaFiltroRequest request)
        {
            var filtro = new ReservaFiltroDataModel
            {
                IdCliente = request.IdCliente,
                IdPasajero = request.IdPasajero,
                IdVuelo = request.IdVuelo,
                CodigoReserva = request.CodigoReserva,
                EstadoReserva = request.EstadoReserva,
                OrigenCanalReserva = request.OrigenCanalReserva,
                TotalMin = request.TotalMin,
                TotalMax = request.TotalMax,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _reservaDataService.GetPagedAsync(filtro);

            return resultado.Data.Select(ReservaBusinessMapper.ToResponse);
        }

        // ============================================================
        // ACTUALIZAR
        // ============================================================
        public async Task<ReservaResponse> ActualizarAsync(int id, ActualizarReservaRequest request)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            if (request.FechaInicio.HasValue)
                model.FechaInicio = request.FechaInicio.Value;

            if (request.FechaFin.HasValue)
                model.FechaFin = request.FechaFin.Value;

            if (request.TotalReserva.HasValue)
                model.TotalReserva = request.TotalReserva.Value;

            if (!string.IsNullOrWhiteSpace(request.ContactoEmail))
                model.ContactoEmail = request.ContactoEmail;

            await _reservaDataService.UpdateAsync(model);

            return ReservaBusinessMapper.ToResponse(model);
        }
        // ============================================================
        // CAMBIAR ESTADO
        // ============================================================
        public async Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoReservaRequest request)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            var nuevoEstado = request.EstadoReserva.ToUpper();

            // ============================================================
            // 🔥 CUANDO PASA A CON → CREAR FACTURA AUTOMÁTICA
            // ============================================================
            if (nuevoEstado == "CON")
            {
                var facturas = await _facturaDataService.GetByReservaAsync(id);

                // evitar duplicar factura
                if (facturas == null || !facturas.Any())
                {
                    var numeroFactura = $"FAC-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

                    var factura = new FacturaDataModel
                    {
                        IdCliente = model.IdCliente,
                        IdReserva = id,
                        NumeroFactura = numeroFactura,
                        FechaEmision = DateTime.UtcNow,

                        Subtotal = model.SubtotalReserva,
                        ValorIva = model.ValorIva,
                        CargoServicio = 0,
                        Total = model.TotalReserva,

                        Estado = "ABI",
                        ObservacionesFactura = "Generada automáticamente",
                        OrigenCanalFactura = "SISTEMA",
                        ServicioOrigen = "VUELOS"
                    };

                    await _facturaDataService.CreateAsync(factura);
                }

                // 🔥 GENERAR BOLETO AQUÍ (CORRECTO)
                var boletos = await _boletoDataService.GetByReservaAsync(id);

                if (boletos == null || !boletos.Any())
                {
                    var codigoBoleto = $"BOL-{DateTime.UtcNow:yyyyMMdd}-{Guid.NewGuid().ToString("N")[..6].ToUpper()}";

                    var asiento = await _asientoDataService.GetByIdAsync(model.IdAsiento);
                    var vuelo = await _vueloDataService.GetByIdAsync(model.IdVuelo);

                    var facturasActualizadas = await _facturaDataService.GetByReservaAsync(id);
                    var factura = facturasActualizadas.First();

                    var boleto = new BoletoDataModel
                    {
                        IdReserva = id,
                        IdVuelo = model.IdVuelo,
                        IdAsiento = model.IdAsiento,
                        IdFactura = factura.IdFactura,

                        CodigoBoleto = codigoBoleto,
                        Clase = asiento?.Clase ?? "ECONOMICA",
                        EstadoBoleto = "ACTIVO",
                        FechaEmision = DateTime.UtcNow,

                        PrecioVueloBase = vuelo.PrecioBase,
                        PrecioAsientoExtra = asiento?.PrecioExtra ?? 0,
                        ImpuestosBoleto = model.ValorIva,
                        CargoEquipaje = 0,

                        PrecioFinal = model.TotalReserva
                    };

                    await _boletoDataService.CreateAsync(boleto);
                }
            }





            // ============================================================
            // 🔥 VALIDAR FIN SOLO SI EL VUELO ATERRIZÓ
            // ============================================================
            if (nuevoEstado == "FIN")
            {
                var vuelo = await _vueloDataService.GetByIdAsync(model.IdVuelo);

                if (vuelo == null)
                    throw new BusinessException("VUELO_NO_ENCONTRADO",
                        "No se encontró el vuelo asociado.");

                if (vuelo.EstadoVuelo != "ATERRIZADO")
                    throw new BusinessException("VUELO_NO_FINALIZADO",
                        "No se puede finalizar la reserva si el vuelo no ha aterrizado.");
            }



            // 🔥 aplicar cambio
            model.EstadoReserva = nuevoEstado;

            await _reservaDataService.UpdateAsync(model);

            return true;



        }



        // ============================================================
        // CANCELAR
        // ============================================================
        public async Task<bool> CancelarAsync(int id, string motivo)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            await _reservaDataService.CancelAsync(id, motivo);

            return true;
        }

        // ============================================================
        // ELIMINAR
        // ============================================================
        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _reservaDataService.GetByIdAsync(id);

            if (model == null)
                throw new NotFoundException("Reserva", id);

            await _reservaDataService.DeleteAsync(id);

            return true;
        }

        // booking

        public async Task<ReservaBookingResponse> CrearBookingAsync(CrearReservaBookingRequest request)
        {
            // 🔥 crear request interno
            var internalRequest = new CrearReservaRequest
            {
                IdCliente = request.IdCliente,
                IdPasajero = request.IdPasajero,
                IdVuelo = request.IdVuelo,
                IdAsiento = request.IdAsiento,
                FechaInicio = request.FechaInicio,
                FechaFin = request.FechaFin,
                SubtotalReserva = request.SubtotalReserva,
                ValorIva = request.ValorIva,
                TotalReserva = request.TotalReserva,
                OrigenCanalReserva = request.OrigenCanalReserva,
                ContactoEmail = request.ContactoEmail,
                ContactoTelefono = request.ContactoTelefono,
                Observaciones = request.Observaciones,

                // 🔥 CLAVE: simular admin
                IdUsuario = 0
            };

            // 🔥 llamar lógica real
            var reserva = await CrearAsync(internalRequest);

            // 🔥 mapear respuesta
            return new ReservaBookingResponse
            {
                IdReserva = reserva.IdReserva,
                CodigoReserva = reserva.CodigoReserva,
                IdCliente = reserva.IdCliente,
                IdPasajero = reserva.IdPasajero,
                IdVuelo = reserva.IdVuelo,
                IdAsiento = reserva.IdAsiento,
                FechaInicio = reserva.FechaInicio,
                FechaFin = reserva.FechaFin,
                TotalReserva = reserva.TotalReserva,
                EstadoReserva = reserva.EstadoReserva
            };
        }

        public async Task<bool> ActualizarEstadoBookingAsync(int id, ActualizarEstadoReservaBookingRequest request)
        {
            var internalRequest = new ActualizarEstadoReservaRequest
            {
                EstadoReserva = request.EstadoReserva,
                MotivoCancelacion = request.MotivoCancelacion
            };

            return await CambiarEstadoAsync(id, internalRequest);
        }



    }
}