using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class AsientoEntity
    {
        public int IdAsiento { get; set; }

        public byte[] RowVersion { get; set; }

        public int IdVuelo { get; set; }

        public string NumeroAsiento { get; set; } = null!;   // ✔ obligatorio
        public string Clase { get; set; } = null!;           // ✔ obligatorio

        public bool Disponible { get; set; }

        public decimal PrecioExtra { get; set; }

        public string? Posicion { get; set; }                // 🔥 nullable

        public string Estado { get; set; } = null!;          // ✔ obligatorio
        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; } = null!;

        public string? ModificadoPorUsuario { get; set; }    // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }          // 🔥 nullable

        // 🔗 Relación
        public virtual VueloEntity? Vuelo { get; set; }      // 🔥 nullable
    }
}