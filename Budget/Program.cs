using Budget.dal.i;
using Budget.dal.sqlserver;
using Budget.service;
using System;
using System.Data.SqlClient;

namespace Budget
{
    class Program
    {
        static void Main(string[] args)
        {
            var sqlConnection = new SqlConnectionStringBuilder()
            {
                ApplicationName = "BudgetTest",
                DataSource = @"ALEKS-000\SQLEXPRESS",
                InitialCatalog = "Budget",
                IntegratedSecurity = true
            };
            using (var transactionHelper = new TransactionHelper(sqlConnection.ConnectionString))
            {
                AccountRepository accountRepository = new AccountRepository(transactionHelper);
                AccountLedgerRepository accountLedgerRepository = new AccountLedgerRepository(transactionHelper);
                TagsRepository tagsRepository = new TagsRepository(transactionHelper);

                AccountService accountService = new AccountService(transactionHelper, accountRepository, accountLedgerRepository, tagsRepository);
                Console.Out.WriteLine(accountService.CreateAccountAsync("Test 3", "Test").GetAwaiter().GetResult().Name);
                Console.Read();
                transactionHelper.CommitTransaction();
            }
        }
    }
}
