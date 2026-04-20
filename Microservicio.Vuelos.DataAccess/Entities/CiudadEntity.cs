using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class CiudadEntity
    {
        public int IdCiudad { get; set; }

        public byte[] RowVersion { get; set; }

        public int IdPais { get; set; }

        public string? Nombre { get; set; }
        public string? CodigoPostal { get; set; }

        public string? ZonaHoraria { get; set; }

        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public string? Estado { get; set; }
        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }

        public string? ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }

        // 🔗 Relaciones

        public virtual PaisEntity Pais { get; set; }

        public virtual ICollection<AeropuertoEntity> Aeropuertos { get; set; }
    }
}