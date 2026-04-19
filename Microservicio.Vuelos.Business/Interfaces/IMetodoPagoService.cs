using System;
using System.Collections.Generic;
using System.Text;

using System.Collections.Generic;
using System.Threading.Tasks;

using Microservicio.Vuelos.Business.DTOs.Internal.MetodoPago;

namespace Microservicio.Vuelos.Business.Interfaces
{
    public interface IMetodoPagoService
    {
        // ============================================================
        // 🔥 CREAR MÉTODO DE PAGO
        // ============================================================
        Task<MetodoPagoResponse> CrearAsync(CrearMetodoPagoRequest request);

        // ============================================================
        // 🔥 OBTENER POR ID
        // ============================================================
        Task<MetodoPagoResponse> GetByIdAsync(int id);

        // ============================================================
        // 🔥 LISTAR POR CLIENTE
        // ============================================================
        Task<IEnumerable<MetodoPagoResponse>> GetByClienteAsync(int idCliente);

        // ============================================================
        // 🔥 FILTRAR
        // ============================================================
        Task<IEnumerable<MetodoPagoResponse>> FiltrarAsync(MetodoPagoFiltroRequest request);

        // ============================================================
        // 🔥 ACTUALIZAR
        // ============================================================
        Task<MetodoPagoResponse> ActualizarAsync(int id, ActualizarMetodoPagoRequest request);

        // ============================================================
        // 🔥 ESTABLECER COMO PRINCIPAL
        // ============================================================
        Task<bool> EstablecerPrincipalAsync(int idMetodo);

        // ============================================================
        // 🔥 CAMBIAR ESTADO (ACTIVO / EXPIRADO / BLOQUEADO)
        // ============================================================
        Task<bool> CambiarEstadoAsync(int idMetodo, string estado);

        // ============================================================
        // 🔥 ELIMINAR (LÓGICO)
        // ============================================================
        Task<bool> EliminarAsync(int id);
    }
}