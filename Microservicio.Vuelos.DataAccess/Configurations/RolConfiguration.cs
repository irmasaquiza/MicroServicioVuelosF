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
            builder.ToTable("ROL", "seg");

            // 🔑 PK
            builder.HasKey(x => x.IdRol);

            builder.Property(x => x.IdRol)
                   .HasColumnName("id_rol")
                   .ValueGeneratedOnAdd();

            // 🆔 GUID
            builder.Property(x => x.RolGuid)
                   .HasColumnName("rol_guid")
                   .HasDefaultValueSql("NEWID()")
                   .IsRequired();

            // 🏷️ Campos
            builder.Property(x => x.NombreRol)
                   .IsRequired()
                   .HasMaxLength(50)
                   .HasColumnName("nombre_rol");

            builder.Property(x => x.DescripcionRol)
                   .HasMaxLength(200)
                   .HasColumnName("descripcion_rol");

            // ⚙️ Estado (CHAR 3)
            builder.Property(x => x.EstadoRol)
                   .IsRequired()
                   .HasColumnType("char(3)")
                   .HasDefaultValue("ACT")
                   .HasColumnName("estado_rol");

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

            // 🔒 Concurrencia
            builder.Property(x => x.RowVersion)
                   .IsRowVersion()
                   .HasColumnName("row_version");

            // 🔗 Relación N:M
            builder.HasMany(x => x.UsuariosRoles)
                   .WithOne(ur => ur.Rol)
                   .HasForeignKey(ur => ur.IdRol);

            // ⚡ Índices y constraints
            builder.HasIndex(x => x.NombreRol)
                   .IsUnique()
                   .HasDatabaseName("UQ_ROL_NOMBRE");

            builder.HasIndex(x => x.RolGuid)
                   .IsUnique()
                   .HasDatabaseName("UQ_ROL_GUID");
        }
    }
}