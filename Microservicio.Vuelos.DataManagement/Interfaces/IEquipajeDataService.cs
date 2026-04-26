using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IEquipajeDataService
    {
        // 🔍 Obtener todos
        Task<IEnumerable<EquipajeDataModel>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<EquipajeDataModel> GetByIdAsync(int id);

        // 🔍 Obtener por boleto (CLAVE 💀)
        Task<IEnumerable<EquipajeDataModel>> GetByBoletoAsync(int idBoleto);

        // 🔍 Obtener por tipo (MANO / BODEGA)
        Task<IEnumerable<EquipajeDataModel>> GetByTipoAsync(string tipo);

        // 🔍 Búsqueda paginada con filtros
        Task<DataPagedResult<EquipajeDataModel>> GetPagedAsync(EquipajeFiltroDataModel filtro);

        // ➕ Crear
        Task<EquipajeDataModel> CreateAsync(EquipajeDataModel model);

        // ✏️ Actualizar
        Task<bool> UpdateAsync(EquipajeDataModel model);

        Task<decimal> SumPrecioByBoletoAsync(int idBoleto);

        // ❌ Eliminación lógica
        Task<bool> DeleteAsync(int id);
    }
}
