using Budget.dal.i;
using Budget.dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public class AccountRepository : IAccountRepository
    {
        private ITransactionHelper _transactionHelper;

        public AccountRepository(ITransactionHelper transactionHelper)
        {
            this._transactionHelper = transactionHelper;
        }

        public async Task CreateAccountAsync(string name, string description)
        {
            string sql = @"
INSERT INTO [Account] ( [Name], [Description] )
VALUES ( @name, @description );
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@name", name));
                _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@description", description));
                await _transactionHelper.SqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteAccountAsync(int id)
        {
            string sql = @"
DELETE FROM [Account]
WERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
                _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@id", id));
                await _transactionHelper.SqlCommand.ExecuteNonQueryAsync();
            }
        }

        public async Task<Account> GetLastAccountAsync()
        {
            string sql = @"
SELECT TOP 1 [ID], [Name], [Money], [Description]
From [Account]
ORDER BY [ID] DESC
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Account>().FirstOrDefault();
        }

        public async Task<Account> GetAccountAsync(int id)
        {
            string sql = @"
SELECT TOP 1 [Id], [Name], [Money], [Description]
FROM [Account]
WHERE [Id] = @id;
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@id", id));
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Account>().FirstOrDefault();
        }

        public async Task<IEnumerable<Account>> GetAccountsAsync()
        {
            string sql = @"
SELECT [Id], [Name], [Money], [Description]
FROM [Account];
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Account>();
        }

        public async Task<IEnumerable<Account>> GetAccountsByNameAsync(string name)
        {
            string sql = @"
SELECT [Id], [Name], [Money], [Description]
FROM [Account]
WHERE [Name] = @name;
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@name", name));
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Account>();
        }

        public async Task<IEnumerable<Account>> GetAccountsByTagAsync(string tag)
        {
            string sql = @"
SELECT [A].[Id], [Name], [Money], [A].[Description]
FROM [Account] as [A]
JOIN [Tags] [T]
 ON [T].[AccountId] = [A].[Id]
 AND [T].[Tag] = @tag;
";
            _transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction);
            _transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@tag", tag));
            _transactionHelper.SqlDataReader = await _transactionHelper.SqlCommand.ExecuteReaderAsync();
            return _transactionHelper.SqlDataReader.GetDtos<Account>();
        }

        public async Task RenameAccountAsync(int id, string name)
        {
            string sql = @"
UPDATE [Account]
SET [Name] = @name
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@name", name));
                command.Parameters.Add(new SqlParameter("@id", id));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateDescriptionAsync(int id, string description)
        {
            string sql = @"
UPDATE [Account]
SET [Description] = @description
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@description", description));
                command.Parameters.Add(new SqlParameter("@id", id));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
