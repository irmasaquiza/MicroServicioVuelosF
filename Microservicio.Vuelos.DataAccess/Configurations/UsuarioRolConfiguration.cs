using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class UsuarioRolConfiguration : IEntityTypeConfiguration<UsuarioRolEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioRolEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("USUARIOS_ROLES", "seg");

            // 🔑 PK REAL
            builder.HasKey(x => x.IdUsuarioRol);

            builder.Property(x => x.IdUsuarioRol)
                   .HasColumnName("id_usuario_rol")
                   .ValueGeneratedOnAdd();

            // 🔗 FK
            builder.Property(x => x.IdUsuario)
                   .HasColumnName("id_usuario")
                   .IsRequired();

            builder.Property(x => x.IdRol)
                   .HasColumnName("id_rol")
                   .IsRequired();

            // ⚙️ Estado
            builder.Property(x => x.EstadoUsuarioRol)
                   .IsRequired()
                   .HasColumnType("char(3)")
                   .HasDefaultValue("ACT")
                   .HasColumnName("estado_usuario_rol");

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
                   .HasDefaultValueSql("CURRENT_TIMESTAMP")
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnName("fecha_modificacion_utc");

            // 🔒 Concurrencia
            builder.Property(x => x.RowVersion)
        .HasColumnName("row_version")
        .HasDefaultValueSql("decode('00000001','hex')")
        .ValueGeneratedOnAdd();

            // 🔗 Relaciones
            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.UsuariosRoles)
                   .HasForeignKey(x => x.IdUsuario)
                   .HasConstraintName("FK_USUARIOS_ROLES_USUARIO");

            builder.HasOne(x => x.Rol)
                   .WithMany(r => r.UsuariosRoles)
                   .HasForeignKey(x => x.IdRol)
                   .HasConstraintName("FK_USUARIOS_ROLES_ROL");

            // ⚡ UNIQUE (clave lógica)
            builder.HasIndex(x => new { x.IdUsuario, x.IdRol })
                   .IsUnique()
                   .HasDatabaseName("UQ_USUARIOS_ROLES_USR_ROL");

            // ⚡ Índices
            builder.HasIndex(x => x.IdUsuario)
                   .HasDatabaseName("IX_USUARIOS_ROLES_USUARIO");

            builder.HasIndex(x => x.IdRol)
                   .HasDatabaseName("IX_USUARIOS_ROLES_ROL");
        }
    }
}