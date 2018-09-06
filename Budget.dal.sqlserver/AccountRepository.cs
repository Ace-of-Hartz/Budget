using Budget.dal.i;
using Budget.dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public class AccountRepository : IAccountRepository
    {
        private TransactionHelper _transactionHelper;

        public AccountRepository(TransactionHelper transactionHelper)
        {
            this._transactionHelper = transactionHelper;
        }

        public async Task<Account> GetAccountAsync(int id)
        {
            string sql = $@"
SELECT [Id], [Name], [Money], [Description]
FROM [Account]
WHERE [Id] = @id;
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDto<Account>();
                }
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            string sql = $@"
SELECT [Id], [Name], [Money], [Description]
FROM [Account];
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                return reader.GetDtos<Account>();
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsByNameAsync(string name)
        {
            string sql = $@"
SELECT [Id], [Name], [Money], [Description]
FROM [Account]
WHERE [Name] = @name;
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@name", name));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDtos<Account>();
                }
            }
        }

        public async Task<IEnumerable<Account>> GetAccountsByTagAsync(string tag)
        {
            string sql = $@"
SELECT [A].[Id], [Name], [Money], [A].[Description]
FROM [Account] as [A]
JOIN [Tags] [T]
 ON [T].[AccountId] = [A].[Id]
 AND [T].[Tag] = @tag;
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@tag", tag));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDtos<Account>();
                }
            }
        }
    }
}
