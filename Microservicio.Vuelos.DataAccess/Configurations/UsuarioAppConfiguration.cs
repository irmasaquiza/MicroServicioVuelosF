using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("USUARIO_APP", "seg");

            // 🔑 PK
            builder.HasKey(x => x.IdUsuario);

            builder.Property(x => x.IdUsuario)
                   .HasColumnName("id_usuario")
                   .ValueGeneratedOnAdd();

            // 🆔 GUID
            builder.Property(x => x.UsuarioGuid)
                   .HasColumnName("usuario_guid")
                   .HasDefaultValueSql("NEWID()")
                   .IsRequired();

            // 🔗 FK Cliente (nullable)
            builder.Property(x => x.IdCliente)
                   .HasColumnName("id_cliente");

            builder.HasOne(x => x.Cliente)
                   .WithMany()
                   .HasForeignKey(x => x.IdCliente)
                   .HasConstraintName("FK_USUARIO_APP_CLIENTE");

            // 🏷️ Campos principales
            builder.Property(x => x.Username)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("username");

            builder.Property(x => x.Correo)
                   .IsRequired()
                   .HasMaxLength(120)
                   .HasColumnName("correo");

            builder.Property(x => x.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(500)
                   .HasColumnName("password_hash");

            builder.Property(x => x.PasswordSalt)
                   .IsRequired()
                   .HasMaxLength(250)
                   .HasColumnName("password_salt");

            builder.Property(x => x.FechaUltimoLogin)
                   .HasColumnName("fecha_ultimo_login");

            // ⚙️ Estado
            builder.Property(x => x.EstadoUsuario)
                   .IsRequired()
                   .HasColumnType("char(3)")
                   .HasDefaultValue("ACT")
                   .HasColumnName("estado_usuario");

            builder.Property(x => x.EsEliminado)
                   .IsRequired()
                   .HasDefaultValue(false)
                   .HasColumnName("es_eliminado");

            builder.Property(x => x.Activo)
                   .IsRequired()
                   .HasDefaultValue(true)
                   .HasColumnName("activo");

            // 🧾 Auditoría
            builder.Property(x => x.CreadoPorUsuario)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasDefaultValue("SYSTEM")
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .IsRequired()
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

            // 🔒 Concurrencia
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 Relación con UsuarioRol
            builder.HasMany(x => x.UsuariosRoles)
                   .WithOne(ur => ur.Usuario)
                   .HasForeignKey(ur => ur.IdUsuario);

            // ⚡ Índices (UNIQUE como en BD)
            builder.HasIndex(x => x.UsuarioGuid)
                   .IsUnique()
                   .HasDatabaseName("UQ_USUARIO_APP_GUID");

            builder.HasIndex(x => x.Username)
                   .IsUnique()
                   .HasDatabaseName("UQ_USUARIO_APP_USERNAME");

            builder.HasIndex(x => x.Correo)
                   .IsUnique()
                   .HasDatabaseName("UQ_USUARIO_APP_CORREO");
        }
    }
}