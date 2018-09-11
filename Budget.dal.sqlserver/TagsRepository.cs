using Budget.dal.i;
using Budget.dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public class TagsRepository : ITagsRepository
    {
        private ITransactionHelper _transactionHelper;

        public TagsRepository(ITransactionHelper transactionHelper)
        {
            this._transactionHelper = transactionHelper;
        }

        public async Task AddTagAsync(int accountId, string tag, string description)
        {
            string sql = @"
INSERT INTO [Tags] ( [AccountId], [Tag], [Description] )
VALUES ( @accountId, @tag, @description )
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@accountId", accountId));
                command.Parameters.Add(new SqlParameter("@tag", tag));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<Tags>> GetTagsAsync()
        {
            string sql = @"
SELECT [Id], [AccountId], [Tag], [Description]
FROM [Tags];
";
            this._transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection);
            this._transactionHelper.SqlDataReader = await this._transactionHelper.SqlCommand.ExecuteReaderAsync();
            return this._transactionHelper.SqlDataReader.GetDtos<Tags>();
        }

        public async Task<IEnumerable<Tags>> GetTagsForAccountAsync(int accountId)
        {
            string sql = @"
SELECT [Id], [AccountId], [Tag], [Description]
FROM [Tags]
WHERE [AccountId] = @accountId;
";
            this._transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection);
            this._transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@accountId", accountId));
            this._transactionHelper.SqlDataReader = await this._transactionHelper.SqlCommand.ExecuteReaderAsync();
            return this._transactionHelper.SqlDataReader.GetDtos<Tags>();
        }

        public async Task<IEnumerable<Tags>> GetTagsThatContainAsync(string tag)
        {
            string sql = $@"
SELECT [Id], [AccountId], [Tag], [Description]
FROM [Tags]
WHERE [Tag] like '%' + @tag + '%';
";
            this._transactionHelper.SqlCommand = new SqlCommand(sql, this._transactionHelper.SqlConnection);
            this._transactionHelper.SqlCommand.Parameters.Add(new SqlParameter("@tag", tag));
            this._transactionHelper.SqlDataReader = await this._transactionHelper.SqlCommand.ExecuteReaderAsync();
            return this._transactionHelper.SqlDataReader.GetDtos<Tags>();
        }

        public async Task RemoveTagAsync(int id)
        {
            string sql = $@"
DELETE FROM [Tags]
WHERE [Id] = @id
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateDescriptionAsync(int id, string description)
        {
            string sql = $@"
UPDATE [Tags]
SET [Description] = @description
WHERE [Id] = @id
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
