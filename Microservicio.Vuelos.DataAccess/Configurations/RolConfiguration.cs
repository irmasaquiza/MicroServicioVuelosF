using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class RolConfiguration : IEntityTypeConfiguration<RolEntity>
    {
        public void Configure(EntityTypeBuilder<RolEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("Rol", "seguridad");

            // 🔑 PK
            builder.HasKey(x => x.IdRol);

            builder.Property(x => x.IdRol)
                   .HasColumnName("id_rol");

            // 🏷️ Campos
            builder.Property(x => x.NombreRol)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("nombre_rol");

            builder.Property(x => x.DescripcionRol)
                   .HasMaxLength(255)
                   .HasColumnName("descripcion_rol");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.EsEliminado)
                   .IsRequired()
                   .HasColumnName("es_eliminado");

            // 🔗 Relación N:M con UsuarioRol
            builder.HasMany(x => x.UsuariosRoles)
                   .WithOne(ur => ur.Rol)
                   .HasForeignKey(ur => ur.IdRol);

            // ⚡ Índice útil
            builder.HasIndex(x => x.NombreRol)
                   .HasDatabaseName("IX_ROL_NOMBRE");
        }
    }
}