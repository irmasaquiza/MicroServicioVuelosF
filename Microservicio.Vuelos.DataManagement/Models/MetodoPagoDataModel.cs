/*using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class MetodoPagoDataModel
    {
        public int IdMetodo { get; set; }

        // 🔗 Relaciones
        public int IdCliente { get; set; }
        public int IdTipoMetodo { get; set; }

        // 💳 Datos del método de pago
        public string Ultimos4 { get; set; }
        public string ReferenciaVisible { get; set; }
        public string TokenPasarela { get; set; }

        public DateTime? FechaExpiracion { get; set; }

        public string NombreTitular { get; set; }
        public string MarcaTarjeta { get; set; }
        public string BancoEmisor { get; set; }
        public string PaisEmision { get; set; }

        public bool EsPrincipal { get; set; }
        public string Alias { get; set; }

        public DateTime? FechaUltimoUso { get; set; }

        // 📊 Estado
        public string Estado { get; set; }
    }
}*/