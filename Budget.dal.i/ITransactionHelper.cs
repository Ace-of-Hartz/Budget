using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Budget.dal.i
{
    public interface ITransactionHelper : IDisposable
    {
        string SqlConnectionString { get; }
        SqlConnection SqlConnection { get; }
        SqlTransaction SqlTransaction { get; }

        void BeginTransaction(string transactionName);
        void RollbackTransactaction();
        void CommitTransaction();
    }
}
