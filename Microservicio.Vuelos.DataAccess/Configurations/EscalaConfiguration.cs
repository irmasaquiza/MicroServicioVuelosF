using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class EscalaConfiguration : IEntityTypeConfiguration<EscalaEntity>
    {
        public void Configure(EntityTypeBuilder<EscalaEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Escala", "vuelos");

            // 🔑 PK
            builder.HasKey(x => x.IdEscala);

            builder.Property(x => x.IdEscala)
                   .HasColumnName("id_escala");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdVuelo)
                   .IsRequired()
                   .HasColumnName("id_vuelo");

            builder.Property(x => x.IdAeropuerto)
                   .IsRequired()
                   .HasColumnName("id_aeropuerto");

            // 🔢 Orden
            builder.Property(x => x.Orden)
                   .IsRequired()
                   .HasColumnName("orden");

            // 🕒 Fechas
            builder.Property(x => x.FechaHoraLlegada)
                   .IsRequired()
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_hora_llegada");

            builder.Property(x => x.FechaHoraSalida)
                   .IsRequired()
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_hora_salida");

            // ⏱️ Duración
            builder.Property(x => x.DuracionMin)
                   .IsRequired()
                   .HasDefaultValue(0)
                   .HasColumnName("duracion_min");

            // 🏷️ Tipo escala
            builder.Property(x => x.TipoEscala)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("COMERCIAL")
                   .HasColumnName("tipo_escala");

            // 📍 Terminal / puerta
            builder.Property(x => x.Terminal)
                   .HasMaxLength(50)
                   .HasColumnName("terminal");

            builder.Property(x => x.Puerta)
                   .HasMaxLength(10)
                   .HasColumnName("puerta");

            // 📝 Observaciones
            builder.Property(x => x.Observaciones)
                   .HasMaxLength(255)
                   .HasColumnName("observaciones");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasDefaultValue("ACTIVO")
                   .HasColumnName("estado");

            builder.Property(x => x.Eliminado)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("eliminado");

            // 🧾 Auditoría
            builder.Property(x => x.FechaRegistroUtc)
                   .IsRequired()
                   .HasDefaultValueSql("SYSUTCDATETIME()")
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.CreadoPorUsuario)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasDefaultValue("SYSTEM")
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(45)
                   .HasColumnName("modificacion_ip");

            // 🔗 Relaciones
            builder.HasOne(x => x.Vuelo)
                   .WithMany(v => v.Escalas)
                   .HasForeignKey(x => x.IdVuelo);

            builder.HasOne(x => x.Aeropuerto)
                   .WithMany()
                   .HasForeignKey(x => x.IdAeropuerto);

            // ⚡ Índices / Constraints

            builder.HasIndex(x => new { x.IdVuelo, x.Orden })
                   .IsUnique()
                   .HasDatabaseName("UQ_Escala_Vuelo_Orden");

            // ⚠️ CHECK constraints (manuales en EF)
            builder.HasCheckConstraint("CK_Escala_Fechas", "fecha_hora_salida > fecha_hora_llegada");

            builder.HasCheckConstraint("CK_Escala_Duracion", "duracion_min >= 0");

            builder.HasCheckConstraint("CK_Escala_Tipo", "tipo_escala IN ('TECNICA','COMERCIAL')");

            builder.HasCheckConstraint("CK_Escala_Orden", "orden >= 1");
        }
    }
}