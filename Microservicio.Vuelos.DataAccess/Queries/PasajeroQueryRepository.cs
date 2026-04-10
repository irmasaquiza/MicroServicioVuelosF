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
    public class PasajeroQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public PasajeroQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🧍‍♂️ Listado de pasajeros
        public async Task<IEnumerable<object>> GetPasajerosAsync()
        {
            return await _context.Pasajeros
                .Where(p => !p.EsEliminado)
                .Select(p => new
                {
                    p.IdPasajero,
                    NombreCompleto = p.NombrePasajero + " " + p.ApellidoPasajero,
                    p.NumeroDocumentoPasajero,
                    p.NacionalidadPasajero
                })
                .ToListAsync();
        }

        // 🧍‍♂️ Perfil completo del pasajero
        public async Task<object> GetPerfilPasajeroAsync(int idPasajero)
        {
            return await _context.Pasajeros
                .Where(p => p.IdPasajero == idPasajero && !p.EsEliminado)
                .Select(p => new
                {
                    p.IdPasajero,
                    p.NombrePasajero,
                    p.ApellidoPasajero,
                    p.NumeroDocumentoPasajero,
                    p.TipoDocumentoPasajero,
                    p.FechaNacimientoPasajero,
                    p.NacionalidadPasajero,
                    p.EmailContactoPasajero,
                    p.TelefonoContactoPasajero,
                    p.GeneroPasajero,
                    p.RequiereAsistencia
                })
                .FirstOrDefaultAsync();
        }

        // 🎟️ Historial de vuelos del pasajero
        public async Task<IEnumerable<object>> GetHistorialVuelosAsync(int idPasajero)
        {
            return await _context.Boletos
                .Where(b => b.Reserva.IdPasajero == idPasajero && !b.EsEliminado)
                .Select(b => new
                {
                    b.IdBoleto,
                    b.CodigoBoleto,
                    Vuelo = b.Vuelo.CodigoVuelo,
                    Fecha = b.Vuelo.FechaHoraSalida,
                    Origen = b.Vuelo.AeropuertoOrigen.Nombre,
                    Destino = b.Vuelo.AeropuertoDestino.Nombre
                })
                .ToListAsync();
        }

        // 💺 Asientos usados por pasajero
        public async Task<IEnumerable<object>> GetAsientosPasajeroAsync(int idPasajero)
        {
            return await _context.Boletos
                .Where(b => b.Reserva.IdPasajero == idPasajero && !b.EsEliminado)
                .Select(b => new
                {
                    Vuelo = b.Vuelo.CodigoVuelo,
                    Asiento = b.Asiento.NumeroAsiento,
                    Clase = b.Clase
                })
                .ToListAsync();
        }

        // 🧳 Equipaje del pasajero
        public async Task<IEnumerable<object>> GetEquipajePasajeroAsync(int idPasajero)
        {
            return await _context.Equipajes
                .Where(e => e.Boleto.Reserva.IdPasajero == idPasajero && !e.EsEliminado)
                .Select(e => new
                {
                    e.Tipo,
                    e.PesoKg,
                    e.PrecioExtra,
                    Vuelo = e.Boleto.Vuelo.CodigoVuelo
                })
                .ToListAsync();
        }

        // 📊 Resumen del pasajero
        public async Task<object> GetResumenPasajeroAsync(int idPasajero)
        {
            var totalVuelos = await _context.Boletos
                .CountAsync(b => b.Reserva.IdPasajero == idPasajero && !b.EsEliminado);

            var gastoTotal = await _context.Boletos
                .Where(b => b.Reserva.IdPasajero == idPasajero && !b.EsEliminado)
                .SumAsync(b => (decimal?)b.PrecioFinal) ?? 0;

            return new
            {
                TotalVuelos = totalVuelos,
                GastoTotal = gastoTotal
            };
        }
    }
}
