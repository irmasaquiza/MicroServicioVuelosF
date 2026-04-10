using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IPasajeroRepository
    {
        Task<IEnumerable<PasajeroEntity>> GetAllAsync();

        Task<PasajeroEntity> GetByIdAsync(int id);

        Task<IEnumerable<PasajeroEntity>> GetByClienteAsync(int idCliente);

        // 🔍 CORREGIDO
        Task<PasajeroEntity> GetByDocumentoAsync(string numeroDocumentoPasajero);

        // 🔍 CORREGIDO
        Task<IEnumerable<PasajeroEntity>> GetByNombreAsync(string nombre, string apellido);

        Task AddAsync(PasajeroEntity pasajero);

        void Update(PasajeroEntity pasajero);

        void Delete(PasajeroEntity pasajero);
    }
}