/*using System;
using System.Collections.Generic;
using System.Text;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Repositories.Interfaces
{
    public interface IMetodoPagoRepository
    {
        // 🔍 Obtener todos
        Task<IEnumerable<MetodoPagoEntity>> GetAllAsync();

        // 🔍 Obtener por ID
        Task<MetodoPagoEntity> GetByIdAsync(int id);

        // 🔍 Obtener métodos por cliente
        Task<IEnumerable<MetodoPagoEntity>> GetByClienteAsync(int idCliente);

        // 🔍 Obtener métodos por tipo (tarjeta, paypal, etc.)
        Task<IEnumerable<MetodoPagoEntity>> GetByTipoAsync(int idTipoMetodo);

        // 🔍 Obtener método principal del cliente
        Task<MetodoPagoEntity> GetPrincipalByClienteAsync(int idCliente);

        // 🔍 Buscar por token (pasarela de pago)
        Task<MetodoPagoEntity> GetByTokenAsync(string token);

        // ➕ Crear
        Task AddAsync(MetodoPagoEntity metodoPago);

        // ✏️ Actualizar
        void Update(MetodoPagoEntity metodoPago);

        // ❌ Eliminación lógica
        void Delete(MetodoPagoEntity metodoPago);
    }
}*/