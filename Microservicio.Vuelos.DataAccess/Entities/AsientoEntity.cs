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

        public string NumeroAsiento { get; set; }
        public string Clase { get; set; }

        public bool Disponible { get; set; }

        public decimal PrecioExtra { get; set; }

        public string Posicion { get; set; }

        public string Estado { get; set; }
        public bool Eliminado { get; set; }

        public DateTime FechaRegistroUtc { get; set; }
        public string CreadoPorUsuario { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Relación
        public virtual VueloEntity Vuelo { get; set; }
    }
}