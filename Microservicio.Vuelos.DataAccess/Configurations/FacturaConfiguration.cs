using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class FacturaConfiguration : IEntityTypeConfiguration<FacturaEntity>
    {
        public void Configure(EntityTypeBuilder<FacturaEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Facturas", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdFactura);

            builder.Property(x => x.IdFactura)
                   .HasColumnName("id_factura");

            // 🆔 GUID
            builder.Property(x => x.GuidFactura)
                   .IsRequired()
                   .HasColumnName("guid_factura");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdCliente)
                   .IsRequired()
                   .HasColumnName("id_cliente");

            builder.Property(x => x.IdReserva)
                   .IsRequired()
                   .HasColumnName("id_reserva");

            builder.Property(x => x.IdMetodo)
                   .IsRequired()
                   .HasColumnName("id_metodo");

            // 🧾 Datos factura
            builder.Property(x => x.NumeroFactura)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("numero_factura");

            builder.Property(x => x.FechaEmision)
                   .IsRequired()
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_emision");

            // 💰 Valores económicos
            builder.Property(x => x.Subtotal)
                   .IsRequired()
                   .HasColumnType("decimal(12,2)")
                   .HasColumnName("subtotal");

            builder.Property(x => x.ValorIva)
                   .IsRequired()
                   .HasColumnType("decimal(12,2)")
                   .HasColumnName("valor_iva");

            builder.Property(x => x.CargoServicio)
                   .IsRequired()
                   .HasColumnType("decimal(12,2)")
                   .HasColumnName("cargo_servicio");

            builder.Property(x => x.Total)
                   .IsRequired()
                   .HasColumnType("decimal(12,2)")
                   .HasColumnName("total");

            builder.Property(x => x.ObservacionesFactura)
                   .HasMaxLength(255)
                   .HasColumnName("observaciones_factura");

            builder.Property(x => x.OrigenCanalFactura)
                   .HasMaxLength(50)
                   .HasColumnName("origen_canal_factura");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(10)
                   .HasColumnName("estado");

            builder.Property(x => x.FechaInhabilitacionUtc)
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_inhabilitacion_utc");

            builder.Property(x => x.EsEliminado)
                   .IsRequired()
                   .HasColumnName("es_eliminado");

            // 🧾 Auditoría
            builder.Property(x => x.CreadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .IsRequired()
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

            // 🔗 Integración
            builder.Property(x => x.ServicioOrigen)
                   .HasMaxLength(50)
                   .HasColumnName("servicio_origen");

            builder.Property(x => x.MotivoInhabilitacion)
                   .HasMaxLength(255)
                   .HasColumnName("motivo_inhabilitacion");

            // 🔗 Relaciones

            builder.HasOne(x => x.Cliente)
                   .WithMany()
                   .HasForeignKey(x => x.IdCliente);

            builder.HasOne(x => x.Reserva)
                   .WithMany(r => r.Facturas)
                   .HasForeignKey(x => x.IdReserva);

            builder.HasOne(x => x.MetodoPago)
                   .WithMany(mp => mp.Facturas)
                   .HasForeignKey(x => x.IdMetodo);

            builder.HasMany(x => x.Boletos)
                   .WithOne(b => b.Factura)
                   .HasForeignKey(b => b.IdFactura);

            // ⚡ Índices

            builder.HasIndex(x => x.GuidFactura)
                   .IsUnique()
                   .HasDatabaseName("UQ_FACTURA_GUID");

            builder.HasIndex(x => x.NumeroFactura)
                   .IsUnique()
                   .HasDatabaseName("UQ_FACTURA_NUMERO");
        }
    }
}