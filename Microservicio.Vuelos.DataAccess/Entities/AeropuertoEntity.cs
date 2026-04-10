using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class AeropuertoEntity
    {
        public int IdAeropuerto { get; set; }

        public byte[] RowVersion { get; set; }

        public string CodigoIata { get; set; }
        public string CodigoIcao { get; set; }

        public string Nombre { get; set; }

        public int? IdCiudad { get; set; }
        public int IdPais { get; set; }

        public string ZonaHoraria { get; set; }

        public decimal? Latitud { get; set; }
        public decimal? Longitud { get; set; }

        public string Estado { get; set; }
        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Relaciones (Navigation Properties)

        public virtual CiudadEntity Ciudad { get; set; }
        public virtual PaisEntity Pais { get; set; }
    }
}