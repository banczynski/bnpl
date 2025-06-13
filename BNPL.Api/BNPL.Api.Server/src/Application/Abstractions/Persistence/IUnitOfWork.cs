using System.Data;

namespace BNPL.Api.Server.src.Application.Abstractions.Persistence
{
    public interface IUnitOfWork : IDisposable
    {
        IDbConnection Connection { get; }
        IDbTransaction Transaction { get; }

        void Begin();
        void Commit();
        void Rollback();
    }
}
