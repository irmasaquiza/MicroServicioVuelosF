using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class PasajeroConfiguration : IEntityTypeConfiguration<PasajeroEntity>
    {
        public void Configure(EntityTypeBuilder<PasajeroEntity> builder)
        {
            builder.ToTable("Pasajero", "ventas");

            builder.HasKey(x => x.IdPasajero);

            builder.Property(x => x.IdPasajero)
                   .HasColumnName("id_pasajero");

            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            builder.Property(x => x.IdCliente)
                   .HasColumnName("id_cliente")
                   .IsRequired(false);

            builder.Property(x => x.NombrePasajero)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("nombre_pasajero");

            builder.Property(x => x.ApellidoPasajero)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("apellido_pasajero");

            builder.Property(x => x.TipoDocumentoPasajero)
                   .IsRequired()
                   .HasMaxLength(30)
                   .HasColumnName("tipo_documento_pasajero");

            builder.Property(x => x.NumeroDocumentoPasajero)
                   .IsRequired()
                   .HasMaxLength(30)
                   .HasColumnName("numero_documento_pasajero");

            builder.Property(x => x.FechaNacimientoPasajero)
                   .HasColumnType("date")
                   .HasColumnName("fecha_nacimiento_pasajero");

            builder.Property(x => x.NacionalidadPasajero)
                   .HasMaxLength(80)
                   .HasColumnName("nacionalidad_pasajero");

            builder.Property(x => x.EmailContactoPasajero)
                   .HasMaxLength(150)
                   .HasColumnName("email_contacto_pasajero");

            builder.Property(x => x.TelefonoContactoPasajero)
                   .HasMaxLength(20)
                   .HasColumnName("telefono_contacto_pasajero");

            builder.Property(x => x.GeneroPasajero)
                   .HasMaxLength(20)
                   .HasColumnName("genero_pasajero");

            builder.Property(x => x.RequiereAsistencia)
                   .HasColumnName("requiere_asistencia")
                   .HasDefaultValue(false);

            builder.Property(x => x.ObservacionesPasajero)
                   .HasMaxLength(255)
                   .HasColumnName("observaciones_pasajero");

            builder.Property(x => x.Estado)
                   .HasColumnName("estado")
                   .HasMaxLength(20)
                   .HasDefaultValue("ACTIVO");

            builder.Property(x => x.EsEliminado)
                   .HasColumnName("es_eliminado")
                   .HasDefaultValue(false);

            builder.Property(x => x.CreadoPorUsuario)
                   .HasColumnName("creado_por_usuario")
                   .HasMaxLength(100);

            builder.Property(x => x.FechaRegistroUtc)
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasColumnName("modificacion_ip");

            // 🔥 RELACIÓN CORRECTA (ESTO ARREGLA TU ERROR)
            builder.HasOne(x => x.Cliente)
                   .WithMany(c => c.Pasajeros)
                   .HasForeignKey(x => x.IdCliente)
                   .OnDelete(DeleteBehavior.Restrict)
                   .HasConstraintName("FK_Pasajero_Cliente");
        }
    }
}