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
        // 🔍 Obtener todos
        Task<IEnumerable<BoletoEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<BoletoEntity> GetByIdAsync(int id);

        // 🔍 Obtener boletos por reserva
        Task<IEnumerable<BoletoEntity>> GetByReservaAsync(int idReserva);

        // 🔍 Obtener boletos por vuelo
        Task<IEnumerable<BoletoEntity>> GetByVueloAsync(int idVuelo);

        // 🔍 Obtener boletos por pasajero
        Task<IEnumerable<BoletoEntity>> GetByPasajeroAsync(int idPasajero);

        // 🔍 Buscar por código de boleto (ej: ETKT123456)
        Task<BoletoEntity> GetByCodigoAsync(string codigo);

        // ➕ Crear
        Task AddAsync(BoletoEntity boleto);

        // ✏️ Actualizar
        void Update(BoletoEntity boleto);

        // ❌ Eliminación lógica
        void Delete(BoletoEntity boleto);
    }
}