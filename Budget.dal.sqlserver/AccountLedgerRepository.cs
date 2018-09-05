using Budget.dal.i;
using Budget.dto;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;

namespace Budget.dal.sqlserver
{
    public class AccountLedgerRepository : IAccountLedgerRepository
    {
        private int _id;
        private string _tableName;
        private TransactionHelper _transactionHelper;

        public AccountLedgerRepository(int id, TransactionHelper transactionHelper)
        {
            this._id = id;
            this._transactionHelper = transactionHelper;

            this._tableName = $"AccountLedger_{this._id}";
        }

        public async Task AddLedgerEntryAsync(decimal transaction, string description = null)
        {
            string sql = $@"
INSERT INTO [{this._tableName}] ([Transaction], [Description])
VALUES (@transaction, @description);
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@transaction", transaction));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task DeleteLedgerEntryAsync(long id)
        {
            string sql = $@"
DELETE FROM [{this._tableName}]
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task<IEnumerable<AccountLedger>> GetEntriesBetweenDatesAsync(DateTimeOffset startDate, DateTimeOffset endDate)
        {
            string sql = $@"
SELECT [Id], [Transaction], [Timestamp], [Description]
FROM [{this._tableName}]
WHERE [Timestamp] >= @startDate
AND [Timestamp] <= @endDate
ORDER BY [Id];
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@startDate", startDate));
                command.Parameters.Add(new SqlParameter("@endDate", endDate));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDtos<AccountLedger>();
                }
            }
        }

        public async Task<IEnumerable<AccountLedger>> GetLastNumEntriesAsync(int lastX)
        {
            string sql = $@"
SELECT TOP (@numRows) [Id], [Transaction], [Timestamp], [Description]
FROM [{this._tableName}]
ORDER BY [Id];
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection))
            {
                command.Parameters.Add(new SqlParameter("@numRows", lastX));
                using (var reader = await command.ExecuteReaderAsync())
                {
                    return reader.GetDtos<AccountLedger>();
                }
            }
        }

        public async Task UpdateDescriptionAsync(long id, string description)
        {
            string sql = $@"
UPDATE [{this._tableName}]
SET [Description] = @description
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }

        public async Task UpdateLedgerEntryAsync(long id, decimal transaction, string description)
        {
            string sql = $@"
UPDATE [{this._tableName}]
SET [Transaction] = @transaction,
    [Description] = @description
WHERE [Id] = @id;
";
            using (var command = new SqlCommand(sql, this._transactionHelper.SqlConnection, this._transactionHelper.SqlTransaction))
            {
                command.Parameters.Add(new SqlParameter("@id", id));
                command.Parameters.Add(new SqlParameter("@transaction", transaction));
                command.Parameters.Add(new SqlParameter("@description", description));
                await command.ExecuteNonQueryAsync();
            }
        }
    }
}
