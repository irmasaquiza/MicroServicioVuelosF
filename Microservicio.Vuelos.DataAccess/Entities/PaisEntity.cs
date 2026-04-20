using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class PaisEntity
    {
        public int IdPais { get; set; }

        public string? CodigoIso2 { get; set; } = null!;   // ✔ obligatorio
        public string? CodigoIso3 { get; set; } = null!;   // ✔ obligatorio

        public string Nombre { get; set; } = null!;
        public string? Continente { get; set; }           // 🔥 nullable

        public string? Estado { get; set; } = null!;
        public bool Eliminado { get; set; }

        // 🔗 Relaciones

        public virtual ICollection<CiudadEntity> Ciudades { get; set; } = new List<CiudadEntity>();
        public virtual ICollection<AeropuertoEntity> Aeropuertos { get; set; } = new List<AeropuertoEntity>();
        public virtual ICollection<ClienteEntity> Clientes { get; set; } = new List<ClienteEntity>();
    }
}