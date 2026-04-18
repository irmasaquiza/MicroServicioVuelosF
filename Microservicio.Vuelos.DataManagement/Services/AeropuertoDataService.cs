using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class AeropuertoDataService : IAeropuertoDataService
    {
        private readonly IUnitOfWork _uow;

        public AeropuertoDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<AeropuertoDataModel>> GetAllAsync()
        {
            var entities = await _uow.AeropuertoRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<AeropuertoDataModel>();

            return entities.Select(AeropuertoDataMapper.ToDataModel);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<AeropuertoDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del aeropuerto debe ser mayor a 0.", nameof(id));

            var entity = await _uow.AeropuertoRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return AeropuertoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET PAGED — con filtros
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<AeropuertoDataModel>> GetPagedAsync(AeropuertoFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(nameof(filtro), "El filtro no puede ser nulo.");

            // Asegurar valores de paginación válidos
            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            // Traer todos los no eliminados desde el repositorio
            var todos = await _uow.AeropuertoRepository.GetAllAsync();

            // ── Aplicar filtros en memoria ──────────────────
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.CodigoIata))
                query = query.Where(a =>
                    a.CodigoIata.ToUpper() == filtro.CodigoIata.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.CodigoIcao))
                query = query.Where(a =>
                    a.CodigoIcao.ToUpper() == filtro.CodigoIcao.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                query = query.Where(a =>
                    a.Nombre.ToUpper().Contains(filtro.Nombre.ToUpper()));

            if (filtro.IdCiudad.HasValue)
                query = query.Where(a => a.IdCiudad == filtro.IdCiudad.Value);

            if (filtro.IdPais.HasValue)
                query = query.Where(a => a.IdPais == filtro.IdPais.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(a =>
                    a.Estado.ToUpper() == filtro.Estado.ToUpper());

            // ── Paginación ──────────────────────────────────
            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(AeropuertoDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<AeropuertoDataModel>
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
        public async Task<AeropuertoDataModel> CreateAsync(AeropuertoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "El modelo no puede ser nulo.");

            // Validaciones básicas de negocio
            if (string.IsNullOrWhiteSpace(model.CodigoIata))
                throw new ArgumentException("El código IATA es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Nombre))
                throw new ArgumentException("El nombre del aeropuerto es obligatorio.");

            // Verificar que no exista ya un aeropuerto con ese IATA
            var existente = await _uow.AeropuertoRepository
                                      .GetByCodigoAsync(model.CodigoIata);

            if (existente != null)
                throw new InvalidOperationException(
                    $"Ya existe un aeropuerto con el código IATA '{model.CodigoIata}'.");

            // Convertir a entidad
            var entity = AeropuertoDataMapper.ToEntity(model);

            // ── Campos de auditoría ─────────────────────────
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.Eliminado = false;
            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                                            ? "ACTIVO"
                                            : model.Estado;

            // Persistir
            await _uow.AeropuertoRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return AeropuertoDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(AeropuertoDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model), "El modelo no puede ser nulo.");

            if (model.IdAeropuerto <= 0)
                throw new ArgumentException("El ID del aeropuerto debe ser mayor a 0.");

            // Buscar entidad existente
            var entity = await _uow.AeropuertoRepository.GetByIdAsync(model.IdAeropuerto);

            if (entity == null)
                return false; // No encontrado

            // Verificar unicidad de IATA si cambió
            if (!string.IsNullOrWhiteSpace(model.CodigoIata) &&
                model.CodigoIata.ToUpper() != entity.CodigoIata.ToUpper())
            {
                var conMismoIata = await _uow.AeropuertoRepository
                                             .GetByCodigoAsync(model.CodigoIata);

                if (conMismoIata != null &&
                    conMismoIata.IdAeropuerto != model.IdAeropuerto)
                    throw new InvalidOperationException(
                        $"Ya existe otro aeropuerto con el código IATA '{model.CodigoIata}'.");
            }

            // Aplicar cambios mediante el mapper (respeta campos protegidos)
            AeropuertoDataMapper.UpdateEntity(entity, model);

            // ── Auditoría de modificación ───────────────────
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.AeropuertoRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // DELETE — eliminación lógica
        // ─────────────────────────────────────────────
        public async Task<bool> DeleteAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("El ID del aeropuerto debe ser mayor a 0.", nameof(id));

            var entity = await _uow.AeropuertoRepository.GetByIdAsync(id);

            if (entity == null)
                return false; // No encontrado o ya eliminado

            // El repositorio ya hace: entity.Eliminado = true + Update
            _uow.AeropuertoRepository.Delete(entity);

            // Auditoría de eliminación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}