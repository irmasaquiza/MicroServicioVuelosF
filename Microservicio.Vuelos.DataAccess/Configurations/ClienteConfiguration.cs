using System;
using System.Collections.Generic;
using System.Text;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microservicio.Vuelos.DataAccess.Entities;

namespace Microservicio.Vuelos.DataAccess.Configuration
{
    public class ClienteConfiguration : IEntityTypeConfiguration<ClienteEntity>
    {
        public void Configure(EntityTypeBuilder<ClienteEntity> builder)
        {
            // 🗂️ Tabla
            builder.ToTable("CLIENTES", "crm");

            // 🔑 PK
            builder.HasKey(x => x.IdCliente);

            builder.Property(x => x.IdCliente)
                   .HasColumnName("id_cliente");

            // 🔁 RowVersion
            builder.Property(x => x.RowVersion)
        .HasColumnName("row_version")
        .HasDefaultValueSql("decode('00000001','hex')")
        .ValueGeneratedOnAdd();

            // 🆔 GUID
            builder.Property(x => x.ClienteGuid)
                   .IsRequired()
                   .HasColumnName("cliente_guid");

            // 🏷️ Identificación
            builder.Property(x => x.TipoIdentificacion)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("tipo_identificacion");

            builder.Property(x => x.NumeroIdentificacion)
                   .IsRequired()
                   .HasMaxLength(30)
                   .HasColumnName("numero_identificacion");

            // 👤 Datos personales
            builder.Property(x => x.Nombres)
                   .HasMaxLength(100)
                   .HasColumnName("nombres");

            builder.Property(x => x.Apellidos)
                   .HasMaxLength(100)
                   .HasColumnName("apellidos");

            builder.Property(x => x.RazonSocial)
                   .HasMaxLength(150)
                   .HasColumnName("razon_social");

            // 📧 Contacto
            builder.Property(x => x.Correo)
                   .HasMaxLength(150)
                   .HasColumnName("correo");

            builder.Property(x => x.Telefono)
                   .HasMaxLength(20)
                   .HasColumnName("telefono");

            builder.Property(x => x.Direccion)
                   .HasMaxLength(200)
                   .HasColumnName("direccion");

            // 🔗 FK
            builder.Property(x => x.IdCiudadResidencia)
                   .IsRequired()
                   .HasColumnName("id_ciudad_residencia");

            builder.Property(x => x.IdPaisNacionalidad)
                   .IsRequired()
                   .HasColumnName("id_pais_nacionalidad");

            // 📌 Datos adicionales
            builder.Property(x => x.FechaNacimiento)
                   .HasColumnName("fecha_nacimiento");

            builder.Property(x => x.Nacionalidad)
                   .HasMaxLength(80)
                   .HasColumnName("nacionalidad");

            builder.Property(x => x.Genero)
                   .HasMaxLength(20)
                   .HasColumnName("genero");

            // ⚙️ Estado
            builder.Property(x => x.Estado)
                   .IsRequired()
                   .HasMaxLength(20)
                   .HasColumnName("estado");

            builder.Property(x => x.EsEliminado)
                   .HasColumnName("es_eliminado");

            // 🧾 Auditoría
            builder.Property(x => x.CreadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("creado_por_usuario");

            builder.Property(x => x.FechaRegistroUtc)
                   .HasColumnName("fecha_registro_utc");

            builder.Property(x => x.ModificadoPorUsuario)
                   .HasMaxLength(100)
                   .HasColumnName("modificado_por_usuario");

            builder.Property(x => x.FechaModificacionUtc)
                   .HasColumnName("fecha_modificacion_utc");

            builder.Property(x => x.ModificacionIp)
                   .HasMaxLength(50)
                   .HasColumnName("modificacion_ip");

            // 🔗 Integración
            builder.Property(x => x.ServicioOrigen)
                   .HasMaxLength(50)
                   .HasColumnName("servicio_origen");

            builder.Property(x => x.FechaInhabilitacionUtc)
                   .HasColumnName("fecha_inhabilitacion_utc");

            builder.Property(x => x.MotivoInhabilitacion)
                   .HasMaxLength(200)
                   .HasColumnName("motivo_inhabilitacion");

            // 🔗 Relaciones

            builder.HasOne(x => x.CiudadResidencia)
                   .WithMany()
                   .HasForeignKey(x => x.IdCiudadResidencia)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.PaisNacionalidad)
                   .WithMany(p => p.Clientes)
                   .HasForeignKey(x => x.IdPaisNacionalidad)
                   .OnDelete(DeleteBehavior.Restrict);

            // ⚡ Índices

            builder.HasIndex(x => x.ClienteGuid)
                   .IsUnique()
                   .HasDatabaseName("UQ_CLIENTE_GUID");

            builder.HasIndex(x => x.NumeroIdentificacion)
                   .IsUnique()
                   .HasDatabaseName("UQ_CLIENTE_IDENTIFICACION");

            builder.HasIndex(x => x.Correo)
                   .HasDatabaseName("IX_CLIENTE_CORREO");
        }
    }
}