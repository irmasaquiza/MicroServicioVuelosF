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
    public class AeropuertoQueryRepository
    {
        private readonly SistemaVuelosDbContext _context;

        public AeropuertoQueryRepository(SistemaVuelosDbContext context)
        {
            _context = context;
        }

        // 🧾 Aeropuertos con ciudad y país
        public async Task<IEnumerable<object>> GetAeropuertosDetalleAsync()
        {
            return await _context.Aeropuertos
                .Where(a => !a.Eliminado)
                .Select(a => new
                {
                    a.IdAeropuerto,
                    a.Nombre,
                    a.CodigoIata,
                    Ciudad = a.Ciudad.Nombre,
                    Pais = a.Pais.Nombre
                })
                .ToListAsync();
        }

        // ✈️ Vuelos que salen de un aeropuerto
        public async Task<IEnumerable<object>> GetVuelosSalidaAsync(int idAeropuerto)
        {
            return await _context.Vuelos
                .Where(v => v.IdAeropuertoOrigen == idAeropuerto && !v.EsEliminado)
                .Select(v => new
                {
                    v.IdVuelo,
                    v.CodigoVuelo,
                    v.FechaHoraSalida,
                    Destino = v.AeropuertoDestino.Nombre
                })
                .ToListAsync();
        }

        // 🛬 Vuelos que llegan a un aeropuerto
        public async Task<IEnumerable<object>> GetVuelosLlegadaAsync(int idAeropuerto)
        {
            return await _context.Vuelos
                .Where(v => v.IdAeropuertoDestino == idAeropuerto && !v.EsEliminado)
                .Select(v => new
                {
                    v.IdVuelo,
                    v.CodigoVuelo,
                    v.FechaHoraLlegada,
                    Origen = v.AeropuertoOrigen.Nombre
                })
                .ToListAsync();
        }

        // 🔄 Escalas por aeropuerto
        public async Task<IEnumerable<object>> GetEscalasPorAeropuertoAsync(int idAeropuerto)
        {
            return await _context.Escalas
                .Where(e => e.IdAeropuerto == idAeropuerto && !e.Eliminado)
                .Select(e => new
                {
                    e.IdEscala,
                    e.IdVuelo,
                    e.Orden,
                    e.FechaHoraLlegada,
                    e.FechaHoraSalida
                })
                .ToListAsync();
        }

        // 📊 Resumen operativo
        public async Task<object> GetResumenOperativoAsync(int idAeropuerto)
        {
            var vuelosSalida = await _context.Vuelos
                .CountAsync(v => v.IdAeropuertoOrigen == idAeropuerto && !v.EsEliminado);

            var vuelosLlegada = await _context.Vuelos
                .CountAsync(v => v.IdAeropuertoDestino == idAeropuerto && !v.EsEliminado);

            var escalas = await _context.Escalas
                .CountAsync(e => e.IdAeropuerto == idAeropuerto && !e.Eliminado);

            return new
            {
                VuelosSalida = vuelosSalida,
                VuelosLlegada = vuelosLlegada,
                TotalEscalas = escalas
            };
        }
    }
}