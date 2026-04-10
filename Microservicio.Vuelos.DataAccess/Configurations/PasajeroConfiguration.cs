using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class PasajeroConfiguration : IEntityTypeConfiguration<PasajeroEntity>
    {
        public void Configure(EntityTypeBuilder<PasajeroEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Pasajero", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdPasajero);

            builder.Property(x => x.IdPasajero)
                   .HasColumnName("id_pasajero");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK (opcional)
            builder.Property(x => x.IdCliente)
                   .HasColumnName("id_cliente");

            // 👤 Datos personales
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
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("requiere_asistencia");

            builder.Property(x => x.ObservacionesPasajero)
                   .HasMaxLength(255)
                   .HasColumnName("observaciones_pasajero");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("ACTIVO")
                   .HasColumnName("estado");

            builder.Property(x => x.EsEliminado)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("es_eliminado");

            // 🧾 Auditoría
            builder.Property(x => x.CreadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(45)
                   .HasColumnName("modificacion_ip");

            // 🔗 Relación

            builder.HasOne(x => x.Cliente)
                   .WithMany()
                   .HasForeignKey(x => x.IdCliente);
        }
    }
}
