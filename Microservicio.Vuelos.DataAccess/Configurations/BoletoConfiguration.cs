using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class BoletoConfiguration : IEntityTypeConfiguration<BoletoEntity>
    {
        public void Configure(EntityTypeBuilder<BoletoEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("BOLETO", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdBoleto);

            builder.Property(x => x.IdBoleto)
                   .HasColumnName("id_boleto");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
        .HasColumnName("row_version")
        .HasDefaultValueSql("decode('00000001','hex')")
        .ValueGeneratedOnAdd();
            // 🔗 FKs
            builder.Property(x => x.IdReserva)
                   .IsRequired()
                   .HasColumnName("id_reserva");

            builder.Property(x => x.IdVuelo)
                   .IsRequired()
                   .HasColumnName("id_vuelo");

            builder.Property(x => x.IdAsiento)
                   .IsRequired(false)
                   .HasColumnName("id_asiento");

            builder.Property(x => x.IdFactura)
                   .IsRequired()
                   .HasColumnName("id_factura");

            // 🏷️ Campos
            builder.Property(x => x.CodigoBoleto)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("codigo_boleto");

            builder.Property(x => x.Clase)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("clase");

            builder.Property(x => x.PrecioVueloBase)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("precio_vuelo_base");

            builder.Property(x => x.PrecioAsientoExtra)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("precio_asiento_extra");

            builder.Property(x => x.ImpuestosBoleto)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("impuestos_boleto");

            builder.Property(x => x.CargoEquipaje)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("cargo_equipaje");

            builder.Property(x => x.PrecioFinal)
                   .HasColumnType("decimal(10,2)")
                   .HasColumnName("precio_final");

            builder.Property(x => x.EstadoBoleto)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado_boleto");

            builder.Property(x => x.FechaEmision)
                   .IsRequired()
                   .HasColumnName("fecha_emision");

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

            // 🔗 Relaciones

            builder.HasOne(x => x.Reserva)
                   .WithMany(r => r.Boletos)
                   .HasForeignKey(x => x.IdReserva)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Vuelo)
                   .WithMany(v => v.Boletos)
                   .HasForeignKey(x => x.IdVuelo)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Asiento)
                   .WithMany()
                   .HasForeignKey(x => x.IdAsiento)
                   .IsRequired(false)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Factura)
                   .WithMany(f => f.Boletos)
                   .HasForeignKey(x => x.IdFactura)
                   .OnDelete(DeleteBehavior.Restrict);

            // ⚡ Índices

            builder.HasIndex(x => x.CodigoBoleto)
                   .IsUnique()
                   .HasDatabaseName("UQ_BOLETO_CODIGO");

          //  builder.HasIndex(x => new { x.IdVuelo, x.IdAsiento })
          //         .IsUnique()
          //         .HasDatabaseName("UQ_BOLETO_VUELO_ASIENTO");
        }
    }
}