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
    public class VueloQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public VueloQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // ✈️ Listado de vuelos con ruta
        public async Task<IEnumerable<object>> GetVuelosAsync()
        {
            return await _context.Vuelos
                .Where(v => !v.EsEliminado)
                .Select(v => new
                {
                    v.IdVuelo,
                    v.CodigoVuelo,
                    Origen = v.AeropuertoOrigen.Nombre,
                    Destino = v.AeropuertoDestino.Nombre,
                    v.FechaHoraSalida,
                    v.EstadoVuelo,
                    v.CapacidadTotal
                })
                .ToListAsync();
        }

        // ✈️ Detalle completo de vuelo
        public async Task<object> GetDetalleVueloAsync(int idVuelo)
        {
            return await _context.Vuelos
                .Where(v => v.IdVuelo == idVuelo && !v.EsEliminado)
                .Select(v => new
                {
                    v.IdVuelo,
                    v.CodigoVuelo,
                    v.FechaHoraSalida,
                    v.FechaHoraLlegada,
                    v.DuracionMin,
                    v.EstadoVuelo,

                    Origen = v.AeropuertoOrigen.Nombre,
                    Destino = v.AeropuertoDestino.Nombre,

                    v.CapacidadTotal,
                    v.PrecioBase
                })
                .FirstOrDefaultAsync();
        }

        // 🔄 Escalas del vuelo
        public async Task<IEnumerable<object>> GetEscalasVueloAsync(int idVuelo)
        {
            return await _context.Escalas
                .Where(e => e.IdVuelo == idVuelo && !e.Eliminado)
                .OrderBy(e => e.Orden)
                .Select(e => new
                {
                    e.Orden,
                    Aeropuerto = e.Aeropuerto.Nombre,
                    e.FechaHoraLlegada,
                    e.FechaHoraSalida
                })
                .ToListAsync();
        }

        // 💺 Mapa de asientos
        public async Task<IEnumerable<object>> GetAsientosVueloAsync(int idVuelo)
        {
            return await _context.Asientos
                .Where(a => a.IdVuelo == idVuelo && !a.Eliminado)
                .Select(a => new
                {
                    a.NumeroAsiento,
                    a.Clase,
                    a.Disponible,
                    a.PrecioExtra
                })
                .ToListAsync();
        }

        // 🧍‍♂️ Manifiesto de pasajeros
        public async Task<IEnumerable<object>> GetPasajerosVueloAsync(int idVuelo)
        {
            return await _context.Boletos
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .Select(b => new
                {
                    Pasajero = b.Reserva.Pasajero.NombrePasajero + " " +
                               b.Reserva.Pasajero.ApellidoPasajero,
                    Documento = b.Reserva.Pasajero.NumeroDocumentoPasajero,
                    Asiento = b.Asiento.NumeroAsiento
                })
                .ToListAsync();
        }

        // 📊 Ocupación del vuelo
        public async Task<object> GetOcupacionVueloAsync(int idVuelo)
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

        // 💰 Ingresos por vuelo
        public async Task<object> GetIngresosVueloAsync(int idVuelo)
        {
            var totalBoletos = await _context.Boletos
                .Where(b => b.IdVuelo == idVuelo && !b.EsEliminado)
                .SumAsync(b => (decimal?)b.PrecioFinal) ?? 0;

            var equipaje = await _context.Equipajes
                .Where(e => e.Boleto.IdVuelo == idVuelo && !e.EsEliminado)
                .SumAsync(e => (decimal?)e.PrecioExtra) ?? 0;

            return new
            {
                IngresoBoletos = totalBoletos,
                IngresoEquipaje = equipaje,
                TotalIngreso = totalBoletos + equipaje
            };
        }

        // 📊 Resumen general del vuelo
        public async Task<object> GetResumenVueloAsync(int idVuelo)
        {
            var pasajeros = await _context.Boletos
                .CountAsync(b => b.IdVuelo == idVuelo && !b.EsEliminado);

            var escalas = await _context.Escalas
                .CountAsync(e => e.IdVuelo == idVuelo && !e.Eliminado);

            return new
            {
                TotalPasajeros = pasajeros,
                TotalEscalas = escalas
            };
        }
    }
}