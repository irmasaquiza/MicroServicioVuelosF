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
    public class ClienteQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public ClienteQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 👤 Listado de clientes con ciudad y país
        public async Task<IEnumerable<object>> GetClientesDetalleAsync()
        {
            return await _context.Clientes
                .Where(c => !c.EsEliminado)
                .Select(c => new
                {
                    c.IdCliente,
                    NombreCompleto = c.Nombres + " " + c.Apellidos,
                    c.RazonSocial,
                    c.NumeroIdentificacion,
                    c.Correo,
                    Ciudad = c.CiudadResidencia.Nombre,
                    Pais = c.PaisNacionalidad.Nombre
                })
                .ToListAsync();
        }

        // 👤 Perfil completo del cliente
        public async Task<object?> GetPerfilClienteAsync(int idCliente)
        {
            return await _context.Clientes
                .Where(c => c.IdCliente == idCliente && !c.EsEliminado)
                .Select(c => new
                {
                    c.IdCliente,
                    c.Nombres,
                    c.Apellidos,
                    c.RazonSocial,
                    c.NumeroIdentificacion,
                    c.Correo,
                    c.Telefono,
                    c.Direccion,
                    c.FechaNacimiento,
                    c.Genero,
                    Ciudad = c.CiudadResidencia.Nombre,
                    Pais = c.PaisNacionalidad.Nombre
                })
                .FirstOrDefaultAsync();
        }

        // 🎟️ Historial de reservas del cliente
        public async Task<IEnumerable<object>> GetHistorialReservasAsync(int idCliente)
        {
            return await _context.Reservas
                .Where(r => r.IdCliente == idCliente && !r.EsEliminado)
                .Select(r => new
                {
                    r.IdReserva,
                    r.CodigoReserva,
                    FechaReserva = r.FechaReservaUtc, // 🔥 corregido
                    r.EstadoReserva,
                    r.TotalReserva
                })
                .ToListAsync();
        }

        // 🎟️ Boletos del cliente
        public async Task<IEnumerable<object>> GetBoletosClienteAsync(int idCliente)
        {
            return await _context.Boletos
                .Where(b => b.Reserva.IdCliente == idCliente && !b.EsEliminado)
                .Select(b => new
                {
                    b.IdBoleto,
                    b.CodigoBoleto,
                    b.Clase,
                    b.PrecioFinal,
                    Vuelo = b.Vuelo.CodigoVuelo
                })
                .ToListAsync();
        }

        // 💳 Métodos de pago del cliente
    /*    public async Task<IEnumerable<object>> GetMetodosPagoAsync(int idCliente)
        {
            return await _context.MetodosPago
                .Where(m => m.IdCliente == idCliente && !m.EsEliminado)
                .Select(m => new
                {
                    m.IdMetodo,
                    m.Alias,
                    m.MarcaTarjeta,
                    m.Ultimos4,
                    m.EsPrincipal
                })
                .ToListAsync();
        }*/

        // 📊 Resumen general del cliente
        public async Task<object> GetResumenClienteAsync(int idCliente)
        {
            var reservas = await _context.Reservas
                .CountAsync(r => r.IdCliente == idCliente && !r.EsEliminado);

            var boletos = await _context.Boletos
                .CountAsync(b => b.Reserva.IdCliente == idCliente && !b.EsEliminado);

            var gastoTotal = await _context.Facturas
                .Where(f => f.IdCliente == idCliente && !f.EsEliminado)
                .SumAsync(f => (decimal?)f.Total) ?? 0;

            return new
            {
                TotalReservas = reservas,
                TotalBoletos = boletos,
                GastoTotal = gastoTotal
            };
        }
    }
}