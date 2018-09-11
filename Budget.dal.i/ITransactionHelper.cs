using System;
using System.Collections.Generic;
using System.Text;
using System.Data.SqlClient;

namespace Budget.dal.i
{
    public interface ITransactionHelper : IDisposable
    {
        bool IsDisposed { get; }
        string SqlConnectionString { get; }
        SqlConnection SqlConnection { get; }
        SqlTransaction SqlTransaction { get; }
        SqlCommand SqlCommand { get; set; }
        SqlDataReader SqlDataReader { get; set; }

        SqlConnection CreateSqlConnection();
        SqlTransaction BeginTransaction();
        SqlTransaction BeginTransaction(string transactionName);
        void RollbackTransactaction();
        void CommitTransaction();
    }
}
