using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class AsientoConfiguration : IEntityTypeConfiguration<AsientoEntity>
    {
        public void Configure(EntityTypeBuilder<AsientoEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("ASIENTO", "vuelos");

            // 🔑 PK
            builder.HasKey(x => x.IdAsiento);

            builder.Property(x => x.IdAsiento)
                   .HasColumnName("id_asiento");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdVuelo)
                   .IsRequired()
                   .HasColumnName("id_vuelo");

            // 🏷️ Campos
            builder.Property(x => x.NumeroAsiento)
                   .IsRequired()
                   .HasMaxLength(5)
                   .HasColumnName("numero_asiento");

            builder.Property(x => x.Clase)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("clase");



            builder.Property(x => x.PrecioExtra)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("precio_extra");
            
            builder.Property(x => x.Disponible)
                   .HasColumnName("disponible")
                   .IsRequired()
                   .HasDefaultValue(true);

            builder.Property(x => x.Posicion)
                   .HasColumnName("posicion")
                   .HasMaxLength(20)
                   .IsRequired(false);

            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.Eliminado)
                   .HasColumnName("eliminado");

            builder.Property(x => x.FechaRegistroUtc)
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.CreadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(50)
                   .HasColumnName("modificacion_ip");

            // 🔗 Relación
            builder.HasOne(x => x.Vuelo)
                   .WithMany(v => v.Asientos)
                   .HasForeignKey(x => x.IdVuelo)
                   .OnDelete(DeleteBehavior.Cascade);

            // ⚡ Índices
            builder.HasIndex(x => new { x.IdVuelo, x.NumeroAsiento })
                   .IsUnique()
                   .HasDatabaseName("UQ_ASIENTO_VUELO_NUMERO");
        }
    }
}