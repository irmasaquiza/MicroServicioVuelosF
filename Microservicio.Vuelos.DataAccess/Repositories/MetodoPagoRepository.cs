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
    public class MetodoPagoRepository : IMetodoPagoRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public MetodoPagoRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<MetodoPagoEntity>> GetAllAsync()
        {
            return await _context.MetodosPago
                .Where(m => !m.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<MetodoPagoEntity> GetByIdAsync(int id)
        {
            return await _context.MetodosPago
                .FirstOrDefaultAsync(m => m.IdMetodo == id && !m.EsEliminado);
        }

        // 🔍 Obtener por cliente
        public async Task<IEnumerable<MetodoPagoEntity>> GetByClienteAsync(int idCliente)
        {
            return await _context.MetodosPago
                .Where(m => m.IdCliente == idCliente && !m.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por tipo
        public async Task<IEnumerable<MetodoPagoEntity>> GetByTipoAsync(int idTipoMetodo)
        {
            return await _context.MetodosPago
                .Where(m => m.IdTipoMetodo == idTipoMetodo && !m.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener método principal del cliente
        public async Task<MetodoPagoEntity> GetPrincipalByClienteAsync(int idCliente)
        {
            return await _context.MetodosPago
                .FirstOrDefaultAsync(m =>
                    m.IdCliente == idCliente &&
                    m.EsPrincipal &&
                    !m.EsEliminado);
        }

        // 🔍 Buscar por token (pasarela)
        public async Task<MetodoPagoEntity> GetByTokenAsync(string token)
        {
            return await _context.MetodosPago
                .FirstOrDefaultAsync(m =>
                    m.TokenPasarela == token &&
                    !m.EsEliminado);
        }

        // ➕ Crear
        public async Task AddAsync(MetodoPagoEntity metodoPago)
        {
            await _context.MetodosPago.AddAsync(metodoPago);
        }

        // ✏️ Actualizar
        public void Update(MetodoPagoEntity metodoPago)
        {
            _context.MetodosPago.Update(metodoPago);
        }

        // ❌ Eliminación lógica
        public void Delete(MetodoPagoEntity metodoPago)
        {
            metodoPago.EsEliminado = true;
            _context.MetodosPago.Update(metodoPago);
        }
    }
}