using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Interfaces;
using Microservicio.Vuelos.DataManagement.Mappers;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Services
{
    public class ClienteDataService : IClienteDataService
    {
        private readonly IUnitOfWork _uow;

        // ── Valores válidos según CHECKs de la BD ───────
        private static readonly string[] TiposIdentificacionValidos =
        {
            "CEDULA",
            "PASAPORTE",
            "RUC",
            "TARJETA_IDENTIDAD",
            "OTRO"
        };

        private static readonly string[] EstadosValidos =
        {
            "ACT",
            "INA"
        };

        private static readonly string[] GenerosValidos =
        {
            "MASCULINO",
            "FEMENINO",
            "OTRO"
        };

        public ClienteDataService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ─────────────────────────────────────────────
        // GET ALL
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ClienteDataModel>> GetAllAsync()
        {
            var entities = await _uow.ClienteRepository.GetAllAsync();

            if (entities == null || !entities.Any())
                return Enumerable.Empty<ClienteDataModel>();

            return ClienteDataMapper.ToDataModelList(entities);
        }

        // ─────────────────────────────────────────────
        // GET BY ID
        // ─────────────────────────────────────────────
        public async Task<ClienteDataModel> GetByIdAsync(int id)
        {
            if (id <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.ClienteRepository.GetByIdAsync(id);

            if (entity == null)
                return null;

            return ClienteDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY IDENTIFICACION
        // ─────────────────────────────────────────────
        public async Task<ClienteDataModel> GetByIdentificacionAsync(
            string numeroIdentificacion)
        {
            if (string.IsNullOrWhiteSpace(numeroIdentificacion))
                throw new ArgumentException(
                    "El número de identificación no puede estar vacío.",
                    nameof(numeroIdentificacion));

            var entity = await _uow.ClienteRepository
                                   .GetByDocumentoAsync(numeroIdentificacion.Trim());

            if (entity == null)
                return null;

            return ClienteDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY CORREO
        // ─────────────────────────────────────────────
        public async Task<ClienteDataModel> GetByCorreoAsync(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                throw new ArgumentException(
                    "El correo no puede estar vacío.",
                    nameof(correo));

            var entity = await _uow.ClienteRepository
                                   .GetByEmailAsync(correo.Trim().ToLower());

            if (entity == null)
                return null;

            return ClienteDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // GET BY PAIS
        // ─────────────────────────────────────────────
        public async Task<IEnumerable<ClienteDataModel>> GetByPaisAsync(int idPais)
        {
            if (idPais <= 0)
                throw new ArgumentException(
                    "El ID del país debe ser mayor a 0.",
                    nameof(idPais));

            // El repositorio no tiene GetByPais directamente
            // filtramos desde GetAll
            var todos = await _uow.ClienteRepository.GetAllAsync();

            var filtrados = todos
                .Where(c => c.IdPaisNacionalidad == idPais)
                .ToList();

            if (!filtrados.Any())
                return Enumerable.Empty<ClienteDataModel>();

            return ClienteDataMapper.ToDataModelList(filtrados);
        }

        // ─────────────────────────────────────────────
        // GET PAGED — con filtros
        // ─────────────────────────────────────────────
        public async Task<DataPagedResult<ClienteDataModel>> GetPagedAsync(
            ClienteFiltroDataModel filtro)
        {
            if (filtro == null)
                throw new ArgumentNullException(
                    nameof(filtro),
                    "El filtro no puede ser nulo.");

            // Asegurar paginación válida
            if (filtro.Page <= 0) filtro.Page = 1;
            if (filtro.PageSize <= 0) filtro.PageSize = 20;

            // Traer todos los no eliminados
            var todos = await _uow.ClienteRepository.GetAllAsync();

            // ── Aplicar filtros en memoria ──────────────────
            var query = todos.AsQueryable();

            if (!string.IsNullOrWhiteSpace(filtro.TipoIdentificacion))
                query = query.Where(c =>
                    c.TipoIdentificacion.ToUpper() ==
                    filtro.TipoIdentificacion.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.NumeroIdentificacion))
                query = query.Where(c =>
                    c.NumeroIdentificacion.Contains(
                        filtro.NumeroIdentificacion.Trim()));

            if (!string.IsNullOrWhiteSpace(filtro.Nombres))
                query = query.Where(c =>
                    c.Nombres != null &&
                    c.Nombres.ToUpper().Contains(filtro.Nombres.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Apellidos))
                query = query.Where(c =>
                    c.Apellidos != null &&
                    c.Apellidos.ToUpper().Contains(filtro.Apellidos.ToUpper()));

            if (!string.IsNullOrWhiteSpace(filtro.Correo))
                query = query.Where(c =>
                    c.Correo != null &&
                    c.Correo.ToUpper().Contains(filtro.Correo.ToUpper()));

            if (filtro.IdCiudadResidencia.HasValue)
                query = query.Where(c =>
                    c.IdCiudadResidencia == filtro.IdCiudadResidencia.Value);

            if (filtro.IdPaisNacionalidad.HasValue)
                query = query.Where(c =>
                    c.IdPaisNacionalidad == filtro.IdPaisNacionalidad.Value);

            if (!string.IsNullOrWhiteSpace(filtro.Estado))
                query = query.Where(c =>
                    c.Estado.ToUpper() == filtro.Estado.ToUpper());

            if (!string.IsNullOrWhiteSpace(filtro.ServicioOrigen))
                query = query.Where(c =>
                    c.ServicioOrigen != null &&
                    c.ServicioOrigen.ToUpper() == filtro.ServicioOrigen.ToUpper());

            // ── Filtro por rango de fecha de nacimiento ─────
            if (filtro.FechaNacimientoInicio.HasValue)
                query = query.Where(c =>
                    c.FechaNacimiento.HasValue &&
                    c.FechaNacimiento.Value >= filtro.FechaNacimientoInicio.Value);

            if (filtro.FechaNacimientoFin.HasValue)
                query = query.Where(c =>
                    c.FechaNacimiento.HasValue &&
                    c.FechaNacimiento.Value <= filtro.FechaNacimientoFin.Value);

            // Ordenar por apellidos + nombres
            query = query.OrderBy(c => c.Apellidos)
                         .ThenBy(c => c.Nombres);

            // ── Paginación ──────────────────────────────────
            var total = query.Count();
            var totalPages = (int)Math.Ceiling(total / (double)filtro.PageSize);

            var items = query
                .Skip((filtro.Page - 1) * filtro.PageSize)
                .Take(filtro.PageSize)
                .Select(ClienteDataMapper.ToDataModel)
                .ToList();

            return new DataPagedResult<ClienteDataModel>
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
        public async Task<ClienteDataModel> CreateAsync(ClienteDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo del cliente no puede ser nulo.");

            // ── Validaciones obligatorias ───────────────────
            if (string.IsNullOrWhiteSpace(model.TipoIdentificacion))
                throw new ArgumentException(
                    "El tipo de identificación es obligatorio.");

            if (!TiposIdentificacionValidos.Contains(
                    model.TipoIdentificacion.ToUpper()))
                throw new ArgumentException(
                    $"Tipo de identificación inválido. " +
                    $"Los válidos son: {string.Join(", ", TiposIdentificacionValidos)}");

            if (string.IsNullOrWhiteSpace(model.NumeroIdentificacion))
                throw new ArgumentException(
                    "El número de identificación es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Correo))
                throw new ArgumentException(
                    "El correo es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Telefono))
                throw new ArgumentException(
                    "El teléfono es obligatorio.");

            if (string.IsNullOrWhiteSpace(model.Direccion))
                throw new ArgumentException(
                    "La dirección es obligatoria.");

            if (model.IdCiudadResidencia <= 0)
                throw new ArgumentException(
                    "El ID de la ciudad de residencia es obligatorio.");

            if (model.IdPaisNacionalidad <= 0)
                throw new ArgumentException(
                    "El ID del país de nacionalidad es obligatorio.");

            // ── Validación CHK_CLIENTES_PERSONA_EMPRESA ─────
            // Si es RUC → RazonSocial obligatoria
            // Si no es RUC → Nombres obligatorio
            ValidarPersonaEmpresa(model);

            // ── Validación formato correo ───────────────────
            ValidarFormatoCorreo(model.Correo);

            // ── Validación género si viene ──────────────────
            if (!string.IsNullOrWhiteSpace(model.Genero) &&
                !GenerosValidos.Contains(model.Genero.ToUpper()))
                throw new ArgumentException(
                    $"Género inválido. " +
                    $"Los válidos son: {string.Join(", ", GenerosValidos)}");

            // ── Validación fecha de nacimiento ──────────────
            if (model.FechaNacimiento.HasValue &&
                model.FechaNacimiento.Value > DateTime.Today)
                throw new ArgumentException(
                    "La fecha de nacimiento no puede ser una fecha futura.");

            // ── Verificar unicidad número de identificación ─
            var existenteDoc = await _uow.ClienteRepository
                                         .GetByDocumentoAsync(
                                             model.NumeroIdentificacion.Trim());

            if (existenteDoc != null)
                throw new InvalidOperationException(
                    $"Ya existe un cliente con la identificación " +
                    $"'{model.NumeroIdentificacion}'.");

            // ── Verificar unicidad correo ───────────────────
            var existenteCorreo = await _uow.ClienteRepository
                                            .GetByEmailAsync(
                                                model.Correo.Trim().ToLower());

            if (existenteCorreo != null)
                throw new InvalidOperationException(
                    $"Ya existe un cliente con el correo '{model.Correo}'.");

            // ── Verificar que ciudad exista ─────────────────
            var ciudad = await _uow.CiudadRepository
                                   .GetByIdAsync(model.IdCiudadResidencia);

            if (ciudad == null)
                throw new InvalidOperationException(
                    $"No existe una ciudad con ID '{model.IdCiudadResidencia}'.");

            // ── Verificar que país exista ───────────────────
            var pais = await _uow.PaisRepository
                                 .GetByIdAsync(model.IdPaisNacionalidad);

            if (pais == null)
                throw new InvalidOperationException(
                    $"No existe un país con ID '{model.IdPaisNacionalidad}'.");

            // ── Construir entidad ───────────────────────────
            var entity = ClienteDataMapper.ToEntity(model);

            // Campos generados internamente
            entity.ClienteGuid = Guid.NewGuid();
            entity.FechaRegistroUtc = DateTime.UtcNow;
            entity.CreadoPorUsuario = "SYSTEM";
            entity.EsEliminado = false;

            // Estado inicial
            entity.Estado = string.IsNullOrWhiteSpace(model.Estado)
                ? "ACT"
                : model.Estado.ToUpper();

            // Servicio origen
            entity.ServicioOrigen = string.IsNullOrWhiteSpace(model.ServicioOrigen)
                ? "VUELOS"
                : model.ServicioOrigen.ToUpper();

            // Normalizar correo
            entity.Correo = model.Correo.Trim().ToLower();

            // Normalizar tipo identificación
            entity.TipoIdentificacion = model.TipoIdentificacion.ToUpper();

            // Normalizar género si viene
            if (!string.IsNullOrWhiteSpace(model.Genero))
                entity.Genero = model.Genero.ToUpper();

            // Persistir
            await _uow.ClienteRepository.AddAsync(entity);
            await _uow.SaveChangesAsync();

            return ClienteDataMapper.ToDataModel(entity);
        }

        // ─────────────────────────────────────────────
        // UPDATE
        // ─────────────────────────────────────────────
        public async Task<bool> UpdateAsync(ClienteDataModel model)
        {
            if (model == null)
                throw new ArgumentNullException(
                    nameof(model),
                    "El modelo del cliente no puede ser nulo.");

            if (model.IdCliente <= 0)
                throw new ArgumentException(
                    "El ID del cliente debe ser mayor a 0.");

            // Buscar entidad existente
            var entity = await _uow.ClienteRepository.GetByIdAsync(model.IdCliente);

            if (entity == null)
                return false;

            // No permitir modificar un cliente inhabilitado
            if (entity.Estado == "INA")
                throw new InvalidOperationException(
                    "No se puede modificar un cliente inhabilitado.");

            // ── Validar persona/empresa si cambió tipo ──────
            ValidarPersonaEmpresa(model);

            // ── Validar formato correo si cambió ───────────
            if (!string.IsNullOrWhiteSpace(model.Correo))
                ValidarFormatoCorreo(model.Correo);

            // ── Validar género si viene ─────────────────────
            if (!string.IsNullOrWhiteSpace(model.Genero) &&
                !GenerosValidos.Contains(model.Genero.ToUpper()))
                throw new ArgumentException(
                    $"Género inválido. " +
                    $"Los válidos son: {string.Join(", ", GenerosValidos)}");

            // ── Validar estado si viene ─────────────────────
            if (!string.IsNullOrWhiteSpace(model.Estado) &&
                !EstadosValidos.Contains(model.Estado.ToUpper()))
                throw new ArgumentException(
                    $"Estado inválido. " +
                    $"Los válidos son: {string.Join(", ", EstadosValidos)}");

            // ── Verificar unicidad identificación si cambió ─
            if (!string.IsNullOrWhiteSpace(model.NumeroIdentificacion) &&
                model.NumeroIdentificacion.Trim() != entity.NumeroIdentificacion)
            {
                var existenteDoc = await _uow.ClienteRepository
                                             .GetByDocumentoAsync(
                                                 model.NumeroIdentificacion.Trim());

                if (existenteDoc != null &&
                    existenteDoc.IdCliente != model.IdCliente)
                    throw new InvalidOperationException(
                        $"Ya existe otro cliente con la identificación " +
                        $"'{model.NumeroIdentificacion}'.");
            }

            // ── Verificar unicidad correo si cambió ─────────
            if (!string.IsNullOrWhiteSpace(model.Correo) &&
                model.Correo.Trim().ToLower() != entity.Correo?.ToLower())
            {
                var existenteCorreo = await _uow.ClienteRepository
                                                .GetByEmailAsync(
                                                    model.Correo.Trim().ToLower());

                if (existenteCorreo != null &&
                    existenteCorreo.IdCliente != model.IdCliente)
                    throw new InvalidOperationException(
                        $"Ya existe otro cliente con el correo '{model.Correo}'.");
            }

            // ── Verificar que ciudad exista si cambió ───────
            if (model.IdCiudadResidencia > 0 &&
                model.IdCiudadResidencia != entity.IdCiudadResidencia)
            {
                var ciudad = await _uow.CiudadRepository
                                       .GetByIdAsync(model.IdCiudadResidencia);

                if (ciudad == null)
                    throw new InvalidOperationException(
                        $"No existe una ciudad con ID '{model.IdCiudadResidencia}'.");
            }

            // ── Verificar que país exista si cambió ─────────
            if (model.IdPaisNacionalidad > 0 &&
                model.IdPaisNacionalidad != entity.IdPaisNacionalidad)
            {
                var pais = await _uow.PaisRepository
                                     .GetByIdAsync(model.IdPaisNacionalidad);

                if (pais == null)
                    throw new InvalidOperationException(
                        $"No existe un país con ID '{model.IdPaisNacionalidad}'.");
            }

            // Aplicar cambios — UpdateEntity NO toca:
            // IdCliente, ClienteGuid
            ClienteDataMapper.UpdateEntity(entity, model);

            // Normalizar
            if (!string.IsNullOrWhiteSpace(entity.Correo))
                entity.Correo = entity.Correo.Trim().ToLower();

            if (!string.IsNullOrWhiteSpace(entity.TipoIdentificacion))
                entity.TipoIdentificacion = entity.TipoIdentificacion.ToUpper();

            if (!string.IsNullOrWhiteSpace(entity.Genero))
                entity.Genero = entity.Genero.ToUpper();

            // Auditoría de modificación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";

            _uow.ClienteRepository.Update(entity);
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
                    "El ID del cliente debe ser mayor a 0.",
                    nameof(id));

            var entity = await _uow.ClienteRepository.GetByIdAsync(id);

            if (entity == null)
                return false;

            // Soft delete via repositorio
            _uow.ClienteRepository.Delete(entity);

            // Auditoría de eliminación
            entity.FechaModificacionUtc = DateTime.UtcNow;
            entity.ModificadoPorUsuario = "SYSTEM";
            entity.FechaInhabilitacionUtc = DateTime.UtcNow;
            entity.MotivoInhabilitacion = "Eliminación lógica del registro.";

            await _uow.SaveChangesAsync();

            return true;
        }

        // ─────────────────────────────────────────────
        // PRIVADOS — Validaciones de negocio
        // ─────────────────────────────────────────────

        /// <summary>
        /// Respeta el CHECK CHK_CLIENTES_PERSONA_EMPRESA de la BD:
        /// Si tipo = RUC  → RazonSocial obligatoria
        /// Si tipo != RUC → Nombres obligatorio
        /// </summary>
        private static void ValidarPersonaEmpresa(ClienteDataModel model)
        {
            var tipo = model.TipoIdentificacion?.ToUpper();

            if (tipo == "RUC")
            {
                if (string.IsNullOrWhiteSpace(model.RazonSocial))
                    throw new ArgumentException(
                        "La razón social es obligatoria para clientes con RUC.");
            }
            else
            {
                if (string.IsNullOrWhiteSpace(model.Nombres))
                    throw new ArgumentException(
                        "El nombre es obligatorio para personas naturales.");
            }
        }

        /// <summary>
        /// Respeta el CHECK CHK_Cliente_CorreoContacto_Formato de la BD:
        /// correo LIKE '%@%._%'
        /// </summary>
        private static void ValidarFormatoCorreo(string correo)
        {
            if (string.IsNullOrWhiteSpace(correo))
                return;

            var trimmed = correo.Trim();

            var tieneArroba = trimmed.Contains("@");
            var partes = trimmed.Split('@');
            var tieneDominio = partes.Length == 2 &&
                                partes[1].Contains(".") &&
                                partes[1].Length > 2;

            if (!tieneArroba || !tieneDominio)
                throw new ArgumentException(
                    $"El formato del correo '{correo}' no es válido.");
        }
    }
}