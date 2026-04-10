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
    public class PasajeroRepository : IPasajeroRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public PasajeroRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<PasajeroEntity>> GetAllAsync()
        {
            return await _context.Pasajeros
                .Where(p => !p.EsEliminado)
                .ToListAsync();
        }

        public async Task<PasajeroEntity> GetByIdAsync(int id)
        {
            return await _context.Pasajeros
                .FirstOrDefaultAsync(p => p.IdPasajero == id && !p.EsEliminado);
        }

        public async Task<IEnumerable<PasajeroEntity>> GetByClienteAsync(int idCliente)
        {
            return await _context.Pasajeros
                .Where(p => p.IdCliente == idCliente && !p.EsEliminado)
                .ToListAsync();
        }

        public async Task<PasajeroEntity> GetByDocumentoAsync(string numeroDocumentoPasajero)
        {
            return await _context.Pasajeros
                .FirstOrDefaultAsync(p =>
                    p.NumeroDocumentoPasajero == numeroDocumentoPasajero &&
                    !p.EsEliminado);
        }

        public async Task<IEnumerable<PasajeroEntity>> GetByNombreAsync(string nombre, string apellido)
        {
            return await _context.Pasajeros
                .Where(p =>
                    p.NombrePasajero.Contains(nombre) &&
                    p.ApellidoPasajero.Contains(apellido) &&
                    !p.EsEliminado)
                .ToListAsync();
        }

        public async Task AddAsync(PasajeroEntity pasajero)
        {
            await _context.Pasajeros.AddAsync(pasajero);
        }

        public void Update(PasajeroEntity pasajero)
        {
            _context.Pasajeros.Update(pasajero);
        }

        public void Delete(PasajeroEntity pasajero)
        {
            pasajero.EsEliminado = true;
            _context.Pasajeros.Update(pasajero);
        }
    }
}
