using System;
using System.Collections.Generic;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class ClienteEntity
    {
        public int IdCliente { get; set; }

        public Guid ClienteGuid { get; set; }

        public string TipoIdentificacion { get; set; } = null!;
        public string NumeroIdentificacion { get; set; } = null!;

        public string? Nombres { get; set; }          // 🔥 nullable (empresa vs persona)
        public string? Apellidos { get; set; }        // 🔥 nullable
        public string? RazonSocial { get; set; }      // 🔥 nullable

        public string? Correo { get; set; }           // 🔥 nullable
        public string? Telefono { get; set; }         // 🔥 nullable
        public string? Direccion { get; set; }        // 🔥 nullable

        public int IdCiudadResidencia { get; set; }
        public int IdPaisNacionalidad { get; set; }

        public DateTime? FechaNacimiento { get; set; }
        public string? Nacionalidad { get; set; }     // 🔥 nullable
        public string? Genero { get; set; }           // 🔥 nullable

        public string Estado { get; set; } = null!;
        public bool EsEliminado { get; set; }

        public string CreadoPorUsuario { get; set; } = null!;
        public DateTime FechaRegistroUtc { get; set; }

        public string? ModificadoPorUsuario { get; set; }   // 🔥 nullable
        public DateTime? FechaModificacionUtc { get; set; }
        public string? ModificacionIp { get; set; }         // 🔥 nullable

        public string? ServicioOrigen { get; set; }         // 🔥 nullable

        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string? MotivoInhabilitacion { get; set; }   // 🔥 nullable

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones
        public virtual CiudadEntity? CiudadResidencia { get; set; }   // 🔥 nullable
        public virtual PaisEntity? PaisNacionalidad { get; set; }     // 🔥 nullable

       
        //public virtual ICollection<MetodoPagoEntity> MetodosPago { get; set; } = new List<MetodoPagoEntity>();
        public virtual ICollection<PasajeroEntity> Pasajeros { get; set; } = new List<PasajeroEntity>();

        public virtual ICollection<ReservaEntity> Reservas { get; set; } = new List<ReservaEntity>();

        public virtual ICollection<UsuarioAppEntity> UsuariosApp { get; set; } = new List<UsuarioAppEntity>();
    }
}