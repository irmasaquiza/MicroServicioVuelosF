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
                   .HasColumnName("id_tipo_metodo");

            // 🏷️ Campos
            builder.Property(x => x.Codigo)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("codigo");

            builder.Property(x => x.Nombre)
                   .IsRequired()
                   .HasMaxLength(150)
                   .HasColumnName("nombre");

            builder.Property(x => x.Descripcion)
                   .HasMaxLength(255)
                   .HasColumnName("descripcion");

            builder.Property(x => x.RequiereAutorizacionExterna)
                   .IsRequired()
                   .HasColumnName("requiere_autorizacion_externa");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.EsEliminado)
                   .IsRequired()
                   .HasColumnName("es_eliminado");

            // 🔗 Relación con MetodoPago
            builder.HasMany(x => x.MetodosPago)
                   .WithOne(mp => mp.TipoMetodoPago)
                   .HasForeignKey(mp => mp.IdTipoMetodo);

            // ⚡ Índice útil (sin inventar lógica, solo performance)
            builder.HasIndex(x => x.Codigo)
                   .HasDatabaseName("IX_TipoMetodoPago_Codigo");
        }
    }
}
