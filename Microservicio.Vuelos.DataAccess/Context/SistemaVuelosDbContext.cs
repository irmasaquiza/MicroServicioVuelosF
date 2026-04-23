using Microservicio.Vuelos.DataAccess.Configuration;       
using Microservicio.Vuelos.DataAccess.Entities;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;

namespace Microservicio.Vuelos.DataAccess.Context
{
    public class SistemaVuelosDbContext : DbContext
    {
        public SistemaVuelosDbContext(DbContextOptions<SistemaVuelosDbContext> options)
            : base(options)
        {
        }

        // =============================
        // 🧾 DbSets (Tablas)
        // =============================

        public DbSet<AeropuertoEntity> Aeropuertos { get; set; }
        public DbSet<AsientoEntity> Asientos { get; set; }
        public DbSet<AuditoriaLogEntity> AuditoriaLogs { get; set; }
        public DbSet<BoletoEntity> Boletos { get; set; }
        public DbSet<CiudadEntity> Ciudades { get; set; }
        public DbSet<ClienteEntity> Clientes { get; set; }
        public DbSet<EquipajeEntity> Equipajes { get; set; }
        public DbSet<EscalaEntity> Escalas { get; set; }
        public DbSet<FacturaEntity> Facturas { get; set; }
       // public DbSet<MetodoPagoEntity> MetodosPago { get; set; }
        public DbSet<PaisEntity> Paises { get; set; }
        public DbSet<PasajeroEntity> Pasajeros { get; set; }
        public DbSet<ReservaEntity> Reservas { get; set; }
        public DbSet<RolEntity> Roles { get; set; }
        //public DbSet<TipoMetodoPagoEntity> TiposMetodoPago { get; set; }
        public DbSet<UsuarioAppEntity> UsuariosApp { get; set; }
        public DbSet<UsuarioRolEntity> UsuariosRoles { get; set; }
        public DbSet<VueloEntity> Vuelos { get; set; }

        // =============================
        // ⚙️ Configuración Fluent API
        // =============================

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Aplicar todas las configuraciones
            modelBuilder.ApplyConfiguration(new AeropuertoConfiguration());
            modelBuilder.ApplyConfiguration(new AsientoConfiguration());
            modelBuilder.ApplyConfiguration(new AuditoriaLogConfiguration());
            modelBuilder.ApplyConfiguration(new BoletoConfiguration());
            modelBuilder.ApplyConfiguration(new CiudadConfiguration());
            modelBuilder.ApplyConfiguration(new ClienteConfiguration());
            modelBuilder.ApplyConfiguration(new EquipajeConfiguration());
            modelBuilder.ApplyConfiguration(new EscalaConfiguration());
            modelBuilder.ApplyConfiguration(new FacturaConfiguration());
           // modelBuilder.ApplyConfiguration(new MetodoPagoConfiguration());
            modelBuilder.ApplyConfiguration(new PaisConfiguration());
            modelBuilder.ApplyConfiguration(new PasajeroConfiguration());
            modelBuilder.ApplyConfiguration(new ReservaConfiguration());
            modelBuilder.ApplyConfiguration(new RolConfiguration());
           // modelBuilder.ApplyConfiguration(new TipoMetodoPagoConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioAppConfiguration());
            modelBuilder.ApplyConfiguration(new UsuarioRolConfiguration());
            modelBuilder.ApplyConfiguration(new VueloConfiguration());
        }
    }
}
