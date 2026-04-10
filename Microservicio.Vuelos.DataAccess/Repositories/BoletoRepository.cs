using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Entities;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;

namespace Microservicio.Vuelos.DataAccess.Repositories
{
    public class BoletoRepository : IBoletoRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public BoletoRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<BoletoEntity>> GetAllAsync()
        {
            return await _context.Boletos
                .Where(b => !b.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<BoletoEntity> GetByIdAsync(int id)
        {
            return await _context.Boletos
                .FirstOrDefaultAsync(b => b.IdBoleto == id && !b.EsEliminado);
        }

        // 🔍 Obtener por reserva
        public async Task<IEnumerable<BoletoEntity>> GetByReservaAsync(int idReserva)
        {
            return await _context.Boletos
                .Where(b => b.IdReserva == idReserva && !b.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por vuelo
        public async Task<IEnumerable<BoletoEntity>> GetByVueloAsync(int idVuelo)
        {
            return await _context.Boletos
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Buscar por código
        public async Task<BoletoEntity> GetByCodigoAsync(string codigo)
        {
            return await _context.Boletos
                .FirstOrDefaultAsync(b => b.CodigoBoleto == codigo && !b.EsEliminado);
        }

        // ➕ Crear
        public async Task AddAsync(BoletoEntity boleto)
        {
            await _context.Boletos.AddAsync(boleto);
        }

        // ✏️ Actualizar
        public void Update(BoletoEntity boleto)
        {
            _context.Boletos.Update(boleto);
        }

        // ❌ Eliminación lógica (CORRECTO SEGÚN TU ENTITY)
        public void Delete(BoletoEntity boleto)
        {
            boleto.EsEliminado = true;
            _context.Boletos.Update(boleto);
        }
    }
}