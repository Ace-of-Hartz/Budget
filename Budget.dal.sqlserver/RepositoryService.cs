using Budget.dal.i;
using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dal.sqlserver
{
    public class RepositoryService : IRepositoryService
    {
        private string _connectionString;
        public RepositoryService(string connectionString)
        {
            this._connectionString = connectionString;
        }

        public ITransactionHelper GetTransactionHelper()
        {
            return new TransactionHelper(this._connectionString);
        }
        public IAccountLedgerRepository GetAccountLedgerRepository(ITransactionHelper transactionHelper)
        {
            return new AccountLedgerRepository(transactionHelper);
        }
        public IAccountRepository GetAccountRepository(ITransactionHelper transactionHelper)
        {
            return new AccountRepository(transactionHelper);
        }
        public ITagsRepository GetTagsRepository(ITransactionHelper transactionHelper)
        {
            return new TagsRepository(transactionHelper);
        }
        public IPaycheckRepository GetPaycheckRepository(ITransactionHelper transactionHelper)
        {
            return new PaycheckRepository(transactionHelper);
        }
    }
}
