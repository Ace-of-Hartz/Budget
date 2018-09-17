using System;
using System.Collections.Generic;
using System.Text;

namespace Budget.dal.i
{
    public interface IRepositoryService
    {
        ITransactionHelper GetTransactionHelper();
        IAccountRepository GetAccountRepository(ITransactionHelper transactionHelper);
        IAccountLedgerRepository GetAccountLedgerRepository(ITransactionHelper transactionHelper);
        ITagsRepository GetTagsRepository(ITransactionHelper transactionHelper);
        IPaycheckRepository GetPaycheckRepository(ITransactionHelper transactionHelper);
    }
}
