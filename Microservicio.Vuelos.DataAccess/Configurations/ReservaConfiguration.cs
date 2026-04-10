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
            builder.ToTable("RESERVAS", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdReserva);

            builder.Property(x => x.IdReserva)
                   .HasColumnName("id_reserva")
                   .ValueGeneratedOnAdd();

            // 🆔 GUID
            builder.Property(x => x.GuidReserva)
                   .IsRequired()
                   .HasColumnName("guid_reserva")
                   .HasDefaultValueSql("NEWID()");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdCliente)
                   .HasColumnName("id_cliente");

            builder.Property(x => x.IdPasajero)
                   .HasColumnName("id_pasajero");

            builder.Property(x => x.IdVuelo)
                   .HasColumnName("id_vuelo");

            builder.Property(x => x.IdAsiento)
                   .HasColumnName("id_asiento");

            // 📌 Identificación
            builder.Property(x => x.CodigoReserva)
                   .IsRequired()
                   .HasMaxLength(40)
                   .HasColumnName("codigo_reserva");

            // 📅 Fechas
            builder.Property(x => x.FechaReservaUtc)
                   .HasColumnName("fecha_reserva_utc")
                   .HasDefaultValueSql("SYSUTCDATETIME()");

            builder.Property(x => x.FechaInicio)
                   .IsRequired()
                   .HasColumnName("fecha_inicio");

            builder.Property(x => x.FechaFin)
                   .IsRequired()
                   .HasColumnName("fecha_fin");

            builder.Property(x => x.FechaConfirmacionUtc)
                   .HasColumnName("fecha_confirmacion_utc");

            builder.Property(x => x.FechaCancelacionUtc)
                   .HasColumnName("fecha_cancelacion_utc");

            // 💰 Valores
            builder.Property(x => x.SubtotalReserva)
                   .HasColumnType("decimal(12,2)")
                   .HasDefaultValue(0)
                   .HasColumnName("subtotal_reserva");

            builder.Property(x => x.ValorIva)
                   .HasColumnType("decimal(12,2)")
                   .HasDefaultValue(0)
                   .HasColumnName("valor_iva");

            builder.Property(x => x.TotalReserva)
                   .HasColumnType("decimal(12,2)")
                   .HasDefaultValue(0)
                   .HasColumnName("total_reserva");

            // ⚙️ Estado
            builder.Property(x => x.EstadoReserva)
                   .IsRequired()
                   .HasColumnType("char(3)")
                   .HasDefaultValue("PEN")
                   .HasColumnName("estado_reserva");

            builder.Property(x => x.OrigenCanalReserva)
                   .HasMaxLength(50)
                   .HasDefaultValue("WEB")
                   .HasColumnName("origen_canal_reserva");

            builder.Property(x => x.MotivoCancelacion)
                   .HasMaxLength(250)
                   .HasColumnName("motivo_cancelacion");

            // 📞 Contacto
            builder.Property(x => x.ContactoEmail)
                   .HasMaxLength(150)
                   .HasColumnName("contacto_email");

            builder.Property(x => x.ContactoTelefono)
                   .HasMaxLength(20)
                   .HasColumnName("contacto_telefono");

            builder.Property(x => x.Observaciones)
                   .HasMaxLength(255)
                   .HasColumnName("observaciones");

            // 🔌 Integración
            builder.Property(x => x.ServicioOrigen)
                   .HasMaxLength(50)
                   .HasDefaultValue("VUELOS")
                   .HasColumnName("servicio_origen");

            // ⚠️ Inhabilitación
            builder.Property(x => x.FechaInhabilitacionUtc)
                   .HasColumnName("fecha_inhabilitacion_utc");

            builder.Property(x => x.MotivoInhabilitacion)
                   .HasMaxLength(250)
                   .HasColumnName("motivo_inhabilitacion");

            // 🧾 Auditoría
            builder.Property(x => x.EsEliminado)
                   .HasDefaultValue(false)
                   .HasColumnName("es_eliminado");

            builder.Property(x => x.CreadoPorUsuario)
                   .HasMaxLength(100)
                   .HasDefaultValue("SYSTEM")
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .HasDefaultValueSql("SYSUTCDATETIME()")
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(45)
                   .HasColumnName("modificacion_ip");

            // 🔗 Relaciones
            builder.HasOne(x => x.Cliente)
                   .WithMany()
                   .HasForeignKey(x => x.IdCliente)
                   .HasConstraintName("FK_RESERVAS_Cliente");

            builder.HasOne(x => x.Pasajero)
                   .WithMany()
                   .HasForeignKey(x => x.IdPasajero)
                   .HasConstraintName("FK_RESERVAS_Pasajero");

            builder.HasMany(x => x.Boletos)
                   .WithOne(b => b.Reserva)
                   .HasForeignKey(b => b.IdReserva);

            // ⚡ UNIQUE
            builder.HasIndex(x => x.GuidReserva)
                   .IsUnique()
                   .HasDatabaseName("UQ_RESERVAS_GUID");

            builder.HasIndex(x => x.CodigoReserva)
                   .IsUnique()
                   .HasDatabaseName("UQ_RESERVAS_CODIGO");

            builder.HasIndex(x => new { x.IdVuelo, x.IdAsiento })
                   .IsUnique()
                   .HasDatabaseName("UQ_RESERVAS_Vuelo_Asiento");

            builder.HasIndex(x => new { x.IdVuelo, x.IdPasajero })
                   .IsUnique()
                   .HasDatabaseName("UQ_RESERVAS_Vuelo_Pasajero");
        }
    }
}