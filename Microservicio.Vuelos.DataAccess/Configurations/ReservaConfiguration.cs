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

            builder.Property(x => x.IdReserva).HasColumnName("id_reserva");
            builder.Property(x => x.GuidReserva).HasColumnName("guid_reserva");
            builder.Property(x => x.CodigoReserva).HasColumnName("codigo_reserva");

            // 🔗 FKs
            builder.Property(x => x.IdCliente).HasColumnName("id_cliente");
            builder.Property(x => x.IdPasajero).HasColumnName("id_pasajero");
            builder.Property(x => x.IdVuelo).HasColumnName("id_vuelo");
            builder.Property(x => x.IdAsiento).HasColumnName("id_asiento");

            // 📅 Fechas
            builder.Property(x => x.FechaReservaUtc).HasColumnName("fecha_reserva_utc");
            builder.Property(x => x.FechaInicio).HasColumnName("fecha_inicio");
            builder.Property(x => x.FechaFin).HasColumnName("fecha_fin");
            builder.Property(x => x.FechaConfirmacionUtc).HasColumnName("fecha_confirmacion_utc");
            builder.Property(x => x.FechaCancelacionUtc).HasColumnName("fecha_cancelacion_utc");

            // 💰 Valores
            builder.Property(x => x.SubtotalReserva).HasColumnName("subtotal_reserva");
            builder.Property(x => x.ValorIva).HasColumnName("valor_iva");
            builder.Property(x => x.TotalReserva).HasColumnName("total_reserva");

            // ⚙️ Estado
            builder.Property(x => x.EstadoReserva).HasColumnName("estado_reserva");
            builder.Property(x => x.OrigenCanalReserva).HasColumnName("origen_canal_reserva");
            builder.Property(x => x.MotivoCancelacion).HasColumnName("motivo_cancelacion");

            // 📞 Contacto
            builder.Property(x => x.ContactoEmail).HasColumnName("contacto_email");
            builder.Property(x => x.ContactoTelefono).HasColumnName("contacto_telefono");
            builder.Property(x => x.Observaciones).HasColumnName("observaciones");

            // 🔌 Integración
            builder.Property(x => x.ServicioOrigen).HasColumnName("servicio_origen");

            // ⚠️ Inhabilitación
            builder.Property(x => x.FechaInhabilitacionUtc).HasColumnName("fecha_inhabilitacion_utc");
            builder.Property(x => x.MotivoInhabilitacion).HasColumnName("motivo_inhabilitacion");

            // 🧾 Auditoría
            builder.Property(x => x.EsEliminado).HasColumnName("es_eliminado");
            builder.Property(x => x.CreadoPorUsuario).HasColumnName("creado_por_usuario");
            builder.Property(x => x.FechaRegistroUtc).HasColumnName("fecha_registro_utc");
            builder.Property(x => x.ModificadoPorUsuario).HasColumnName("modificado_por_usuario");
            builder.Property(x => x.FechaModificacionUtc).HasColumnName("fecha_modificacion_utc");
            builder.Property(x => x.ModificacionIp).HasColumnName("modificacion_ip");

            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔥 RELACIONES
            builder.HasOne(x => x.Cliente)
                   .WithMany(c => c.Reservas)
                   .HasForeignKey(x => x.IdCliente);

            builder.HasOne(x => x.Pasajero)
                   .WithMany(p => p.Reservas)
                   .HasForeignKey(x => x.IdPasajero);

            builder.HasOne(x => x.Vuelo)
                   .WithMany()
                   .HasForeignKey(x => x.IdVuelo);

            builder.HasOne(x => x.Asiento)
                   .WithMany()
                   .HasForeignKey(x => x.IdAsiento);

            builder.HasMany(x => x.Boletos)
                   .WithOne(b => b.Reserva)
                   .HasForeignKey(b => b.IdReserva);

            builder.HasMany(x => x.Facturas)
                   .WithOne(f => f.Reserva)
                   .HasForeignKey(f => f.IdReserva);
        }
    }
}