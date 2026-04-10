using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class PaisEntity
    {
        public int IdPais { get; set; }

        public string CodigoIso2 { get; set; }
        public string CodigoIso3 { get; set; }

        public string Nombre { get; set; }
        public string Continente { get; set; }

        public string Estado { get; set; }
        public bool Eliminado { get; set; }

        // 🔗 Relaciones

        public virtual ICollection<CiudadEntity> Ciudades { get; set; }
        public virtual ICollection<AeropuertoEntity> Aeropuertos { get; set; }
        public virtual ICollection<ClienteEntity> Clientes { get; set; }
    }
}