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
    public class ClienteRepository : IClienteRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public ClienteRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🔍 Obtener todos
        public async Task<IEnumerable<ClienteEntity>> GetAllAsync()
        {
            return await _context.Clientes
                .Where(c => !c.EsEliminado)
                .ToListAsync();
        }

        // 🔍 Obtener por ID
        public async Task<ClienteEntity> GetByIdAsync(int id)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c => c.IdCliente == id && !c.EsEliminado);
        }

        // 🔍 Buscar por documento
        public async Task<ClienteEntity> GetByDocumentoAsync(string numeroDocumento)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.NumeroIdentificacion == numeroDocumento &&
                    !c.EsEliminado);
        }

        // 🔍 Buscar por email
        public async Task<ClienteEntity> GetByEmailAsync(string email)
        {
            return await _context.Clientes
                .FirstOrDefaultAsync(c =>
                    c.Correo == email &&
                    !c.EsEliminado);
        }

        // 🔍 Buscar por nombre
        public async Task<IEnumerable<ClienteEntity>> GetByNombreAsync(string nombre)
        {
            return await _context.Clientes
                .Where(c =>
                    (c.Nombres.Contains(nombre) || c.Apellidos.Contains(nombre)) &&
                    !c.EsEliminado)
                .ToListAsync();
        }

        // ➕ Crear
        public async Task AddAsync(ClienteEntity cliente)
        {
            await _context.Clientes.AddAsync(cliente);
        }

        // ✏️ Actualizar
        public void Update(ClienteEntity cliente)
        {
            _context.Clientes.Update(cliente);
        }

        // ❌ Eliminación lógica
        public void Delete(ClienteEntity cliente)
        {
            cliente.EsEliminado = true;
            _context.Clientes.Update(cliente);
        }
    }
}