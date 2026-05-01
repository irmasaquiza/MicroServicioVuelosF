using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class AuditoriaLogConfiguration : IEntityTypeConfiguration<AuditoriaLogEntity>
    {
        public void Configure(EntityTypeBuilder<AuditoriaLogEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("AUDITORIA_LOG", "crm");

            // 🔑 PK
            builder.HasKey(x => x.IdAuditoria);

            builder.Property(x => x.IdAuditoria)
                   .HasColumnName("id_auditoria");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
        .HasColumnName("row_version")
        .HasDefaultValueSql("decode('00000001','hex')")
        .ValueGeneratedOnAdd();

            // 🆔 GUID
            builder.Property(x => x.AuditoriaGuid)
                   .IsRequired()
                   .HasColumnName("auditoria_guid");

            // 🏷️ Campos principales
            builder.Property(x => x.TablaAfectada)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("tabla_afectada");

            builder.Property(x => x.Operacion)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("operacion");

            builder.Property(x => x.IdRegistroAfectado)
                   .HasMaxLength(50)
                   .HasColumnName("id_registro_afectado");

            builder.Property(x => x.DatosAnteriores)
                   .HasColumnType("text")
                   .HasColumnName("datos_anteriores");

            builder.Property(x => x.DatosNuevos)
                   .HasColumnType("text")
                   .HasColumnName("datos_nuevos");

            builder.Property(x => x.UsuarioEjecutor)
                   .HasMaxLength(100)
                   .HasColumnName("usuario_ejecutor");

            builder.Property(x => x.IpOrigen)
                   .HasMaxLength(50)
                   .HasColumnName("ip_origen");

            builder.Property(x => x.FechaEventoUtc)
                   .IsRequired()
                   .HasColumnName("fecha_evento_utc");

            builder.Property(x => x.Activo)
                   .HasColumnName("activo");

            // ⚡ Índices útiles
            builder.HasIndex(x => x.AuditoriaGuid)
                   .IsUnique()
                   .HasDatabaseName("UQ_AUDITORIA_GUID");

            builder.HasIndex(x => x.TablaAfectada)
                   .HasDatabaseName("IX_AUDITORIA_TABLA");

            builder.HasIndex(x => x.FechaEventoUtc)
                   .HasDatabaseName("IX_AUDITORIA_FECHA");
        }
    }
}