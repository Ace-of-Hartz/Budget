using Budget.dal.i;
using Budget.dal.sqlserver;
using Budget.service;
using Budget.service.i;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;

namespace Budget.Api.Dependencies
{
    public static class RepositoryServiceProvider
    {
        public static string SqlConnection => new SqlConnectionStringBuilder()
        {
            ApplicationName = "BudgetTest",
            DataSource = @"ALEKS-000\SQLEXPRESS",
            InitialCatalog = "Budget",
            IntegratedSecurity = true
        }.ConnectionString;

        public static IRepositoryService RepositoryService => new RepositoryService(RepositoryServiceProvider.SqlConnection);
        public static IAccountService AccountService => new AccountService(RepositoryService);
    }
}
