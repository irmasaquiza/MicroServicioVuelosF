using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class PasajeroEntity
    {
        public int IdPasajero { get; set; }

        public byte[] RowVersion { get; set; }

        public int? IdCliente { get; set; }   // ✔ ya nullable

        public string NombrePasajero { get; set; } = null!;
        public string ApellidoPasajero { get; set; } = null!;

        public string TipoDocumentoPasajero { get; set; } = null!;
        public string NumeroDocumentoPasajero { get; set; } = null!;

        public DateTime? FechaNacimientoPasajero { get; set; }
        public string? NacionalidadPasajero { get; set; }        // 🔥 nullable

        public string? EmailContactoPasajero { get; set; }       // 🔥 nullable
        public string? TelefonoContactoPasajero { get; set; }    // 🔥 nullable

        public string? GeneroPasajero { get; set; }              // 🔥 nullable

        public bool RequiereAsistencia { get; set; }

        public string? ObservacionesPasajero { get; set; }       // 🔥 nullable

        public string Estado { get; set; } = null!;
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }        // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }              // 🔥 nullable

        // 🔗 Relaciones
        public virtual ClienteEntity? Cliente { get; set; }      // 🔥 nullable

        public virtual ICollection<ReservaEntity> Reservas { get; set; } = new List<ReservaEntity>();
    }
}