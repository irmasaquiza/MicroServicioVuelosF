/*using System;
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
    public class TipoMetodoPagoRepository : ITipoMetodoPagoRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public TipoMetodoPagoRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<TipoMetodoPagoEntity>> GetAllAsync()
        {
            return await _context.TiposMetodoPago
                .Where(t => !t.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<TipoMetodoPagoEntity?> GetByIdAsync(int id)
        {
            return await _context.TiposMetodoPago
                .FirstOrDefaultAsync(t =>
                    t.IdTipoMetodo == id &&
                    !t.EsEliminado);
        }

        // 🔍 Buscar por nombre exacto
        public async Task<TipoMetodoPagoEntity?> GetByNombreExactoAsync(string nombre)
        {
            return await _context.TiposMetodoPago
                .FirstOrDefaultAsync(t =>
                    t.NombreTipo == nombre &&
                    !t.EsEliminado);
        }

        // 🔍 Buscar por nombre parcial
        public async Task<IEnumerable<TipoMetodoPagoEntity>> GetByNombreAsync(string nombre)
        {
            return await _context.TiposMetodoPago
                .Where(t =>
                    t.NombreTipo.Contains(nombre) &&
                    !t.EsEliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(TipoMetodoPagoEntity tipoMetodo)
        {
            await _context.TiposMetodoPago.AddAsync(tipoMetodo);
        }

        // ✏️ Actualizar
        public void Update(TipoMetodoPagoEntity tipoMetodo)
        {
            _context.TiposMetodoPago.Update(tipoMetodo);
        }

        // ❌ Eliminación lógica
        public void Delete(TipoMetodoPagoEntity tipoMetodo)
        {
            tipoMetodo.EsEliminado = true;
            _context.TiposMetodoPago.Update(tipoMetodo);
        }
    }
}*/