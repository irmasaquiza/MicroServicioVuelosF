using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class VueloConfiguration : IEntityTypeConfiguration<VueloEntity>
    {
        public void Configure(EntityTypeBuilder<VueloEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Vuelo", "vuelos");

            // 🔑 PK
            builder.HasKey(x => x.IdVuelo);

            builder.Property(x => x.IdVuelo)
                   .HasColumnName("id_vuelo");

            // 🔒 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdAeropuertoOrigen)
                   .HasColumnName("id_aeropuerto_origen");

            builder.Property(x => x.IdAeropuertoDestino)
                   .HasColumnName("id_aeropuerto_destino");

            // ⚠️ MAPEO REAL BD
            builder.Property(x => x.CodigoVuelo)
                   .IsRequired()
                   .HasMaxLength(10)
                   .HasColumnName("numero_vuelo");

            // 🕒 Fechas
            builder.Property(x => x.FechaHoraSalida)
                   .HasColumnName("fecha_hora_salida");

            builder.Property(x => x.FechaHoraLlegada)
                   .HasColumnName("fecha_hora_llegada");

            builder.Property(x => x.DuracionMin)
                   .HasColumnName("duracion_min");

            // 💰 Precio
            builder.Property(x => x.PrecioBase)
                   .HasColumnType("decimal(12,2)")
                   .HasColumnName("precio_base");

            // 💺 Capacidad
            builder.Property(x => x.CapacidadTotal)
                   .HasColumnName("capacidad_total");

            // ✈️ Estado vuelo
            builder.Property(x => x.EstadoVuelo)
                   .HasMaxLength(20)
                   .HasColumnName("estado_vuelo");

            // ⚙️ Estado técnico
            builder.Property(x => x.Estado)
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.EsEliminado)
                   .HasColumnName("eliminado");

            // 🧾 Auditoría
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
                   .HasMaxLength(45)
                   .HasColumnName("modificacion_ip");

            // 🔗 RELACIONES
            builder.HasOne(x => x.AeropuertoOrigen)
                   .WithMany()
                   .HasForeignKey(x => x.IdAeropuertoOrigen)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.AeropuertoDestino)
                   .WithMany()
                   .HasForeignKey(x => x.IdAeropuertoDestino)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasMany(x => x.Escalas)
                   .WithOne(e => e.Vuelo)
                   .HasForeignKey(e => e.IdVuelo);

            builder.HasMany(x => x.Asientos)
                   .WithOne(a => a.Vuelo)
                   .HasForeignKey(a => a.IdVuelo);

            builder.HasMany(x => x.Boletos)
                   .WithOne(b => b.Vuelo)
                   .HasForeignKey(b => b.IdVuelo);
        }
    }
}