using Core.Persistence.Interfaces;
using Npgsql;
using System.Data;

namespace Core.Persistence
{
    public sealed class UnitOfWork : IUnitOfWork
    {
        private readonly IDbConnection _connection;
        private IDbTransaction? _transaction;
        private bool _disposed;

        public UnitOfWork(string connectionString)
        {
            _connection = new NpgsqlConnection(connectionString);
            _connection.Open();
        }

        public IDbConnection Connection => _connection;
        public IDbTransaction Transaction => _transaction ?? throw new InvalidOperationException("Transaction has not been started.");

        public void Begin()
        {
            if (_transaction != null)
                throw new InvalidOperationException("Transaction has already been started.");

            _transaction = _connection.BeginTransaction();
        }

        public void Commit()
        {
            _transaction?.Commit();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Rollback()
        {
            _transaction?.Rollback();
            _transaction?.Dispose();
            _transaction = null;
        }

        public void Dispose()
        {
            if (_disposed) return;

            _transaction?.Dispose();
            _connection.Dispose();
            _disposed = true;
        }
    }
}
