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
    public class BoletoQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public BoletoQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🎟️ Detalle completo de boleto
        public async Task<object> GetDetalleBoletoAsync(int idBoleto)
        {
            return await _context.Boletos
                .Where(b => b.IdBoleto == idBoleto && !b.EsEliminado)
                .Select(b => new
                {
                    b.IdBoleto,
                    b.CodigoBoleto,
                    b.Clase,
                    b.PrecioFinal,

                    Vuelo = new
                    {
                        b.Vuelo.CodigoVuelo,
                        b.Vuelo.FechaHoraSalida,
                        Origen = b.Vuelo.AeropuertoOrigen.Nombre,
                        Destino = b.Vuelo.AeropuertoDestino.Nombre
                    },

                    Asiento = b.Asiento.NumeroAsiento,

                    Pasajero = new
                    {
                        b.Reserva.Pasajero.NombrePasajero,
                        b.Reserva.Pasajero.ApellidoPasajero,
                        b.Reserva.Pasajero.NumeroDocumentoPasajero
                    }
                })
                .FirstOrDefaultAsync();
        }

        // 🎟️ Boletos por reserva
        public async Task<IEnumerable<object>> GetByReservaAsync(int idReserva)
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
                    Asiento = b.Asiento.NumeroAsiento
                })
                .ToListAsync();
        }

        // 🎟️ Boletos por pasajero
        public async Task<IEnumerable<object>> GetByPasajeroAsync(int idPasajero)
        {
            return await _context.Boletos
                .Where(b => b.Reserva.IdPasajero == idPasajero && !b.EsEliminado)
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

        // 🧾 Manifiesto de pasajeros por vuelo
        public async Task<IEnumerable<object>> GetManifiestoVueloAsync(int idVuelo)
        {
            return await _context.Boletos
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .Select(b => new
                {
                    Asiento = b.Asiento.NumeroAsiento,
                    Pasajero = b.Reserva.Pasajero.NombrePasajero + " " +
                               b.Reserva.Pasajero.ApellidoPasajero,
                    Documento = b.Reserva.Pasajero.NumeroDocumentoPasajero
                })
                .ToListAsync();
        }

        // 🛫 Listado de embarque
        public async Task<IEnumerable<object>> GetListadoEmbarqueAsync(int idVuelo)
        {
            return await _context.Boletos
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .Select(b => new
                {
                    b.CodigoBoleto,
                    Pasajero = b.Reserva.Pasajero.NombrePasajero + " " +
                               b.Reserva.Pasajero.ApellidoPasajero,
                    Asiento = b.Asiento.NumeroAsiento,
                    Estado = b.EstadoBoleto
                })
                .ToListAsync();
        }

        // 🧳 Equipaje por boleto
        public async Task<IEnumerable<object>> GetEquipajePorBoletoAsync(int idBoleto)
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
    }
}