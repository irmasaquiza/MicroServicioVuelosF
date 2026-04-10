using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class ReservaRepository : IReservaRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public ReservaRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todas
        public async Task<IEnumerable<ReservaEntity>> GetAllAsync()
        {
            return await _context.Reservas
                .Where(r => !r.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<ReservaEntity> GetByIdAsync(int id)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(r => r.IdReserva == id && !r.EsEliminado);
        }

        // 🔍 Obtener por GUID
        public async Task<ReservaEntity> GetByGuidAsync(Guid reservaGuid)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(r => r.GuidReserva == reservaGuid && !r.EsEliminado);
        }

        // 🔍 Obtener por código (PNR)
        public async Task<ReservaEntity> GetByCodigoAsync(string codigoReserva)
        {
            return await _context.Reservas
                .FirstOrDefaultAsync(r =>
                    r.CodigoReserva == codigoReserva &&
                    !r.EsEliminado);
        }

        // 🔍 Obtener por cliente
        public async Task<IEnumerable<ReservaEntity>> GetByClienteAsync(int idCliente)
        {
            return await _context.Reservas
                .Where(r => r.IdCliente == idCliente && !r.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por pasajero
        public async Task<IEnumerable<ReservaEntity>> GetByPasajeroAsync(int idPasajero)
        {
            return await _context.Reservas
                .Where(r => r.IdPasajero == idPasajero && !r.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por estado
        public async Task<IEnumerable<ReservaEntity>> GetByEstadoAsync(string estadoReserva)
        {
            return await _context.Reservas
                .Where(r => r.EstadoReserva == estadoReserva && !r.EsEliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(ReservaEntity reserva)
        {
            await _context.Reservas.AddAsync(reserva);
        }

        // ✏️ Actualizar
        public void Update(ReservaEntity reserva)
        {
            _context.Reservas.Update(reserva);
        }

        // ❌ Eliminación lógica
        public void Delete(ReservaEntity reserva)
        {
            reserva.EsEliminado = true;
            _context.Reservas.Update(reserva);
        }
    }
}
