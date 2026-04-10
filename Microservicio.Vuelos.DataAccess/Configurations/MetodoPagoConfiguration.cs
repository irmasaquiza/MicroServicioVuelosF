using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class MetodoPagoConfiguration : IEntityTypeConfiguration<MetodoPagoEntity>
    {
        public void Configure(EntityTypeBuilder<MetodoPagoEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("MetodoPago", "ventas");

            // 🔑 PK
            builder.HasKey(x => x.IdMetodo);

            builder.Property(x => x.IdMetodo)
                   .HasColumnName("id_metodo");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 FK
            builder.Property(x => x.IdCliente)
                   .IsRequired()
                   .HasColumnName("id_cliente");

            builder.Property(x => x.IdTipoMetodo)
                   .IsRequired()
                   .HasColumnName("id_tipo_metodo");

            // 💳 Datos del método
            builder.Property(x => x.Ultimos4)
                   .HasColumnType("char(4)")
                   .HasColumnName("ultimos4");

            builder.Property(x => x.ReferenciaVisible)
                   .HasMaxLength(150)
                   .HasColumnName("referencia_visible");

            builder.Property(x => x.TokenPasarela)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("token_pasarela");

            builder.Property(x => x.FechaExpiracion)
                   .HasColumnType("date")
                   .HasColumnName("fecha_expiracion");

            builder.Property(x => x.NombreTitular)
                   .HasMaxLength(150)
                   .HasColumnName("nombre_titular");

            builder.Property(x => x.MarcaTarjeta)
                   .HasMaxLength(50)
                   .HasColumnName("marca_tarjeta");

            builder.Property(x => x.BancoEmisor)
                   .HasMaxLength(100)
                   .HasColumnName("banco_emisor");

            builder.Property(x => x.PaisEmision)
                   .HasMaxLength(100)
                   .HasColumnName("pais_emision");

            builder.Property(x => x.EsPrincipal)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("es_principal");

            builder.Property(x => x.Alias)
                   .HasMaxLength(100)
                   .HasColumnName("alias");

            builder.Property(x => x.FechaUltimoUso)
                   .HasColumnType("datetime2(0)")
                   .HasColumnName("fecha_ultimo_uso");

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

            // 🧾 Auditoría
            builder.Property(x => x.CreadoPorUsuario)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasDefaultValue("SYSTEM")
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .IsRequired()
                   .HasDefaultValueSql("SYSUTCDATETIME()")
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

            builder.HasOne(x => x.TipoMetodoPago)
                   .WithMany()
                   .HasForeignKey(x => x.IdTipoMetodo);

            builder.HasMany(x => x.Facturas)
                   .WithOne()
                   .HasForeignKey("id_metodo"); // ⚠️ solo si tu Factura usa este FK

            // ⚡ CHECK constraint
            builder.HasCheckConstraint(
                "CHK_MetodoPago_Estado",
                "estado IN ('ACTIVO','EXPIRADO','BLOQUEADO')"
            );
        }
    }
}