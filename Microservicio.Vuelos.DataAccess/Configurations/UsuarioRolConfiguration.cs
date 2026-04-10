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
            builder.ToTable("UsuarioRol", "seguridad");

            // 🔑 PK (compuesta)
            builder.HasKey(x => new { x.IdUsuario, x.IdRol });

            builder.Property(x => x.IdUsuario)
                   .HasColumnName("id_usuario");

            builder.Property(x => x.IdRol)
                   .HasColumnName("id_rol");

            // 🔗 Relación con UsuarioApp
            builder.HasOne(x => x.Usuario)
                   .WithMany(u => u.UsuariosRoles)
                   .HasForeignKey(x => x.IdUsuario);

            // 🔗 Relación con Rol
            builder.HasOne(x => x.Rol)
                   .WithMany(r => r.UsuariosRoles)
                   .HasForeignKey(x => x.IdRol);
        }
    }
}
