using System;
using System.Collections.Generic;
using System.Text;

 
namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class TipoMetodoPagoEntity
    {
        public int IdTipoMetodo { get; set; }

        public string Codigo { get; set; }   // TARJETA, PAYPAL, TRANSFERENCIA
        public string Nombre { get; set; }   // Tarjeta de crédito, PayPal, etc.
        public string Descripcion { get; set; }

        public bool RequiereAutorizacionExterna { get; set; }

        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        // 🔗 Relaciones
        public virtual ICollection<MetodoPagoEntity> MetodosPago { get; set; }
    }
}