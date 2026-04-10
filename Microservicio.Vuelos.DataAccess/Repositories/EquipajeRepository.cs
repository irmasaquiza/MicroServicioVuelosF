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
    public class EquipajeRepository : IEquipajeRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public EquipajeRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<EquipajeEntity>> GetAllAsync()
        {
            return await _context.Equipajes
                .Where(e => !e.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<EquipajeEntity> GetByIdAsync(int id)
        {
            return await _context.Equipajes
                .FirstOrDefaultAsync(e => e.IdEquipaje == id && !e.EsEliminado);
        }

        // 🔍 Obtener por boleto
        public async Task<IEnumerable<EquipajeEntity>> GetByBoletoAsync(int idBoleto)
        {
            return await _context.Equipajes
                .Where(e => e.IdBoleto == idBoleto && !e.EsEliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(EquipajeEntity equipaje)
        {
            await _context.Equipajes.AddAsync(equipaje);
        }

        // ✏️ Actualizar
        public void Update(EquipajeEntity equipaje)
        {
            _context.Equipajes.Update(equipaje);
        }

        // ❌ Eliminación lógica
        public void Delete(EquipajeEntity equipaje)
        {
            equipaje.EsEliminado = true;
            _context.Equipajes.Update(equipaje);
        }
    }
}