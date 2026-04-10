using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class PaisConfiguration : IEntityTypeConfiguration<PaisEntity>
    {
        public void Configure(EntityTypeBuilder<PaisEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Pais", "crm");

            // 🔑 PK
            builder.HasKey(x => x.IdPais);

            builder.Property(x => x.IdPais)
                   .HasColumnName("id_pais");

            // 🌍 Campos
            builder.Property(x => x.CodigoIso2)
                   .HasMaxLength(2)
                   .HasColumnName("codigo_iso2");

            builder.Property(x => x.CodigoIso3)
                   .HasMaxLength(3)
                   .HasColumnName("codigo_iso3");

            builder.Property(x => x.Nombre)
                   .HasMaxLength(100)
                   .HasColumnName("nombre");

            builder.Property(x => x.Continente)
                   .HasMaxLength(50)
                   .HasColumnName("continente");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.Eliminado)
                   .HasColumnName("eliminado");

            // 🔗 Relaciones

            builder.HasMany(x => x.Ciudades)
                   .WithOne(c => c.Pais)
                   .HasForeignKey(c => c.IdPais);

            builder.HasMany(x => x.Aeropuertos)
                   .WithOne(a => a.Pais)
                   .HasForeignKey(a => a.IdPais);

            builder.HasMany(x => x.Clientes)
                   .WithOne(c => c.PaisNacionalidad)
                   .HasForeignKey(c => c.IdPaisNacionalidad);
        }
    }
}