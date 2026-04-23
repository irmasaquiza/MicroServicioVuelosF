using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore.Storage;
using Microservicio.Vuelos.DataAccess.Context;
using Microservicio.Vuelos.DataAccess.Repositories;
using Microservicio.Vuelos.DataAccess.Repositories.Interfaces;
using Microservicio.Vuelos.DataManagement.Interfaces;

namespace Microservicio.Vuelos.DataManagement.UoW
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SistemaVuelosDbContext _context;
        private IDbContextTransaction _transaction;

        // ─────────────────────────────
        // REPOSITORIOS
        // ─────────────────────────────
        public IPaisRepository PaisRepository { get; }
        public ICiudadRepository CiudadRepository { get; }
        public IAeropuertoRepository AeropuertoRepository { get; }
        public IClienteRepository ClienteRepository { get; }
        public IAuditoriaLogRepository AuditoriaLogRepository { get; }
        public IUsuarioAppRepository UsuarioAppRepository { get; }
        public IRolRepository RolRepository { get; }
        public IUsuarioRolRepository UsuarioRolRepository { get; }
    //    public ITipoMetodoPagoRepository TipoMetodoPagoRepository { get; }
    //    public IMetodoPagoRepository MetodoPagoRepository { get; }
        public IPasajeroRepository PasajeroRepository { get; }
        public IReservaRepository ReservaRepository { get; }
        public IFacturaRepository FacturaRepository { get; }
        public IBoletoRepository BoletoRepository { get; }
        public IEquipajeRepository EquipajeRepository { get; }
        public IVueloRepository VueloRepository { get; }
        public IEscalaRepository EscalaRepository { get; }
        public IAsientoRepository AsientoRepository { get; }

        public UnitOfWork(SistemaVuelosDbContext context)
        {
            _context = context;

            PaisRepository = new PaisRepository(_context);
            CiudadRepository = new CiudadRepository(_context);
            AeropuertoRepository = new AeropuertoRepository(_context);
            ClienteRepository = new ClienteRepository(_context);
            AuditoriaLogRepository = new AuditoriaLogRepository(_context);
            UsuarioAppRepository = new UsuarioAppRepository(_context);
            RolRepository = new RolRepository(_context);
            UsuarioRolRepository = new UsuarioRolRepository(_context);
        //    TipoMetodoPagoRepository = new TipoMetodoPagoRepository(_context);
        //    MetodoPagoRepository = new MetodoPagoRepository(_context);
            PasajeroRepository = new PasajeroRepository(_context);
            ReservaRepository = new ReservaRepository(_context);
            FacturaRepository = new FacturaRepository(_context);
            BoletoRepository = new BoletoRepository(_context);
            EquipajeRepository = new EquipajeRepository(_context);
            VueloRepository = new VueloRepository(_context);
            EscalaRepository = new EscalaRepository(_context);
            AsientoRepository = new AsientoRepository(_context);
        }

        // ─────────────────────────────
        // SAVE CHANGES (SOLO CUANDO NO HAY TRANSACCIÓN)
        // ─────────────────────────────
        public async Task<int> SaveChangesAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException(
                    "No se debe llamar SaveChangesAsync dentro de una transacción activa. Use CommitAsync.");

            return await _context.SaveChangesAsync();
        }

        // ─────────────────────────────
        // TRANSACCIONES
        // ─────────────────────────────
        public async Task BeginTransactionAsync()
        {
            if (_transaction != null)
                throw new InvalidOperationException(
                    "Ya existe una transacción activa.");

            _transaction = await _context.Database.BeginTransactionAsync();
        }

        public async Task CommitAsync()
        {
            if (_transaction == null)
                throw new InvalidOperationException(
                    "No hay una transacción activa para confirmar.");

            try
            {
                await _context.SaveChangesAsync();
                await _transaction.CommitAsync();
            }
            catch (Exception)
            {
                await RollbackAsync();
                throw;
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        public async Task RollbackAsync()
        {
            if (_transaction == null)
                return;

            try
            {
                await _transaction.RollbackAsync();
            }
            finally
            {
                await _transaction.DisposeAsync();
                _transaction = null;
            }
        }

        // ─────────────────────────────
        // DISPOSE
        // ─────────────────────────────
        public void Dispose()
        {
            _transaction?.Dispose();
            _context.Dispose();
        }
    }
}