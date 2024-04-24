using System.Data;

namespace Location.Consumer.Worker.Domain.Abstractions.Interfaces;

public interface IUnitOfWork
{
    IDbConnection Connection { get; }

    IDbTransaction Transaction { get; }

    void BeginTransaction();
    void Commit();
    void Rollback();
}