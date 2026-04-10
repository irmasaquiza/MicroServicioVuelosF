using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microservicio.Vuelos.DataAccess.Context;

namespace Microservicio.Vuelos.DataAccess.Queries
{
    public class AsientoQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public AsientoQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🧾 Mapa de asientos por vuelo
        public async Task<IEnumerable<object>> GetMapaAsientosAsync(int idVuelo)
        {
            return await _context.Asientos
                .Where(a => a.IdVuelo == idVuelo && !a.Eliminado)
                .Select(a => new
                {
                    a.IdAsiento,
                    a.NumeroAsiento,
                    a.Clase,
                    a.Disponible,
                    a.Posicion
                })
                .ToListAsync();
        }

        // 🟢 Asientos disponibles
        public async Task<IEnumerable<object>> GetDisponiblesAsync(int idVuelo)
        {
            return await _context.Asientos
                .Where(a =>
                    a.IdVuelo == idVuelo &&
                    a.Disponible &&
                    !a.Eliminado)
                .Select(a => new
                {
                    a.IdAsiento,
                    a.NumeroAsiento,
                    a.Clase,
                    a.PrecioExtra
                })
                .ToListAsync();
        }

        // 🔴 Asientos ocupados (basado en boletos)
        public async Task<IEnumerable<object>> GetOcupadosAsync(int idVuelo)
        {
            return await _context.Boletos
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .Select(b => new
                {
                    b.IdAsiento,
                    Asiento = b.Asiento.NumeroAsiento,
                    Pasajero = b.Reserva.Pasajero.NombrePasajero + " " +
                               b.Reserva.Pasajero.ApellidoPasajero
                })
                .ToListAsync();
        }

        // 📊 Resumen de ocupación
        public async Task<object> GetResumenOcupacionAsync(int idVuelo)
        {
            var total = await _context.Asientos
                .CountAsync(a => a.IdVuelo == idVuelo && !a.Eliminado);

            var ocupados = await _context.Boletos
                .CountAsync(b => b.IdVuelo == idVuelo && !b.EsEliminado);

            var disponibles = total - ocupados;

            return new
            {
                TotalAsientos = total,
                Ocupados = ocupados,
                Disponibles = disponibles
            };
        }

        // 📊 Distribución por disponibilidad (CORREGIDO)
        public async Task<IEnumerable<object>> GetDistribucionDisponibilidadAsync(int idVuelo)
        {
            return await _context.Asientos
                .Where(a => a.IdVuelo == idVuelo && !a.Eliminado)
                .GroupBy(a => a.Disponible)
                .Select(g => new
                {
                    Disponible = g.Key,
                    Cantidad = g.Count()
                })
                .ToListAsync();
        }
    }
}