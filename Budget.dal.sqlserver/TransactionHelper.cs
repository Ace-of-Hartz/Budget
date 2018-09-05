using Budget.dal.i;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Budget.dal.sqlserver
{
    public class TransactionHelper : ITransactionHelper
    {
        public string SqlConnectionString { get; }

        public SqlConnection SqlConnection { get; }
        public SqlTransaction SqlTransaction
        {
            get
            {
                if(this._sqlTransaction == null)
                {
                    this.BeginTransaction(Guid.NewGuid().ToString("N"));
                }
                return this._sqlTransaction;
            }
        }

        private SqlTransaction _sqlTransaction;

        public TransactionHelper(string connectionString)
        {
            this.SqlConnectionString = connectionString;
            this.SqlConnection = new SqlConnection(SqlConnectionString);
        }

        public void BeginTransaction(string transactionName)
        {
            if (string.IsNullOrWhiteSpace(transactionName)) { this._sqlTransaction = this.SqlConnection.BeginTransaction(); }
            else { this._sqlTransaction = this.SqlConnection.BeginTransaction(); }
        }

        public void RollbackTransactaction()
        {
            this.SqlTransaction.Rollback();
        }

        public void CommitTransaction()
        {
            this.SqlTransaction.Commit();
        }

        public void Dispose()
        {
            this.SqlTransaction.Dispose();
            this.SqlConnection.Dispose();
        }
    }
}
