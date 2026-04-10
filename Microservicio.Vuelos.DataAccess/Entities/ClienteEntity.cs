using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Entities
{
    public class ClienteEntity
    {
        public int IdCliente { get; set; }

        public Guid ClienteGuid { get; set; }

        // Identificación
        public string TipoIdentificacion { get; set; }
        public string NumeroIdentificacion { get; set; }

        // Datos personales / fiscales
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string RazonSocial { get; set; }

        // Contacto
        public string Correo { get; set; }
        public string Telefono { get; set; }
        public string Direccion { get; set; }

        public int IdCiudadResidencia { get; set; }
        public int IdPaisNacionalidad { get; set; }

        // Datos adicionales
        public DateTime? FechaNacimiento { get; set; }
        public string Nacionalidad { get; set; }
        public string Genero { get; set; }

        // Estado
        public string Estado { get; set; }
        public bool EsEliminado { get; set; }

        // Auditoría
        public string CreadoPorUsuario { get; set; }
        public DateTime FechaRegistroUtc { get; set; }

        public string ModificadoPorUsuario { get; set; }
        public DateTime? FechaModificacionUtc { get; set; }
        public string ModificacionIp { get; set; }

        // Integración
        public string ServicioOrigen { get; set; }

        // Campos opcionales
        public DateTime? FechaInhabilitacionUtc { get; set; }
        public string MotivoInhabilitacion { get; set; }

        public byte[] RowVersion { get; set; }

        // 🔗 Relaciones

        public virtual CiudadEntity CiudadResidencia { get; set; }
        public virtual PaisEntity PaisNacionalidad { get; set; }

        public virtual ICollection<MetodoPagoEntity> MetodosPago { get; set; }
        public virtual ICollection<PasajeroEntity> Pasajeros { get; set; }
        public virtual ICollection<ReservaEntity> Reservas { get; set; }
        public virtual ICollection<UsuarioAppEntity> UsuariosApp { get; set; }
    }
}
