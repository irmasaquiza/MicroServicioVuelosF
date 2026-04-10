using System;
using System.Collections.Generic;
using System.Text;

using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class TipoMetodoPagoEntity
    {
        public int IdTipoMetodo { get; set; }

        public string NombreTipo { get; set; }   // VARCHAR(50)
        public string? Descripcion { get; set; } // VARCHAR(150)

        public string Estado { get; set; }       // ACTIVO / INACTIVO
        public bool EsEliminado { get; set; }

        // 🔗 Relaciones
        public virtual ICollection<MetodoPagoEntity> MetodosPago { get; set; } = new List<MetodoPagoEntity>();
    }
}