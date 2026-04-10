using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class ReservaConfiguration : IEntityTypeConfiguration<ReservaEntity>
    {
        public void Configure(EntityTypeBuilder<ReservaEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Reserva", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdReserva);

            builder.Property(x => x.IdReserva)
                   .HasColumnName("id_reserva");

            // 🆔 GUID
            builder.Property(x => x.ReservaGuid)
                   .IsRequired()
                   .HasColumnName("reserva_guid");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdCliente)
                   .IsRequired()
                   .HasColumnName("id_cliente");

            builder.Property(x => x.IdPasajero)
                   .IsRequired()
                   .HasColumnName("id_pasajero");

            // 📌 Datos de reserva
            builder.Property(x => x.CodigoReserva)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("codigo_reserva");

            builder.Property(x => x.FechaReserva)
                   .IsRequired()
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_reserva");

            builder.Property(x => x.FechaLimitePago)
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_limite_pago");

            builder.Property(x => x.EstadoReserva)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado_reserva");

            builder.Property(x => x.TotalReserva)
                   .IsRequired()
                   .HasColumnType("decimal(12,2)")
                   .HasColumnName("total_reserva");

            builder.Property(x => x.ObservacionesReserva)
                   .HasMaxLength(255)
                   .HasColumnName("observaciones_reserva");

            builder.Property(x => x.CanalOrigen)
                   .HasMaxLength(20)
                   .HasColumnName("canal_origen");

            // ⚙️ Estado técnico
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

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

            // 🔗 Relaciones

            builder.HasOne(x => x.Cliente)
                   .WithMany()
                   .HasForeignKey(x => x.IdCliente);

            builder.HasOne(x => x.Pasajero)
                   .WithMany()
                   .HasForeignKey(x => x.IdPasajero);

            builder.HasMany(x => x.Boletos)
                   .WithOne(b => b.Reserva)
                   .HasForeignKey(b => b.IdReserva);

            builder.HasMany(x => x.Facturas)
                   .WithOne()
                   .HasForeignKey("id_reserva"); // ⚠️ solo si existe en BD

            // ⚡ Índices útiles

            builder.HasIndex(x => x.ReservaGuid)
                   .IsUnique()
                   .HasDatabaseName("UQ_RESERVA_GUID");

            builder.HasIndex(x => x.CodigoReserva)
                   .IsUnique()
                   .HasDatabaseName("UQ_RESERVA_CODIGO");
        }
    }
}