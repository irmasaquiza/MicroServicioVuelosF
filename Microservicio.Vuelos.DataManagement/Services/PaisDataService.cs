// ============================================================
// PaisDataService.cs
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
    public class PaisDataService : IPaisDataService
    {
        private readonly IUnitOfWork _uow;

        // Continentes válidos — referencia según datos de la BD
        private static readonly string[] ContinentesValidos =
        {
            "AMÉRICA DEL SUR",
            "AMÉRICA DEL NORTE",
            "AMÉRICA CENTRAL",
            "EUROPA",
            "ASIA",
            "ÁFRICA",
            "OCEANÍA",
            "ANTÁRTIDA"
        };

        public PaisDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<PaisDataModel>> GetAllAsync()
        {
            var entities = await _uow.PaisRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<PaisDataModel>();

            return PaisDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<PaisDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del país debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.PaisRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return PaisDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY ISO2
        // ─────────────────────────────────────────────
        public async Task<PaisDataModel> GetByIso2Async(string codigoIso2)
        {
            if (string.IsNullOrWhiteSpace(codigoIso2))
                throw new ArgumentException(
                    "El código ISO2 no puede estar vacío.",
                    nameof(codigoIso2));

            if (codigoIso2.Trim().Length != 2)
                throw new ArgumentException(
                    "El código ISO2 debe tener exactamente 2 caracteres.",
                    nameof(codigoIso2));

            var entity = await _uow.PaisRepository
                                   .GetByCodigoIso2Async(codigoIso2.Trim().ToUpper());

            if (entity == null)
                return null;

            return PaisDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY ISO3
        // ─────────────────────────────────────────────
        public async Task<PaisDataModel> GetByIso3Async(string codigoIso3)
        {
            if (string.IsNullOrWhiteSpace(codigoIso3))
                throw new ArgumentException(
                    "El código ISO3 no puede estar vacío.",
                    nameof(codigoIso3));

            if (codigoIso3.Trim().Length != 3)
                throw new ArgumentException(
                    "El código ISO3 debe tener exactamente 3 caracteres.",
                    nameof(codigoIso3));

            var entity = await _uow.PaisRepository
                                   .GetByCodigoIso3Async(codigoIso3.Trim().ToUpper());

            if (entity == null)
                return null;

            return PaisDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CONTINENTE
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<PaisDataModel>> GetByContinenteAsync(string continente)
        {
            if (string.IsNullOrWhiteSpace(continente))
                throw new ArgumentException(
                    "El continente no puede estar vacío.",
                    nameof(continente));

            // Traer todos y filtrar por continente en memoria
            var todos = await _uow.PaisRepository.GetAllAsync();

            var filtrados = todos
                .Where(p =>
                    p.Continente != null &&
                    p.Continente.ToUpper().Contains(continente.ToUpper()))
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<PaisDataModel>();

            return PaisDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET PAGED — con filtros
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<PaisDataModel>> GetPagedAsync(
            PaisFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(
                    nameof(filtro),
                    "El filtro no puede ser nulo.");

            // Asegurar paginación válida
            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            // Traer todos los no eliminados
            var todos = await _uow.PaisRepository.GetAllAsync();

            // ── Aplicar filtros en memoria ──────────────────
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.Nombre))
                query = query.Where(p =>
                    p.Nombre.ToUpper().Contains(filtro.Nombre.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.CodigoIso2))
                query = query.Where(p =>
                    p.CodigoIso2 != null &&
                    p.CodigoIso2.ToUpper() == filtro.CodigoIso2.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.CodigoIso3))
                query = query.Where(p =>
                    p.CodigoIso3 != null &&
                    p.CodigoIso3.ToUpper() == filtro.CodigoIso3.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.Continente))
                query = query.Where(p =>
                    p.Continente != null &&
                    p.Continente.ToUpper().Contains(filtro.Continente.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(p =>
                    p.Estado != null &&
                    p.Estado.ToUpper() == filtro.Estado.ToUpper());

            // Ordenar alfabéticamente por nombre
            query = query.OrderBy(p => p.Nombre);

            // ── Paginación ──────────────────────────────────
            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(PaisDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<PaisDataModel>
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
        public async Task<PaisDataModel> CreateAsync(PaisDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo de país no puede ser nulo.");

            // ── Validaciones de negocio ─────────────────────
            if (string.IsNullOrWhiteSpace(model.Nombre))
                throw new ArgumentException(
                    "El nombre del país es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.CodigoIso2))
                throw new ArgumentException(
                    "El código ISO2 es obligatorio.");

            if (model.CodigoIso2.Trim().Length != 2)
                throw new ArgumentException(
                    "El código ISO2 debe tener exactamente 2 caracteres.");

            if (!string.IsNullOrWhiteSpace(model.CodigoIso3) &&
                model.CodigoIso3.Trim().Length != 3)
                throw new ArgumentException(
                    "El código ISO3 debe tener exactamente 3 caracteres.");

            // Verificar duplicado ISO2 (UQ_Pais_iso2 en BD)
            var existenteIso2 = await _uow.PaisRepository
                                          .GetByCodigoIso2Async(
                                              model.CodigoIso2.Trim().ToUpper());

            if (existenteIso2 != null)
                throw new InvalidOperationException(
                    $"Ya existe un país con el código ISO2 '{model.CodigoIso2}'.");

            // Verificar duplicado ISO3 si viene (UQ_Pais_iso3 en BD)
            if (!string.IsNullOrWhiteSpace(model.CodigoIso3))
            {
                var existenteIso3 = await _uow.PaisRepository
                                              .GetByCodigoIso3Async(
                                                  model.CodigoIso3.Trim().ToUpper());

                if (existenteIso3 != null)
                    throw new InvalidOperationException(
                        $"Ya existe un país con el código ISO3 '{model.CodigoIso3}'.");
            }

            // Verificar duplicado por nombre (UQ_Pais_nombre en BD)
            var existenteNombre = await _uow.PaisRepository
                                            .GetByNombreAsync(model.Nombre.Trim());

            if (existenteNombre != null && existenteNombre.Any())
                throw new InvalidOperationException(
                    $"Ya existe un país con el nombre '{model.Nombre}'.");

            // ── Construir entidad ───────────────────────────
            var entity = PaisDataMapper.ToEntity(model);

            // Normalizar códigos
            entity.CodigoIso2 = model.CodigoIso2.Trim().ToUpper();
            entity.CodigoIso3 = model.CodigoIso3?.Trim().ToUpper();
            entity.Nombre = model.Nombre.Trim();

            // Estado inicial
            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                                  ? "ACTIVO"
                                  : model.Estado.ToUpper();
            entity.Eliminado = false;

            // Persistir
            await _uow.PaisRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return PaisDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(PaisDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo de país no puede ser nulo.");

            if (model.IdPais <= 0)
                throw new ArgumentException(
                    "El ID del país debe ser mayor a 0.");

            // Buscar entidad existente
            var entity = await _uow.PaisRepository.GetByIdAsync(model.IdPais);

            if (entity == null)
                return false;

            // Verificar unicidad ISO2 si cambió
            if (!string.IsNullOrWhiteSpace(model.CodigoIso2) &&
                model.CodigoIso2.ToUpper() != entity.CodigoIso2?.ToUpper())
            {
                if (model.CodigoIso2.Trim().Length != 2)
                    throw new ArgumentException(
                        "El código ISO2 debe tener exactamente 2 caracteres.");

                var conMismoIso2 = await _uow.PaisRepository
                                             .GetByCodigoIso2Async(
                                                 model.CodigoIso2.Trim().ToUpper());

                if (conMismoIso2 != null && conMismoIso2.IdPais != model.IdPais)
                    throw new InvalidOperationException(
                        $"Ya existe otro país con el código ISO2 '{model.CodigoIso2}'.");
            }

            // Verificar unicidad ISO3 si cambió
            if (!string.IsNullOrWhiteSpace(model.CodigoIso3) &&
                model.CodigoIso3.ToUpper() != entity.CodigoIso3?.ToUpper())
            {
                if (model.CodigoIso3.Trim().Length != 3)
                    throw new ArgumentException(
                        "El código ISO3 debe tener exactamente 3 caracteres.");

                var conMismoIso3 = await _uow.PaisRepository
                                             .GetByCodigoIso3Async(
                                                 model.CodigoIso3.Trim().ToUpper());

                if (conMismoIso3 != null && conMismoIso3.IdPais != model.IdPais)
                    throw new InvalidOperationException(
                        $"Ya existe otro país con el código ISO3 '{model.CodigoIso3}'.");
            }

            // Aplicar cambios mediante el mapper
            PaisDataMapper.UpdateEntity(entity, model);

            // Normalizar
            if (!string.IsNullOrWhiteSpace(entity.CodigoIso2))
                entity.CodigoIso2 = entity.CodigoIso2.ToUpper();

            if (!string.IsNullOrWhiteSpace(entity.CodigoIso3))
                entity.CodigoIso3 = entity.CodigoIso3.ToUpper();

            _uow.PaisRepository.Update(entity);
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
                    "El ID del país debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.PaisRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            // Soft delete via repositorio
            _uow.PaisRepository.Delete(entity);
            await _uow.SaveChangesAsync();

            return true;
        }
    }
}