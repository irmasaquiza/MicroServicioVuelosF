using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class AeropuertoEntity
    {
        public int IdAeropuerto { get; set; }

        public byte[] RowVersion { get; set; }

        public string? CodigoIata { get; set; }      // 🔥 nullable
        public string? CodigoIcao { get; set; }      // 🔥 nullable

        public string Nombre { get; set; } = null!;  // ✔ obligatorio

        public int? IdCiudad { get; set; }           // ✔ ya nullable
        public int IdPais { get; set; }              // ✔ obligatorio

        public string? ZonaHoraria { get; set; }     // 🔥 nullable

        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public string Estado { get; set; } = null!;  // ✔ obligatorio
        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; } = null!;

        public string? ModificadoPorUsuario { get; set; }   // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }         // 🔥 nullable

        // 🔗 Relaciones

        public virtual CiudadEntity? Ciudad { get; set; }   // 🔥 nullable
        public virtual PaisEntity? Pais { get; set; }       // 🔥 nullable
    }
}