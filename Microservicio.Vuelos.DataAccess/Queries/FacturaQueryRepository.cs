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
    public class FacturaQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public FacturaQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🧾 Detalle completo de factura
        public async Task<object> GetDetalleFacturaAsync(int idFactura)
        {
            return await _context.Facturas
                .Where(f => f.IdFactura == idFactura && !f.EsEliminado)
                .Select(f => new
                {
                    f.IdFactura,
                    f.NumeroFactura,
                    f.FechaEmision,

                    Cliente = f.Cliente.Nombres + " " + f.Cliente.Apellidos,

                    Totales = new
                    {
                        f.Subtotal,
                        f.ValorIva,
                        f.CargoServicio,
                        f.Total
                    },

                    /*MetodoPago = new
                    {
                        f.MetodoPago.Alias,
                        f.MetodoPago.MarcaTarjeta,
                        f.MetodoPago.Ultimos4
                    },*/

                    Estado = f.Estado
                })
                .FirstOrDefaultAsync();
        }

        // 🧾 Facturas por cliente
        public async Task<IEnumerable<object>> GetByClienteAsync(int idCliente)
        {
            return await _context.Facturas
                .Where(f => f.IdCliente == idCliente && !f.EsEliminado)
                .Select(f => new
                {
                    f.IdFactura,
                    f.NumeroFactura,
                    f.FechaEmision,
                    f.Total,
                    f.Estado
                })
                .ToListAsync();
        }

        // 🧾 Facturas por reserva
        public async Task<IEnumerable<object>> GetByReservaAsync(int idReserva)
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

        // 🎟️ Detalle de boletos facturados
        public async Task<IEnumerable<object>> GetBoletosFacturaAsync(int idFactura)
        {
            return await _context.Boletos
                .Where(b => b.IdFactura == idFactura && !b.EsEliminado)
                .Select(b => new
                {
                    b.IdBoleto,
                    b.CodigoBoleto,
                    b.Clase,
                    b.PrecioFinal,
                    Vuelo = b.Vuelo.CodigoVuelo,
                    Asiento = b.Asiento.NumeroAsiento
                })
                .ToListAsync();
        }

        // 📊 Resumen por cliente
        public async Task<object> GetResumenClienteAsync(int idCliente)
        {
            var totalFacturas = await _context.Facturas
                .CountAsync(f => f.IdCliente == idCliente && !f.EsEliminado);

            var totalPagado = await _context.Facturas
                .Where(f => f.IdCliente == idCliente && !f.EsEliminado)
                .SumAsync(f => (decimal?)f.Total) ?? 0;

            return new
            {
                TotalFacturas = totalFacturas,
                TotalPagado = totalPagado
            };
        }

        // 📊 Facturación por estado
        public async Task<IEnumerable<object>> GetDistribucionEstadoAsync()
        {
            return await _context.Facturas
                .Where(f => !f.EsEliminado)
                .GroupBy(f => f.Estado)
                .Select(g => new
                {
                    Estado = g.Key,
                    Cantidad = g.Count(),
                    Total = g.Sum(x => x.Total)
                })
                .ToListAsync();
        }

        // 📊 Ingresos por fecha
        public async Task<IEnumerable<object>> GetIngresosPorFechaAsync()
        {
            return await _context.Facturas
                .Where(f => !f.EsEliminado)
                .GroupBy(f => f.FechaEmision.Date)
                .Select(g => new
                {
                    Fecha = g.Key,
                    Total = g.Sum(x => x.Total)
                })
                .OrderBy(x => x.Fecha)
                .ToListAsync();
        }
    }
}
