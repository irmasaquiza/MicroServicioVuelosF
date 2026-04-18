// ============================================================
// PasajeroDataService.cs
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
    public class PasajeroDataService : IPasajeroDataService
    {
        private readonly IUnitOfWork _uow;

        private static readonly string[] TiposDocumentoValidos =
            { "CEDULA", "PASAPORTE", "RUC", "OTRO" };

        public PasajeroDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<PasajeroDataModel>> GetAllAsync()
        {
            var entities = await _uow.PasajeroRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<PasajeroDataModel>();

            return PasajeroDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<PasajeroDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del pasajero debe ser mayor a 0.", nameof(id));

            var entity = await _uow.PasajeroRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return PasajeroDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CLIENTE
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<PasajeroDataModel>> GetByClienteAsync(
            int idCliente)
        {
            if (idCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.", nameof(idCliente));

            var entities = await _uow.PasajeroRepository.GetByClienteAsync(idCliente);

            if (entities == null || !entities.Any())
                return Enumerable.Empty<PasajeroDataModel>();

            return PasajeroDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY DOCUMENTO
        // ─────────────────────────────────────────────
        public async Task<PasajeroDataModel> GetByDocumentoAsync(string numeroDocumento)
        {
            if (string.IsNullOrWhiteSpace(numeroDocumento))
                throw new ArgumentException(
                    "El número de documento no puede estar vacío.",
                    nameof(numeroDocumento));

            var entity = await _uow.PasajeroRepository
                                   .GetByDocumentoAsync(numeroDocumento.Trim());

            if (entity == null)
                return null;

            return PasajeroDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY NACIONALIDAD
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<PasajeroDataModel>> GetByNacionalidadAsync(
            string nacionalidad)
        {
            if (string.IsNullOrWhiteSpace(nacionalidad))
                throw new ArgumentException(
                    "La nacionalidad no puede estar vacía.",
                    nameof(nacionalidad));

            var todos = await _uow.PasajeroRepository.GetAllAsync();

            var filtrados = todos
                .Where(p =>
                    p.NacionalidadPasajero != null &&
                    p.NacionalidadPasajero.ToUpper()
                     .Contains(nacionalidad.ToUpper()))
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<PasajeroDataModel>();

            return PasajeroDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET PAGED
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<PasajeroDataModel>> GetPagedAsync(
            PasajeroFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro));

            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            var todos = await _uow.PasajeroRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (filtro.IdCliente.HasValue)
                query = query.Where(p => p.IdCliente == filtro.IdCliente.Value);

            if (!string.IsNullOrWhiteSpace(filtro.NombrePasajero))
                query = query.Where(p =>
                    p.NombrePasajero.ToUpper()
                     .Contains(filtro.NombrePasajero.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.ApellidoPasajero))
                query = query.Where(p =>
                    p.ApellidoPasajero.ToUpper()
                     .Contains(filtro.ApellidoPasajero.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.TipoDocumentoPasajero))
                query = query.Where(p =>
                    p.TipoDocumentoPasajero.ToUpper() ==
                    filtro.TipoDocumentoPasajero.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.NumeroDocumentoPasajero))
                query = query.Where(p =>
                    p.NumeroDocumentoPasajero.Contains(
                        filtro.NumeroDocumentoPasajero.Trim()));

            if (!string.IsNullOrWhiteSpace(filtro.NacionalidadPasajero))
                query = query.Where(p =>
                    p.NacionalidadPasajero != null &&
                    p.NacionalidadPasajero.ToUpper()
                     .Contains(filtro.NacionalidadPasajero.ToUpper()));

            if (filtro.RequiereAsistencia.HasValue)
                query = query.Where(p =>
                    p.RequiereAsistencia == filtro.RequiereAsistencia.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(p =>
                    p.Estado.ToUpper() == filtro.Estado.ToUpper());

            if (filtro.FechaNacimientoInicio.HasValue)
                query = query.Where(p =>
                    p.FechaNacimientoPasajero.HasValue &&
                    p.FechaNacimientoPasajero.Value >=
                    filtro.FechaNacimientoInicio.Value);

            if (filtro.FechaNacimientoFin.HasValue)
                query = query.Where(p =>
                    p.FechaNacimientoPasajero.HasValue &&
                    p.FechaNacimientoPasajero.Value <=
                    filtro.FechaNacimientoFin.Value);

            query = query.OrderBy(p => p.ApellidoPasajero)
                         .ThenBy(p => p.NombrePasajero);

            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(PasajeroDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<PasajeroDataModel>
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

        // ─────────────────────────────────────────────
        // CREATE
        // ─────────────────────────────────────────────
        public async Task<PasajeroDataModel> CreateAsync(PasajeroDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (string.IsNullOrWhiteSpace(model.NombrePasajero))
                throw new ArgumentException("El nombre del pasajero es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.ApellidoPasajero))
                throw new ArgumentException(
                    "El apellido del pasajero es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.TipoDocumentoPasajero))
                throw new ArgumentException(
                    "El tipo de documento es obligatorio.");

            if (!TiposDocumentoValidos.Contains(
                    model.TipoDocumentoPasajero.ToUpper()))
                throw new ArgumentException(
                    $"Tipo de documento inválido. " +
                    $"Los válidos son: {string.Join(", ", TiposDocumentoValidos)}");

            if (string.IsNullOrWhiteSpace(model.NumeroDocumentoPasajero))
                throw new ArgumentException(
                    "El número de documento es obligatorio.");

            if (model.FechaNacimientoPasajero.HasValue &&
                model.FechaNacimientoPasajero.Value > DateTime.Today)
                throw new ArgumentException(
                    "La fecha de nacimiento no puede ser una fecha futura.");

            // Verificar que cliente exista si viene el ID
            if (model.IdCliente.HasValue && model.IdCliente.Value > 0)
            {
                var cliente = await _uow.ClienteRepository
                                        .GetByIdAsync(model.IdCliente.Value);

                if (cliente == null)
                    throw new InvalidOperationException(
                        $"No existe un cliente con ID '{model.IdCliente}'.");
            }

            var entity = PasajeroDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.TipoDocumentoPasajero = model.TipoDocumentoPasajero.ToUpper();

            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                ? "ACTIVO"
                : model.Estado.ToUpper();

            await _uow.PasajeroRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return PasajeroDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(PasajeroDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdPasajero <= 0)
                throw new ArgumentException(
                    "El ID del pasajero debe ser mayor a 0.");

            var entity = await _uow.PasajeroRepository.GetByIdAsync(model.IdPasajero);

            if (entity == null)
                return false;

            if (!string.IsNullOrWhiteSpace(model.TipoDocumentoPasajero) &&
                !TiposDocumentoValidos.Contains(
                    model.TipoDocumentoPasajero.ToUpper()))
                throw new ArgumentException(
                    $"Tipo de documento inválido. " +
                    $"Los válidos son: {string.Join(", ", TiposDocumentoValidos)}");

            if (model.FechaNacimientoPasajero.HasValue &&
                model.FechaNacimientoPasajero.Value > DateTime.Today)
                throw new ArgumentException(
                    "La fecha de nacimiento no puede ser una fecha futura.");

            PasajeroDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.PasajeroRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // DELETE — eliminación lógica
        // ─────────────────────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del pasajero debe ser mayor a 0.", nameof(id));

            var entity = await _uow.PasajeroRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            _uow.PasajeroRepository.Delete(entity);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}