using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IBoletoRepository
    {
        Task<IEnumerable<BoletoEntity>> GetAllAsync();

        Task<BoletoEntity> GetByIdAsync(int id);

        Task<IEnumerable<BoletoEntity>> GetByReservaAsync(int idReserva);

        Task<IEnumerable<BoletoEntity>> GetByVueloAsync(int idVuelo);

        Task<BoletoEntity> GetByCodigoAsync(string codigo);

        Task AddAsync(BoletoEntity boleto);

        void Update(BoletoEntity boleto);

        void Delete(BoletoEntity boleto);
    }
}