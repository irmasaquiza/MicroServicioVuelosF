// ============================================================
// VueloDataService.cs
// ============================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class VueloDataService : IVueloDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] EstadosVueloValidos =
        {
            "PROGRAMADO", "EN_VUELO",
            "ATERRIZADO",  "CANCELADO", "DEMORADO"
        };

        public VueloDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        public async Task<IEnumerable<VueloDataModel>> GetAllAsync()
        {
            var entities = await _uow.VueloRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<VueloDataModel>();

            return entities.Select(VueloDataMapper.ToDataModel);
        }

        public async Task<VueloDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.", nameof(id));

            var entity = await _uow.VueloRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return VueloDataMapper.ToDataModel(entity);
        }

        public async Task<VueloDataModel> GetByCodigoAsync(string codigoVuelo)
        {
            if (string.IsNullOrWhiteSpace(codigoVuelo))
                throw new ArgumentException(
                    "El código del vuelo no puede estar vacío.",
                    nameof(codigoVuelo));

            var entity = await _uow.VueloRepository
                                   .GetByCodigoAsync(codigoVuelo.Trim().ToUpper());

            if (entity == null)
                return null;

            return VueloDataMapper.ToDataModel(entity);
        }

        public async Task<IEnumerable<VueloDataModel>> GetByOrigenAsync(
            int idAeropuertoOrigen)
        {
            if (idAeropuertoOrigen <= 0)
                throw new ArgumentException(
                    "El ID del aeropuerto de origen debe ser mayor a 0.",
                    nameof(idAeropuertoOrigen));

            var todos = await _uow.VueloRepository.GetAllAsync();

            var filtrados = todos
                .Where(v => v.IdAeropuertoOrigen == idAeropuertoOrigen)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<VueloDataModel>();

            return filtrados.Select(VueloDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VueloDataModel>> GetByDestinoAsync(
            int idAeropuertoDestino)
        {
            if (idAeropuertoDestino <= 0)
                throw new ArgumentException(
                    "El ID del aeropuerto de destino debe ser mayor a 0.",
                    nameof(idAeropuertoDestino));

            var todos = await _uow.VueloRepository.GetAllAsync();

            var filtrados = todos
                .Where(v => v.IdAeropuertoDestino == idAeropuertoDestino)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<VueloDataModel>();

            return filtrados.Select(VueloDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VueloDataModel>> GetByFechaAsync(
            DateTime fechaInicio, DateTime fechaFin)
        {
            if (fechaInicio > fechaFin)
                throw new ArgumentException(
                    "La fecha de inicio no puede ser mayor a la fecha de fin.");

            var entities = await _uow.VueloRepository.GetAllAsync();

            var filtrados = entities
                .Where(v =>
                    v.FechaHoraSalida >= fechaInicio &&
                    v.FechaHoraSalida <= fechaFin)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<VueloDataModel>();

            return filtrados.Select(VueloDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VueloDataModel>> GetByEstadoAsync(
            string estadoVuelo)
        {
            if (string.IsNullOrWhiteSpace(estadoVuelo))
                throw new ArgumentException(
                    "El estado del vuelo no puede estar vacío.",
                    nameof(estadoVuelo));

            if (!EstadosVueloValidos.Contains(estadoVuelo.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosVueloValidos)}");

            var entities = await _uow.VueloRepository
                                     .GetByEstadoAsync(estadoVuelo.ToUpper());

            if (entities == null || !entities.Any())
                return Enumerable.Empty<VueloDataModel>();

            return entities.Select(VueloDataMapper.ToDataModel);
        }

        public async Task<IEnumerable<VueloDataModel>> GetDisponiblesAsync()
        {
            var entities = await _uow.VueloRepository.GetDisponiblesAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<VueloDataModel>();

            return entities.Select(VueloDataMapper.ToDataModel);
        }

        public async Task<DataPagedResult<VueloDataModel>> GetPagedAsync(
            VueloFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.VueloRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.CodigoVuelo))
                query = query.Where(v =>
                    v.CodigoVuelo.ToUpper()
                     .Contains(filtro.CodigoVuelo.ToUpper()));

            if (filtro.IdAeropuertoOrigen.HasValue)
                query = query.Where(v =>
                    v.IdAeropuertoOrigen == filtro.IdAeropuertoOrigen.Value);

            if (filtro.IdAeropuertoDestino.HasValue)
                query = query.Where(v =>
                    v.IdAeropuertoDestino == filtro.IdAeropuertoDestino.Value);

            if (!string.IsNullOrWhiteSpace(filtro.EstadoVuelo))
                query = query.Where(v =>
                    v.EstadoVuelo.ToUpper() == filtro.EstadoVuelo.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.TipoVuelo))
                query = query.Where(v =>
                    v.TipoVuelo != null &&
                    v.TipoVuelo.ToUpper() == filtro.TipoVuelo.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.Aerolinea))
                query = query.Where(v =>
                    v.Aerolinea != null &&
                    v.Aerolinea.ToUpper().Contains(filtro.Aerolinea.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Terminal))
                query = query.Where(v =>
                    v.Terminal != null &&
                    v.Terminal.ToUpper() == filtro.Terminal.ToUpper());

            if (filtro.FechaSalidaInicio.HasValue)
                query = query.Where(v =>
                    v.FechaHoraSalida >= filtro.FechaSalidaInicio.Value);

            if (filtro.FechaSalidaFin.HasValue)
                query = query.Where(v =>
                    v.FechaHoraSalida <= filtro.FechaSalidaFin.Value);

            if (filtro.PrecioMin.HasValue)
                query = query.Where(v => v.PrecioBase >= filtro.PrecioMin.Value);

            if (filtro.PrecioMax.HasValue)
                query = query.Where(v => v.PrecioBase <= filtro.PrecioMax.Value);

            if (filtro.CapacidadDisponibleMin.HasValue)
                query = query.Where(v =>
                    v.CapacidadDisponible >= filtro.CapacidadDisponibleMin.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(v =>
                    v.Estado != null &&
                    v.Estado.ToUpper() == filtro.Estado.ToUpper());

            query = query.OrderBy(v => v.FechaHoraSalida);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(VueloDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<VueloDataModel>
            {
                Data = items,
                Meta = new MetaData
                {
                    Page = filtro.Page,
                    PageSize = filtro.PageSize,
                    Total = total,
                    TotalPages = totalPages
                }
            };
        }

        public async Task<VueloDataModel> CreateAsync(VueloDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.CodigoVuelo))
                throw new ArgumentException("El código del vuelo es obligatorio.");

            if (model.IdAeropuertoOrigen <= 0)
                throw new ArgumentException(
                    "El aeropuerto de origen es obligatorio.");

            if (model.IdAeropuertoDestino <= 0)
                throw new ArgumentException(
                    "El aeropuerto de destino es obligatorio.");

            // CK_Vuelo_OrigenDestino
            if (model.IdAeropuertoOrigen == model.IdAeropuertoDestino)
                throw new ArgumentException(
                    "El aeropuerto de origen y destino no pueden ser el mismo.");

            // CK_Vuelo_Fechas
            if (model.FechaHoraLlegada <= model.FechaHoraSalida)
                throw new ArgumentException(
                    "La fecha de llegada debe ser posterior a la de salida.");

            // CK_Vuelo_Duracion
            if (model.DuracionMin < 0)
                throw new ArgumentException(
                    "La duración no puede ser negativa.");

            if (model.PrecioBase < 0)
                throw new ArgumentException(
                    "El precio base no puede ser negativo.");

            if (model.CapacidadTotal <= 0)
                throw new ArgumentException(
                    "La capacidad total debe ser mayor a 0.");

            // Verificar código único
            var existente = await _uow.VueloRepository
                                      .GetByCodigoAsync(model.CodigoVuelo.Trim());

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe un vuelo con el código '{model.CodigoVuelo}'.");

            // Verificar que aeropuertos existan
            var origen = await _uow.AeropuertoRepository
                                   .GetByIdAsync(model.IdAeropuertoOrigen);

            if (origen == null)
                throw new InvalidOperationException(
                    $"No existe el aeropuerto de origen con ID " +
                    $"'{model.IdAeropuertoOrigen}'.");

            var destino = await _uow.AeropuertoRepository
                                    .GetByIdAsync(model.IdAeropuertoDestino);

            if (destino == null)
                throw new InvalidOperationException(
                    $"No existe el aeropuerto de destino con ID " +
                    $"'{model.IdAeropuertoDestino}'.");

            var entity = VueloDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.Estado = "ACTIVO";
            entity.CodigoVuelo = model.CodigoVuelo.Trim().ToUpper();
            entity.CapacidadDisponible = model.CapacidadTotal;

            entity.EstadoVuelo = string.IsNullOrWhiteSpace(model.EstadoVuelo)
                ? "PROGRAMADO"
                : model.EstadoVuelo.ToUpper();

            await _uow.VueloRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return VueloDataMapper.ToDataModel(entity);
        }

        public async Task<bool> UpdateAsync(VueloDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdVuelo <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.");

            var entity = await _uow.VueloRepository.GetByIdAsync(model.IdVuelo);

            if (entity == null)
                return false;

            if (entity.EstadoVuelo == "CANCELADO")
                throw new InvalidOperationException(
                    "No se puede modificar un vuelo cancelado.");

            if (!string.IsNullOrWhiteSpace(model.EstadoVuelo) &&
                !EstadosVueloValidos.Contains(model.EstadoVuelo.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. Los válidos son: " +
                    $"{string.Join(", ", EstadosVueloValidos)}");

            if (model.FechaHoraLlegada <= model.FechaHoraSalida)
                throw new ArgumentException(
                    "La fecha de llegada debe ser posterior a la de salida.");

            if (model.IdAeropuertoOrigen == model.IdAeropuertoDestino &&
                model.IdAeropuertoOrigen > 0)
                throw new ArgumentException(
                    "El aeropuerto de origen y destino no pueden ser el mismo.");

            VueloDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.VueloRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> CancelAsync(int id, string motivo)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.", nameof(id));

            if (string.IsNullOrWhiteSpace(motivo))
                throw new ArgumentException(
                    "El motivo de cancelación es obligatorio.", nameof(motivo));

            var entity = await _uow.VueloRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            if (entity.EstadoVuelo == "CANCELADO")
                throw new InvalidOperationException(
                    "El vuelo ya se encuentra cancelado.");

            if (entity.EstadoVuelo == "ATERRIZADO")
                throw new InvalidOperationException(
                    "No se puede cancelar un vuelo que ya aterrizó.");

            entity.EstadoVuelo = "CANCELADO";
            entity.Observaciones = motivo.Trim();
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.VueloRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del vuelo debe ser mayor a 0.", nameof(id));

            var entity = await _uow.VueloRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            if (entity.EstadoVuelo == "EN_VUELO")
                throw new InvalidOperationException(
                    "No se puede eliminar un vuelo que está en curso.");

            _uow.VueloRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}