
// ============================================================
// Services/PasajeroService.cs
// ============================================================
using Microservicio.Vuelos.Business.DTOs.Booking.Pasajero;
using Microservicio.Vuelos.Business.DTOs.Internal.Pasajero;
using Microservicio.Vuelos.Business.Exceptions;
using Microservicio.Vuelos.Business.Interfaces;
using Microservicio.Vuelos.Business.Mappers;
using Microservicio.Vuelos.Business.Validators;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Booking.Pasajero;

namespace Microservicio.Vuelos.Business.Services
{
    public class PasajeroService : IPasajeroService
    {
        private readonly IPasajeroDataService _pasajeroDataService;
        private readonly IAuditoriaLogService _auditoria;

        public PasajeroService(
            IPasajeroDataService pasajeroDataService,
            IAuditoriaLogService auditoria)
        {
            _pasajeroDataService = pasajeroDataService;
            _auditoria = auditoria;
        }

        public async Task<PasajeroResponse> CrearAsync(CrearPasajeroRequest request)
        {
            PasajeroValidator.ValidarCrear(request);

            var dataModel = PasajeroBusinessMapper.ToDataModel(request);
            var creado = await _pasajeroDataService.CreateAsync(dataModel);

            return PasajeroBusinessMapper.ToResponse(creado);
        }

        public async Task<PasajeroResponse> GetByIdAsync(int id)
        {
            var model = await _pasajeroDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Pasajero", id);

            return PasajeroBusinessMapper.ToResponse(model);
        }

        public async Task<IEnumerable<PasajeroResponse>> GetByClienteAsync(int idCliente)
        {
            var pasajeros = await _pasajeroDataService.GetByClienteAsync(idCliente);
            return pasajeros.Select(PasajeroBusinessMapper.ToResponse);
        }

        public async Task<IEnumerable<PasajeroResponse>> FiltrarAsync(PasajeroFiltroRequest request)
        {
            var filtro = new PasajeroFiltroDataModel
            {
                IdCliente = request.IdCliente,
                NombrePasajero = request.NombrePasajero,
                ApellidoPasajero = request.ApellidoPasajero,
                TipoDocumentoPasajero = request.TipoDocumentoPasajero,
                NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
                NacionalidadPasajero = request.NacionalidadPasajero,
                RequiereAsistencia = request.RequiereAsistencia,
                FechaNacimientoInicio = request.FechaNacimientoInicio,
                FechaNacimientoFin = request.FechaNacimientoFin,
                Page = request.Page,
                PageSize = request.PageSize
            };

            var resultado = await _pasajeroDataService.GetPagedAsync(filtro);
            return resultado.Data.Select(PasajeroBusinessMapper.ToResponse);
        }

        public async Task<PasajeroResponse> ActualizarAsync(int id, ActualizarPasajeroRequest request)
        {
            PasajeroValidator.ValidarActualizar(request);

            var model = await _pasajeroDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Pasajero", id);

            if (!string.IsNullOrWhiteSpace(request.NombrePasajero))
                model.NombrePasajero = request.NombrePasajero.Trim();
            if (!string.IsNullOrWhiteSpace(request.ApellidoPasajero))
                model.ApellidoPasajero = request.ApellidoPasajero.Trim();
            if (!string.IsNullOrWhiteSpace(request.TipoDocumentoPasajero))
                model.TipoDocumentoPasajero = request.TipoDocumentoPasajero.ToUpper();
            if (!string.IsNullOrWhiteSpace(request.NumeroDocumentoPasajero))
                model.NumeroDocumentoPasajero = request.NumeroDocumentoPasajero.Trim();
            if (request.FechaNacimientoPasajero.HasValue)
                model.FechaNacimientoPasajero = request.FechaNacimientoPasajero;
            if (!string.IsNullOrWhiteSpace(request.NacionalidadPasajero))
                model.NacionalidadPasajero = request.NacionalidadPasajero.Trim();
            if (!string.IsNullOrWhiteSpace(request.EmailContactoPasajero))
                model.EmailContactoPasajero = request.EmailContactoPasajero.ToLower().Trim();
            if (!string.IsNullOrWhiteSpace(request.TelefonoContactoPasajero))
                model.TelefonoContactoPasajero = request.TelefonoContactoPasajero.Trim();
            if (!string.IsNullOrWhiteSpace(request.GeneroPasajero))
                model.GeneroPasajero = request.GeneroPasajero;
            if (request.RequiereAsistencia.HasValue)
                model.RequiereAsistencia = request.RequiereAsistencia.Value;
            if (!string.IsNullOrWhiteSpace(request.ObservacionesPasajero))
                model.ObservacionesPasajero = request.ObservacionesPasajero.Trim();

            await _pasajeroDataService.UpdateAsync(model);
            return PasajeroBusinessMapper.ToResponse(model);
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var model = await _pasajeroDataService.GetByIdAsync(id);
            if (model == null)
                throw new NotFoundException("Pasajero", id);

            await _pasajeroDataService.DeleteAsync(id);
            return true;
        }


        // booking

    public async Task<PasajeroBookingResponse> CrearBookingAsync(CrearPasajeroBookingRequest request)
    {
        var internalRequest = new CrearPasajeroRequest
        {
            NombrePasajero = request.NombrePasajero,
            ApellidoPasajero = request.ApellidoPasajero,
            TipoDocumentoPasajero = request.TipoDocumentoPasajero,
            NumeroDocumentoPasajero = request.NumeroDocumentoPasajero,
            IdCliente = request.IdCliente,
            FechaNacimientoPasajero = request.FechaNacimientoPasajero,
            NacionalidadPasajero = request.NacionalidadPasajero,
            EmailContactoPasajero = request.EmailContactoPasajero,
            TelefonoContactoPasajero = request.TelefonoContactoPasajero,
            GeneroPasajero = request.GeneroPasajero,
            RequiereAsistencia = request.RequiereAsistencia,
            ObservacionesPasajero = request.ObservacionesPasajero
        };

        var pasajero = await CrearAsync(internalRequest);

        return new PasajeroBookingResponse
        {
            IdPasajero = pasajero.IdPasajero,
            NombrePasajero = pasajero.NombrePasajero,
            ApellidoPasajero = pasajero.ApellidoPasajero,
            TipoDocumentoPasajero = pasajero.TipoDocumentoPasajero,
            NumeroDocumentoPasajero = pasajero.NumeroDocumentoPasajero,
            FechaNacimientoPasajero = pasajero.FechaNacimientoPasajero,
            RequiereAsistencia = pasajero.RequiereAsistencia
        };
    }
}
}

