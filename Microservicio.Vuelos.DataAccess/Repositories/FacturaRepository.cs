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
    public class FacturaRepository : IFacturaRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public FacturaRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todas
        public async Task<IEnumerable<FacturaEntity>> GetAllAsync()
        {
            return await _context.Facturas
                .Where(f => !f.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<FacturaEntity> GetByIdAsync(int id)
        {
            return await _context.Facturas
                .FirstOrDefaultAsync(f => f.IdFactura == id && !f.EsEliminado);
        }

        // 🔍 Buscar por número de factura
        public async Task<FacturaEntity> GetByNumeroAsync(string numeroFactura)
        {
            return await _context.Facturas
                .FirstOrDefaultAsync(f =>
                    f.NumeroFactura == numeroFactura &&
                    !f.EsEliminado);
        }

        // 🔍 Obtener por cliente
        public async Task<IEnumerable<FacturaEntity>> GetByClienteAsync(int idCliente)
        {
            return await _context.Facturas
                .Where(f => f.IdCliente == idCliente && !f.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por reserva
        public async Task<IEnumerable<FacturaEntity>> GetByReservaAsync(int idReserva)
        {
            return await _context.Facturas
                .Where(f => f.IdReserva == idReserva && !f.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por estado
        public async Task<IEnumerable<FacturaEntity>> GetByEstadoAsync(string estado)
        {
            return await _context.Facturas
                .Where(f => f.Estado == estado && !f.EsEliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(FacturaEntity factura)
        {
            await _context.Facturas.AddAsync(factura);
        }

        // ✏️ Actualizar
        public void Update(FacturaEntity factura)
        {
            _context.Facturas.Update(factura);
        }

        // ❌ Eliminación lógica
        public void Delete(FacturaEntity factura)
        {
            factura.EsEliminado = true;
            _context.Facturas.Update(factura);
        }
    }
}
