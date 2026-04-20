using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataManagement.Models;

namespace Microservicio.Vuelos.DataManagement.Interfaces
{
    public interface IVueloDataService
    {
        Task<IEnumerable<VueloDataModel>> GetAllAsync();

        Task<VueloDataModel> GetByIdAsync(int id);

        Task<DataPagedResult<VueloDataModel>> GetPagedAsync(VueloFiltroDataModel filtro);

        Task<VueloDataModel> CreateAsync(VueloDataModel model);

        Task<bool> UpdateAsync(VueloDataModel model);

        Task<bool> DeleteAsync(int id);
    }
}