using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IEquipajeRepository
    {
        Task<IEnumerable<EquipajeEntity>> GetAllAsync();

        Task<EquipajeEntity> GetByIdAsync(int id);

        Task<IEnumerable<EquipajeEntity>> GetByBoletoAsync(int idBoleto);

        Task AddAsync(EquipajeEntity equipaje);

        void Update(EquipajeEntity equipaje);

        void Delete(EquipajeEntity equipaje);
    }
}