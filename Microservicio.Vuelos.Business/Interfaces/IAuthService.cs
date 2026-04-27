using System.Threading.Tasks;
using Microservicio.Vuelos.Business.DTOs.Internal.Auth;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IAuthService
    {
        Task<LoginResponse> LoginAsync(LoginRequest request);

        Task<bool> RegisterAsync(RegisterRequest request);

    }
}