using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class UsuarioAppConfiguration : IEntityTypeConfiguration<UsuarioAppEntity>
    {
        public void Configure(EntityTypeBuilder<UsuarioAppEntity> builder)
        {
            builder.ToTable("USUARIO_APP", "seg");

            builder.HasKey(x => x.IdUsuario);

            builder.Property(x => x.IdUsuario)
                   .HasColumnName("id_usuario")
                   .ValueGeneratedOnAdd();

            builder.Property(x => x.UsuarioGuid)
                   .HasColumnName("usuario_guid")
                   .HasDefaultValueSql("gen_random_uuid()")
                   .IsRequired();

            builder.Property(x => x.IdCliente)
                   .HasColumnName("id_cliente");

            // 🔥 🔥 🔥 AQUÍ ESTÁ LA CORRECCIÓN
            builder.HasOne(x => x.Cliente)
                   .WithMany(c => c.UsuariosApp)
                   .HasForeignKey(x => x.IdCliente)
                   .HasConstraintName("FK_USUARIO_APP_CLIENTE");

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

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(45)
                   .HasColumnName("modificacion_ip");

            builder.Property(x => x.RowVersion)
        .HasColumnName("row_version")
        .HasDefaultValueSql("decode('00000001','hex')")
        .ValueGeneratedOnAdd();

            builder.HasMany(x => x.UsuariosRoles)
                   .WithOne(ur => ur.Usuario)
                   .HasForeignKey(ur => ur.IdUsuario);

            builder.HasIndex(x => x.UsuarioGuid).IsUnique();
            builder.HasIndex(x => x.Username).IsUnique();
            builder.HasIndex(x => x.Correo).IsUnique();
        }
    }
}