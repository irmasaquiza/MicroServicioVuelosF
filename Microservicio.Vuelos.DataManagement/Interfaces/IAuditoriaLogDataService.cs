using System;
using System.Collections.Generic;
using System.Text;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IAuditoriaLogDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<AuditoriaLogDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<AuditoriaLogDataModel> GetByIdAsync(long id);

        // 🔍 Filtros específicos de auditoría
        Task<IEnumerable<AuditoriaLogDataModel>> GetByTablaAsync(string tabla);

        Task<IEnumerable<AuditoriaLogDataModel>> GetByOperacionAsync(string operacion);

        Task<IEnumerable<AuditoriaLogDataModel>> GetByUsuarioAsync(string usuario);

        Task<IEnumerable<AuditoriaLogDataModel>> GetByFechaAsync(DateTime fechaInicio, DateTime fechaFin);

        // 🔍 Búsqueda paginada (importante 💀)
        Task<DataPagedResult<AuditoriaLogDataModel>> GetPagedAsync(AuditoriaLogFiltroDataModel filtro);

        // ➕ Registrar auditoría (único "write")
        Task<AuditoriaLogDataModel> CreateAsync(AuditoriaLogDataModel model);
    }
}