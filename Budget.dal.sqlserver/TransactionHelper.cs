using Budget.dal.i;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Budget.dal.sqlserver
{
    public class TransactionHelper : ITransactionHelper
    {
        public bool IsDisposed { get; private set; }
        public string SqlConnectionString { get; private set; }

        public SqlConnection SqlConnection
        {
            get
            {
                if (this._sqlConnection == null)
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

        public SqlCommand SqlCommand { get; set; }
        public SqlDataReader SqlDataReader { get; set; }

        public TransactionHelper(string connectionString)
        {
            this.SqlConnectionString = connectionString;
        }

        public SqlConnection CreateSqlConnection()
        {
            this._sqlConnection = new SqlConnection(this.SqlConnectionString);
            this._sqlConnection.Open();
            return this._sqlConnection;
        }

        public SqlTransaction BeginTransaction()
        {
            this._sqlTransaction = this.SqlConnection.BeginTransaction();
            return this._sqlTransaction;
        }

        public SqlTransaction BeginTransaction(string transactionName)
        {
            if (string.IsNullOrWhiteSpace(transactionName)) { this.BeginTransaction(); }
            else { this._sqlTransaction = this.SqlConnection.BeginTransaction(transactionName); }
            return this._sqlTransaction;
        }

        public void RollbackTransactaction()
        {
            this.SqlDataReader?.Dispose();
            this.SqlTransaction?.Rollback();
        }

        public void CommitTransaction()
        {
            this.SqlDataReader?.Dispose();
            this.SqlTransaction?.Commit();
        }

        public void Dispose()
        {
            this.IsDisposed = true;

            this.SqlDataReader?.Dispose();
            this.SqlCommand?.Dispose();
            this._sqlTransaction?.Dispose();
            this._sqlConnection?.Dispose();

            this.SqlDataReader = null;
            this.SqlCommand = null;
            this._sqlTransaction = null;
            this._sqlConnection = null;
        }
    }
}
