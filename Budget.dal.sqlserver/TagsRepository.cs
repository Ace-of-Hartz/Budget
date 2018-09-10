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
        private TransactionHelper _transactionHelper;

        public TagsRepository(TransactionHelper transactionHelper)
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

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            using (var reader = await command.ExecuteReaderAsync())
            {
                return reader.GetDtos<Tags>();
            }
        }

        public async Task<IEnumerable<Tags>> GetTagsForAccountAsync(int accountId)
        {
            string sql = @"
SELECT [Id], [AccountId], [Tag], [Description]
FROM [Tags]
WHERE [AccountId] = @accountId;
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@accountId", accountId));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDtos<Tags>();
                }
            }
        }

        public async Task<IEnumerable<Tags>> GetTagsThatContainAsync(string tag)
        {
            string sql = $@"
SELECT [Id], [AccountId], [Tag], [Description]
FROM [Tags]
WHERE [Tag] like '%' + @tag + '%';
";

            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@tag", tag));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDtos<Tags>();
                }
            }
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
