using Location.Consumer.Worker.Domain.Abstractions.Interfaces;
using Location.Consumer.Worker.Domain.Configuration.Exceptions;
using System.Data;

namespace Location.Consumer.Worker.Infra.Data.Contexts;

public class UnitOfWork : IUnitOfWork
{
    private int _transactionCounter;
    private bool _disposed;
    public IDbConnection Connection { get; }
    public IDbTransaction Transaction { get; protected set; }

    public UnitOfWork(IDbConnection connection)
    {
        if (connection.State is not ConnectionState.Open)
            connection.Open();

        Connection = connection;
    }
    public void BeginTransaction()
    {
        if (Connection.State is not ConnectionState.Open)
            Connection.Open();

        if (_transactionCounter == 0)
            Transaction = Connection.BeginTransaction();

        _transactionCounter++;
    }

    public void Commit()
    {
        try
        {
            TryCommit();
        }
        catch (NotOpenTransactionException)
        {
            throw;
        }
        catch (Exception)
        {
            Rollback();
            throw;
        }
    }

    public void Rollback()
    {
        if (Transaction is null)
            return;

        _transactionCounter = 0;
        Transaction.Rollback();

        ClearTransaction();
    }

    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            LogLostedTransaction();

            if (disposing)
            {
                Transaction?.Dispose();
                Connection?.Dispose();
            }
            _disposed = true;
        }
    }

    private void TryCommit()
    {
        if (Transaction is null || _transactionCounter < 0)
            throw new NotOpenTransactionException("Commit");

        _transactionCounter--;
        if (_transactionCounter > 0)
            return;

        Transaction.Commit();

        ClearTransaction();
    }

    private void ClearTransaction()
    {
        Transaction.Dispose();
        Transaction = null;
        Connection.Close();
    }

    private void LogLostedTransaction()
    {
        if (_transactionCounter == 0)
            return;
    }
}