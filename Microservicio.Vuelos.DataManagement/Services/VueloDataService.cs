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
            "ATERRIZADO", "CANCELADO", "DEMORADO"
        };

        public VueloDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ============================================================
        // GET ALL
        // ============================================================
        public async Task<IEnumerable<VueloDataModel>> GetAllAsync()
        {
            var entities = await _uow.VueloRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<VueloDataModel>();

            return entities.Select(VueloDataMapper.ToDataModel);
        }

        // ============================================================
        // GET BY ID
        // ============================================================
        public async Task<VueloDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException("ID inválido");

            var entity = await _uow.VueloRepository.GetByIdAsync(id);

            return entity == null ? null : VueloDataMapper.ToDataModel(entity);
        }

        // ============================================================
        // FILTRO SIMPLE (LIMPIO)
        // ============================================================
        public async Task<DataPagedResult<VueloDataModel>> GetPagedAsync(
            VueloFiltroDataModel filtro)
        {
            var todos = await _uow.VueloRepository.GetAllAsync();
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.CodigoVuelo))
                query = query.Where(v =>
                    v.CodigoVuelo.Contains(filtro.CodigoVuelo));

            if (filtro.IdAeropuertoOrigen.HasValue)
                query = query.Where(v =>
                    v.IdAeropuertoOrigen == filtro.IdAeropuertoOrigen);

            if (filtro.IdAeropuertoDestino.HasValue)
                query = query.Where(v =>
                    v.IdAeropuertoDestino == filtro.IdAeropuertoDestino);

            if (!string.IsNullOrWhiteSpace(filtro.EstadoVuelo))
                query = query.Where(v =>
                    v.EstadoVuelo == filtro.EstadoVuelo);

            if (filtro.PrecioMin.HasValue)
                query = query.Where(v =>
                    v.PrecioBase >= filtro.PrecioMin);

            if (filtro.PrecioMax.HasValue)
                query = query.Where(v =>
                    v.PrecioBase <= filtro.PrecioMax);

            query = query.OrderBy(v => v.FechaHoraSalida);

            var total = query.Count();

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
                    TotalPages = (int)Math.Ceiling(total / (double)filtro.PageSize)
                }
            };
        }

        // ============================================================
        // CREATE
        // ============================================================
        public async Task<VueloDataModel> CreateAsync(VueloDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(nameof(model));

            if (model.IdAeropuertoOrigen == model.IdAeropuertoDestino)
                throw new Exception("Origen y destino no pueden ser iguales");

            if (model.FechaHoraLlegada <= model.FechaHoraSalida)
                throw new Exception("Fechas inválidas");

            var entity = VueloDataMapper.ToEntity(model);

            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;
            entity.Estado = "ACTIVO";

            entity.EstadoVuelo = string.IsNullOrWhiteSpace(model.EstadoVuelo)
                ? "PROGRAMADO"
                : model.EstadoVuelo;

            await _uow.VueloRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return VueloDataMapper.ToDataModel(entity);
        }

        // ============================================================
        // UPDATE
        // ============================================================
        public async Task<bool> UpdateAsync(VueloDataModel model)
        {
            var entity = await _uow.VueloRepository.GetByIdAsync(model.IdVuelo);

            if (entity == null)
                return false;

            VueloDataMapper.UpdateEntity(entity, model);

            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.VueloRepository.Update(entity);
            await _uow.SaveChangesAsync();

            return true;
        }

        // ============================================================
        // DELETE
        // ============================================================
        public async Task<bool> DeleteAsync(int id)
        {
            var entity = await _uow.VueloRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            entity.EsEliminado = true;
            entity.FechaModificacionUtc = DateTime.UtcNow;

            await _uow.SaveChangesAsync();

            return true;
        }
    }
}