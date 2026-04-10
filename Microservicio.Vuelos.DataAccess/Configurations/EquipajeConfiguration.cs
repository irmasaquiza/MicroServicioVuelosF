using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class EquipajeConfiguration : IEntityTypeConfiguration<EquipajeEntity>
    {
        public void Configure(EntityTypeBuilder<EquipajeEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("EQUIPAJE", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdEquipaje);

            builder.Property(x => x.IdEquipaje)
                   .HasColumnName("id_equipaje");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdBoleto)
                   .IsRequired()
                   .HasColumnName("id_boleto");

            // 🏷️ Campos
            builder.Property(x => x.Tipo)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("tipo");

            builder.Property(x => x.PesoKg)
                   .HasColumnType("decimal(5,2)")
                   .HasColumnName("peso_kg");

            builder.Property(x => x.DescripcionEquipaje)
                   .HasMaxLength(200)
                   .HasColumnName("descripcion_equipaje");

            builder.Property(x => x.PrecioExtra)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("precio_extra");

            builder.Property(x => x.DimensionesCm)
                   .HasMaxLength(50)
                   .HasColumnName("dimensiones_cm");

            builder.Property(x => x.NumeroEtiqueta)
                   .HasMaxLength(50)
                   .HasColumnName("numero_etiqueta");

            builder.Property(x => x.EstadoEquipaje)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado_equipaje");

            builder.Property(x => x.EsEliminado)
                   .HasColumnName("es_eliminado");

            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.CreadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(50)
                   .HasColumnName("modificacion_ip");

            // 🔗 Relación

            builder.HasOne(x => x.Boleto)
                   .WithMany()
                   .HasForeignKey(x => x.IdBoleto)
                   .OnDelete(DeleteBehavior.Cascade);

            // ⚡ Índices

            builder.HasIndex(x => x.NumeroEtiqueta)
                   .IsUnique()
                   .HasDatabaseName("UQ_EQUIPAJE_ETIQUETA");
        }
    }
}