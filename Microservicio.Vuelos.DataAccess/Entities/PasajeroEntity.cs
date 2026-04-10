using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class PasajeroEntity
    {
        public int IdPasajero { get; set; }

        public byte[] RowVersion { get; set; }

        public int? IdCliente { get; set; }

        public string NombrePasajero { get; set; }
        public string ApellidoPasajero { get; set; }

        public string TipoDocumentoPasajero { get; set; }
        public string NumeroDocumentoPasajero { get; set; }

        public DateTime? FechaNacimientoPasajero { get; set; }
        public string NacionalidadPasajero { get; set; }

        public string EmailContactoPasajero { get; set; }
        public string TelefonoContactoPasajero { get; set; }

        public string GeneroPasajero { get; set; }

        public bool RequiereAsistencia { get; set; }

        public string ObservacionesPasajero { get; set; }

        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // 🔗 Relaciones
        public virtual ClienteEntity Cliente { get; set; }

        public virtual ICollection<ReservaEntity> Reservas { get; set; }
    }
}
