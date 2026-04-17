using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class PasajeroDataModel
    {
        public int IdPasajero { get; set; }

        // 🔗 Relación
        public int? IdCliente { get; set; }

        // 👤 Datos personales
        public string NombrePasajero { get; set; }
        public string ApellidoPasajero { get; set; }

        public string TipoDocumentoPasajero { get; set; }
        public string NumeroDocumentoPasajero { get; set; }

        public DateTime? FechaNacimientoPasajero { get; set; }
        public string NacionalidadPasajero { get; set; }

        // 📞 Contacto
        public string EmailContactoPasajero { get; set; }
        public string TelefonoContactoPasajero { get; set; }

        public string GeneroPasajero { get; set; }

        public bool RequiereAsistencia { get; set; }

        public string ObservacionesPasajero { get; set; }

        // 📊 Estado
        public string Estado { get; set; }
    }
}