using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class TipoMetodoPagoConfiguration : IEntityTypeConfiguration<TipoMetodoPagoEntity>
    {
        public void Configure(EntityTypeBuilder<TipoMetodoPagoEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("TipoMetodoPago", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdTipoMetodo);

            builder.Property(x => x.IdTipoMetodo)
                   .HasColumnName("id_tipo_metodo")
                   .ValueGeneratedOnAdd();

            // 🏷️ Campos reales
            builder.Property(x => x.NombreTipo)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("nombre_tipo");

            builder.Property(x => x.Descripcion)
                   .HasMaxLength(150)
                   .HasColumnName("descripcion");

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

            // 🔗 Relación
            builder.HasMany(x => x.MetodosPago)
                   .WithOne(mp => mp.TipoMetodoPago)
                   .HasForeignKey(mp => mp.IdTipoMetodo);

            // ⚡ UNIQUE real de la BD
            builder.HasIndex(x => x.NombreTipo)
                   .IsUnique()
                   .HasDatabaseName("UQ_TipoMetodoPago_Nombre");
        }
    }
}