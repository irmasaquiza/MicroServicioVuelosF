using System.Collections.Generic;
using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Vuelo;
using Microservicio.Vuelos.Business.DTOs.Booking.Vuelo;
namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IVueloService
    {
        Task<VueloResponse> CrearAsync(CrearVueloRequest request);

        Task<VueloResponse> GetByIdAsync(int id);

        Task<VueloDetalleResponse> GetDetalleAsync(int id);

        Task<IEnumerable<VueloResponse>> GetAllAsync();

        Task<IEnumerable<VueloResponse>> FiltrarAsync(VueloFiltroRequest request);

        Task<IEnumerable<VueloBookingResponse>> BuscarBookingAsync(VueloBookingFiltroRequest request);

        Task<VueloResponse> ActualizarAsync(int id, ActualizarVueloRequest request);

        Task<bool> CambiarEstadoAsync(int id, ActualizarEstadoVueloRequest request);

        Task<bool> EliminarAsync(int id);
    }
}