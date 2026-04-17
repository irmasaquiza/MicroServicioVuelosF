using System;
using System.Collections.Generic;
using System.Text;
using System;

namespace Microservicio.Vuelos.DataManagement.Models
{
    public class ClienteDataModel
    {
        public int IdCliente { get; set; }

        public Guid ClienteGuid { get; set; }

        // 🪪 Identificación
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

        // 👤 Datos personales / fiscales
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string RazonSocial { get; set; }

        // 📞 Contacto
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        // 🌍 Ubicación
        public int IdCiudadResidencia { get; set; }
        public int IdPaisNacionalidad { get; set; }

        // 📅 Datos adicionales
        public DateTime? FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Genero { get; set; }

        // 📊 Estado
        public string Estado { get; set; }

        // 🔗 Integración
        public string ServicioOrigen { get; set; }

        // ⚠️ Opcionales
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; }
    }
}
