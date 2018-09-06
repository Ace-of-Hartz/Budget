using Budget.dal.i;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Budget.dal.sqlserver
{
    public class TransactionHelper : ITransactionHelper
    {
        public string SqlConnectionString { get; private set; }

        public SqlConnection SqlConnection
        {
            get
            {
                if (this.SqlConnection == null)
                {
                    this.CreateSqlConnection();
                }
                return this._sqlConnection;
            }
        }
        private SqlConnection _sqlConnection;

        public SqlTransaction SqlTransaction
        {
            get
            {
                if (this._sqlTransaction == null)
                {
                    this.BeginTransaction(Guid.NewGuid().ToString("N"));
                }
                return this._sqlTransaction;
            }
        }
        private SqlTransaction _sqlTransaction;

        public TransactionHelper() { }

        public SqlConnection CreateSqlConnection()
        {
            this._sqlConnection = new SqlConnection(this.SqlConnectionString);
            return this._sqlConnection;
        }

        public SqlTransaction BeginTransaction()
        {
            if (this.SqlConnection == null) { this.CreateSqlConnection(); }

            this._sqlTransaction = this.SqlConnection.BeginTransaction();
            return this._sqlTransaction;
        }

        public SqlTransaction BeginTransaction(string transactionName)
        {
            if (this.SqlConnection == null) { this.CreateSqlConnection(); }

            if (string.IsNullOrWhiteSpace(transactionName)) { this.BeginTransaction(); }
            else { this._sqlTransaction = this.SqlConnection.BeginTransaction(transactionName); }
            return this._sqlTransaction;
        }

        public void RollbackTransactaction()
        {
            this.SqlTransaction?.Rollback();
        }

        public void CommitTransaction()
        {
            this.SqlTransaction?.Commit();
        }

        public void Dispose()
        {
            this.SqlTransaction?.Dispose();
            this.SqlConnection?.Dispose();

            this._sqlTransaction = null;
            this._sqlConnection = null;
        }
    }
}
