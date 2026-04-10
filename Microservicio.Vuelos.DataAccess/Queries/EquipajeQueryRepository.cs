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
    public class EquipajeQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public EquipajeQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🧳 Equipaje por boleto
        public async Task<IEnumerable<object>> GetByBoletoAsync(int idBoleto)
        {
            return await _context.Equipajes
                .Where(e => e.IdBoleto == idBoleto && !e.EsEliminado)
                .Select(e => new
                {
                    e.IdEquipaje,
                    e.Tipo,
                    e.PesoKg,
                    e.PrecioExtra,
                    e.EstadoEquipaje
                })
                .ToListAsync();
        }

        // 🧳 Equipaje por vuelo (via boletos)
        public async Task<IEnumerable<object>> GetByVueloAsync(int idVuelo)
        {
            return await _context.Equipajes
                .Where(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado)
                .Select(e => new
                {
                    e.IdEquipaje,
                    e.Tipo,
                    e.PesoKg,
                    Pasajero = e.Boleto.Reserva.Pasajero.NombrePasajero + " " +
                               e.Boleto.Reserva.Pasajero.ApellidoPasajero
                })
                .ToListAsync();
        }

        // 👤 Equipaje por pasajero
        public async Task<IEnumerable<object>> GetByPasajeroAsync(int idPasajero)
        {
            return await _context.Equipajes
                .Where(e => e.Boleto.Reserva.IdPasajero == idPasajero && !e.EsEliminado)
                .Select(e => new
                {
                    e.IdEquipaje,
                    e.Tipo,
                    e.PesoKg,
                    e.PrecioExtra
                })
                .ToListAsync();
        }

        // 📊 Resumen de equipaje por vuelo
        public async Task<object> GetResumenPorVueloAsync(int idVuelo)
        {
            var total = await _context.Equipajes
                .CountAsync(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado);

            var pesoTotal = await _context.Equipajes
                .Where(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado)
                .SumAsync(e => (decimal?)e.PesoKg) ?? 0;

            var ingresoExtra = await _context.Equipajes
                .Where(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado)
                .SumAsync(e => (decimal?)e.PrecioExtra) ?? 0;

            return new
            {
                TotalEquipajes = total,
                PesoTotalKg = pesoTotal,
                IngresoExtra = ingresoExtra
            };
        }

        // 📊 Distribución por tipo
        public async Task<IEnumerable<object>> GetDistribucionTipoAsync(int idVuelo)
        {
            return await _context.Equipajes
                .Where(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado)
                .GroupBy(e => e.Tipo)
                .Select(g => new
                {
                    Tipo = g.Key,
                    Cantidad = g.Count(),
                    PesoTotal = g.Sum(x => x.PesoKg)
                })
                .ToListAsync();
        }
    }
}
