using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class CiudadConfiguration : IEntityTypeConfiguration<CiudadEntity>
    {
        public void Configure(EntityTypeBuilder<CiudadEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("CIUDAD", "aero");

            // 🔑 PK
            builder.HasKey(x => x.IdCiudad);

            builder.Property(x => x.IdCiudad)
                   .HasColumnName("id_ciudad");

            // 🔁 RowVersion
           /* builder.Property(x => x.RowVersion)
                   //.IsRowVersion()
                   .HasColumnName("row_version");
           */
            builder.Property(x => x.RowVersion)
                    .HasColumnName("row_version")
                    .HasDefaultValueSql("decode('00000001','hex')")
                    .ValueGeneratedOnAdd();

            // 🔗 FK
            builder.Property(x => x.IdPais)
                   .IsRequired()
                   .HasColumnName("id_pais");

            // 🏷️ Campos
            builder.Property(x => x.Nombre)
                   .IsRequired()
                   .HasMaxLength(150)
                   .HasColumnName("nombre");

            builder.Property(x => x.CodigoPostal)
                   .HasMaxLength(20)
                   .HasColumnName("codigo_postal");

            builder.Property(x => x.ZonaHoraria)
                   .HasMaxLength(50)
                   .HasColumnName("zona_horaria");

            builder.Property(x => x.Latitud)
                   .HasColumnType("decimal(9,6)")
                   .HasColumnName("latitud");

            builder.Property(x => x.Longitud)
                   .HasColumnType("decimal(9,6)")
                   .HasColumnName("longitud");

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

            // 🔗 Relaciones

            builder.HasOne(x => x.Pais)
                   .WithMany(p => p.Ciudades)
                   .HasForeignKey(x => x.IdPais)
                   .OnDelete(DeleteBehavior.Restrict);

            // ⚡ Índices

            builder.HasIndex(x => new { x.IdPais, x.Nombre })
                   .IsUnique()
                   .HasDatabaseName("UQ_CIUDAD_PAIS_NOMBRE");
        }
    }
}