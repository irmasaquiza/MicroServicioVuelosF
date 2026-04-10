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
    public class ReservaQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public ReservaQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🧾 Listado de reservas
        public async Task<IEnumerable<object>> GetReservasAsync()
        {
            return await _context.Reservas
                .Where(r => !r.EsEliminado)
                .Select(r => new
                {
                    r.IdReserva,
                    r.CodigoReserva,
                    FechaReserva = r.FechaReservaUtc, // 🔥 corregido
                    r.EstadoReserva,
                    r.TotalReserva,
                    Cliente = r.Cliente.Nombres + " " + r.Cliente.Apellidos,
                    Pasajero = r.Pasajero.NombrePasajero + " " + r.Pasajero.ApellidoPasajero
                })
                .ToListAsync();
        }

        // 🧾 Detalle completo de reserva
        public async Task<object?> GetDetalleReservaAsync(int idReserva)
        {
            return await _context.Reservas
                .Where(r => r.IdReserva == idReserva && !r.EsEliminado)
                .Select(r => new
                {
                    r.IdReserva,
                    r.CodigoReserva,
                    FechaReserva = r.FechaReservaUtc, // 🔥 corregido
                    r.EstadoReserva,
                    r.TotalReserva,

                    Cliente = new
                    {
                        r.Cliente.Nombres,
                        r.Cliente.Apellidos,
                        r.Cliente.Correo
                    },

                    Pasajero = new
                    {
                        r.Pasajero.NombrePasajero,
                        r.Pasajero.ApellidoPasajero,
                        r.Pasajero.NumeroDocumentoPasajero // 🔥 corregido
                    }
                })
                .FirstOrDefaultAsync();
        }

        // 🎟️ Boletos de la reserva
        public async Task<IEnumerable<object>> GetBoletosReservaAsync(int idReserva)
        {
            return await _context.Boletos
                .Where(b => b.IdReserva == idReserva && !b.EsEliminado)
                .Select(b => new
                {
                    b.IdBoleto,
                    b.CodigoBoleto,
                    b.Clase,
                    b.PrecioFinal,
                    Vuelo = b.Vuelo.CodigoVuelo,
                    Fecha = b.Vuelo.FechaHoraSalida,
                    Asiento = b.Asiento.NumeroAsiento
                })
                .ToListAsync();
        }

        // 💳 Facturas de la reserva
        public async Task<IEnumerable<object>> GetFacturasReservaAsync(int idReserva)
        {
            return await _context.Facturas
                .Where(f => f.IdReserva == idReserva && !f.EsEliminado)
                .Select(f => new
                {
                    f.IdFactura,
                    f.NumeroFactura,
                    f.Total,
                    f.Estado
                })
                .ToListAsync();
        }

        // 🧳 Equipaje de la reserva
        public async Task<IEnumerable<object>> GetEquipajeReservaAsync(int idReserva)
        {
            return await _context.Equipajes
                .Where(e => e.Boleto.IdReserva == idReserva && !e.EsEliminado)
                .Select(e => new
                {
                    e.Tipo,
                    e.PesoKg,
                    e.PrecioExtra,
                    Vuelo = e.Boleto.Vuelo.CodigoVuelo
                })
                .ToListAsync();
        }

        // 📊 Resumen de reserva
        public async Task<object> GetResumenReservaAsync(int idReserva)
        {
            var totalBoletos = await _context.Boletos
                .CountAsync(b => b.IdReserva == idReserva && !b.EsEliminado);

            var totalEquipaje = await _context.Equipajes
                .CountAsync(e => e.Boleto.IdReserva == idReserva && !e.EsEliminado);

            var totalPagado = await _context.Facturas
                .Where(f => f.IdReserva == idReserva && !f.EsEliminado)
                .SumAsync(f => (decimal?)f.Total) ?? 0;

            return new
            {
                TotalBoletos = totalBoletos,
                TotalEquipaje = totalEquipaje,
                TotalPagado = totalPagado
            };
        }
    }
}
