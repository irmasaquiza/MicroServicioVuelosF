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
            builder.ToTable("UsuarioApp", "seguridad");

            // 🔑 PK
            builder.HasKey(x => x.IdUsuario);

            builder.Property(x => x.IdUsuario)
                   .HasColumnName("id_usuario");

            // 🏷️ Campos principales
            builder.Property(x => x.Username)
                   .IsRequired()
                   .HasMaxLength(100)
                   .HasColumnName("username");

            builder.Property(x => x.Email)
                   .IsRequired()
                   .HasMaxLength(150)
                   .HasColumnName("email");

            builder.Property(x => x.PasswordHash)
                   .IsRequired()
                   .HasMaxLength(255)
                   .HasColumnName("password_hash");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.EsEliminado)
                   .IsRequired()
                   .HasColumnName("es_eliminado");

            // 🔗 Relación con UsuarioRol
            builder.HasMany(x => x.UsuariosRoles)
                   .WithOne(ur => ur.Usuario)
                   .HasForeignKey(ur => ur.IdUsuario);

            // ⚡ Índices útiles
            builder.HasIndex(x => x.Username)
                   .HasDatabaseName("IX_UsuarioApp_Username");

            builder.HasIndex(x => x.Email)
                   .HasDatabaseName("IX_UsuarioApp_Email");
        }
    }
}